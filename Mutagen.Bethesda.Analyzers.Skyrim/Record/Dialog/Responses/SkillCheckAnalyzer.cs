﻿using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Responses;

public class SkillCheckAnalyzer : IIsolatedRecordAnalyzer<IDialogResponsesGetter>
{
    public static readonly TopicDefinition<float> NonPlayerSkillCheck = MutagenTopicBuilder.DevelopmentTopic(
            "Non-Player Skill Check",
            Severity.Warning)
        .WithFormatting<float>("Skill check in dialog are not checked on the player but on {0} - this is usually a sign of a mistake");

    public static readonly TopicDefinition<Condition.RunOnType> NonGlobalSkillCheck = MutagenTopicBuilder.DevelopmentTopic(
            "Non-Global Skill Check",
            Severity.Suggestion)
        .WithFormatting<Condition.RunOnType>("Skill check in dialog doesn't use global to evaluate skill level but {0} - this is usually a sign of a mistake");

    public IEnumerable<TopicDefinition> Topics { get; } = [NonPlayerSkillCheck, NonGlobalSkillCheck];

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IDialogResponsesGetter> param)
    {
        var dialogResponses = param.Record;

        foreach (var condition in dialogResponses.Conditions)
        {
            if (condition.Data is not IGetActorValueConditionDataGetter getActorValue) continue;
            if (!getActorValue.ActorValue.IsSkill()) continue;

            // Non-Player Skill Check
            if (getActorValue.RunOnType != Condition.RunOnType.Target && !getActorValue.RunsOnPlayer())
            {
                param.AddTopic(
                    NonGlobalSkillCheck.Format(condition.Data.RunOnType));
            }

            // Non-Global Skill Check
            if (condition is IConditionFloatGetter conditionFloatGetter && condition.Data.RunsOnPlayer())
            {
                param.AddTopic(
                    NonPlayerSkillCheck.Format(conditionFloatGetter.ComparisonValue));
            }
        }
    }

    public IEnumerable<Func<IDialogResponsesGetter, object?>> FieldsOfInterest()
    {
        yield return x => x.Conditions;
    }
}
