﻿using Mutagen.Bethesda.Analyzers.Config;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Drops;

public class SeverityAdjuster : IReportDropbox
{
    private readonly IReportDropbox _dropbox;
    private readonly ISeverityLookup _severityLookup;

    public SeverityAdjuster(
        IReportDropbox dropbox,
        ISeverityLookup severityLookup)
    {
        _dropbox = dropbox;
        _severityLookup = severityLookup;
    }

    public void Dropoff(ReportContextParameters parameters, IModGetter sourceMod, IMajorRecordGetter majorRecord, ITopic topic)
    {
        _dropbox.Dropoff(parameters, sourceMod, majorRecord,
            AdjustSeverity(topic));
    }

    public void Dropoff(ReportContextParameters parameters, ITopic topic)
    {
        _dropbox.Dropoff(parameters,
            AdjustSeverity(topic));
    }

    private ITopic AdjustSeverity(ITopic topic)
    {
        var sev = _severityLookup.LookupSeverity(topic.TopicDefinition);
        if (sev == topic.Severity) return topic;
        return topic.WithSeverity(sev);
    }
}
