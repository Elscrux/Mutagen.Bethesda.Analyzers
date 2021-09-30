﻿using System;
using System.IO;
using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.Engines;
using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Analyzers.Testing.AutoFixture;
using Mutagen.Bethesda.Environments;
using Mutagen.Bethesda.Environments.DI;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Plugins.Records.DI;
using NSubstitute;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Tests
{
    public class EngineTests
    {
        [Theory, NSubData]
        public void IsolatedEngineCallsRecordAnalyzers(
            ModPath modPath,
            IModGetter modGetter,
            IReportDropbox reportDropbox,
            IIsolatedDriver[] drivers,
            IsolatedEngine sut)
        {
            sut.IsolatedDrivers.Drivers.Returns(drivers);
            sut.ModImporter.Import(modPath).Returns(modGetter);

            sut.RunOn(modPath, reportDropbox);

            foreach (var driver in drivers)
            {
                driver.Received(1).Drive(Arg.Is<IsolatedDriverParams>(
                    x => x.ReportDropbox == reportDropbox
                         && x.TargetMod == modGetter
                         && x.TargetModPath == modPath));
            }
        }

        [Theory, NSubData]
        public void ContextualEngineCallsIsolatedRecordAnalyzers(
            IModGetter modA,
            IModGetter modB,
            IReportDropbox reportDropbox,
            IIsolatedDriver[] drivers,
            IGameEnvironmentState gameEnv,
            ContextualEngine sut)
        {
            var loadOrder = new LoadOrder<IModListingGetter<IModGetter>>();
            loadOrder.Add(new ModListing<IModGetter>(modA));
            loadOrder.Add(new ModListing<IModGetter>(modB));

            gameEnv.LoadOrder.Returns(loadOrder);
            sut.EnvGetter.Construct().ReturnsForAnyArgs(gameEnv);
            sut.IsolatedModDrivers.Drivers.Returns(drivers);

            sut.Run(reportDropbox);

            var modAPath = new ModPath(Path.Combine(sut.DataDirectoryProvider.Path, modA.ModKey.FileName));
            var modBPath = new ModPath(Path.Combine(sut.DataDirectoryProvider.Path, modB.ModKey.FileName));

            foreach (var driver in drivers)
            {
                driver.Received(1).Drive(Arg.Is<IsolatedDriverParams>(
                    x => x.ReportDropbox == reportDropbox
                         && x.TargetMod == modA
                         && x.TargetModPath == modAPath));
                driver.Received(1).Drive(Arg.Is<IsolatedDriverParams>(
                    x => x.ReportDropbox == reportDropbox
                         && x.TargetMod == modB
                         && x.TargetModPath == modBPath));
            }
        }

        [Theory, NSubData]
        public void ContextualEngineCallsContextualRecordAnalyzers(
            IModGetter modA,
            IModGetter modB,
            IReportDropbox reportDropbox,
            IContextualDriver[] drivers,
            IGameEnvironmentState gameEnv,
            IGameEnvironmentProvider gameEnvironmentProvider,
            ContextualEngine sut)
        {
            var loadOrder = new LoadOrder<IModListingGetter<IModGetter>>();
            loadOrder.Add(new ModListing<IModGetter>(modA));
            loadOrder.Add(new ModListing<IModGetter>(modB));

            gameEnv.LoadOrder.Returns(loadOrder);
            sut.EnvGetter.Construct().ReturnsForAnyArgs(gameEnv);
            sut.ContextualModDrivers.Drivers.Returns(drivers);

            sut.Run(reportDropbox);

            foreach (var driver in drivers)
            {
                driver.Received(1).Drive(Arg.Is<ContextualDriverParams>(
                    x => x.ReportDropbox == reportDropbox
                         && x.LoadOrder == loadOrder));
            }
        }
    }
}
