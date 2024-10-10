using System.Collections;
using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Drops;

public class DisallowedParametersChecker : IReportDropbox
{
    private readonly IReportDropbox _reportDropbox;

    public DisallowedParametersChecker(
        IReportDropbox reportDropbox)
    {
        _reportDropbox = reportDropbox;
    }

    public void Dropoff(
        ReportContextParameters parameters,
        ModKey mod,
        IMajorRecordIdentifier record,
        Topic topic)
    {
        _reportDropbox.Dropoff(parameters, mod, record, Check(parameters, topic));
    }

    public void Dropoff(
        ReportContextParameters parameters,
        Topic topic)
    {
        _reportDropbox.Dropoff(parameters, Check(parameters, topic));
    }

    private Topic Check(
        ReportContextParameters parameters,
        Topic topic)
    {
        return topic with
        {
            FormattedTopic = topic.FormattedTopic.Transform(parameters, Checker)
        };
    }

    private object? Checker(
        ReportContextParameters parameters,
        object? item)
    {
        switch (item)
        {
            case null:
            case string:
                return item;
            case IEnumerable:
                throw new ArgumentException("Enumerables are not allowed in formatted topic parameters");
            case FormKey:
                throw new ArgumentException("FormKeys are not allowed in formatted topic parameters.  Pass in typed FormLinks instead");
            default:
                return item;
        }
    }
}
