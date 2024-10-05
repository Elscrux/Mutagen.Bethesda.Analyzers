using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Handlers;

public interface IReportHandler
{
    void Dropoff(
        ReportContextParameters parameters,
        ModKey sourceMod,
        IMajorRecordIdentifier majorRecord,
        ITopic topic);

    void Dropoff(
        ReportContextParameters parameters,
        ITopic topic);
}
