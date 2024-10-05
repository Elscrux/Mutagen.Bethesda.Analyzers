﻿using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

public interface IContextualRecordFrameAnalyzer<TMajor> : IAnalyzer
    where TMajor : IMajorRecordGetter
{
    void AnalyzeRecord(ContextualRecordFrameAnalyzerParams<TMajor> param);
}

public interface IContextualRecordFrameAnalyzer : IAnalyzer
{
    bool AcceptsType<TMajor>() where TMajor : IMajorRecordGetter;

    void AnalyzeRecord(ContextualRecordFrameAnalyzerParams param);
}
