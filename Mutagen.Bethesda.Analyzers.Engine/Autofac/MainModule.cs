﻿using Autofac;
using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.Engines;
using Mutagen.Bethesda.Autofac;
using Noggog.Autofac.Modules;

namespace Mutagen.Bethesda.Analyzers.Autofac
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<NoggogModule>();
            builder.RegisterModule<MutagenModule>();
            builder.RegisterModule<ReflectionDriverModule>();
            builder.RegisterAssemblyTypes(typeof(IsolatedEngine).Assembly)
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();
            builder.RegisterGeneric(typeof(InjectionDriverProvider<>))
                .As(typeof(IDriverProvider<>))
                .SingleInstance();
        }
    }
}
