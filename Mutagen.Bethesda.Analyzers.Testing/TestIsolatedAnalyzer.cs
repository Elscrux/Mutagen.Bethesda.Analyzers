using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Testing;

public class TestIsolatedRecordAnalyzer : IIsolatedRecordAnalyzer<INpcGetter>
{
    public static readonly TopicDefinition HasHeight = MutagenTopicBuilder.DevelopmentTopic(
            "Has Height",
            Severity.Warning)
        .WithoutFormatting("Test analyzer is angry the NPC has a height");

    public IEnumerable<TopicDefinition> Topics => [HasHeight];

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<INpcGetter> param)
    {
        if (param.Record.Height > 0)
        {
            param.AddTopic(HasHeight.Format());
        }
    }

    public IEnumerable<Func<INpcGetter, object?>> FieldsOfInterest()
    {
        yield return x => x.Height;
    }
}
