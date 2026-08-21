using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Placed.Object;

public sealed class PlayerActivatorAnalyzer: IContextualRecordAnalyzer<IPlacedObjectGetter>
{
    public static readonly TopicDefinition InvalidRotation = MutagenTopicBuilder.FromDiscussion(
            652,
            "Player Activator with Invalid Rotation",
            Severity.Error)
        .WithoutFormatting("Player activator is a box without a rotation of 0 in all axes and will not be visible in the world");

    public static readonly TopicDefinition MissingName = MutagenTopicBuilder.FromDiscussion(
            653,
            "Player Activator with Missing Name",
            Severity.Error)
        .WithoutFormatting("Player activator has no name set and will not be visible in the world");

    public IEnumerable<TopicDefinition> Topics { get; } = [InvalidRotation];

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<IPlacedObjectGetter> param)
    {
        var placedObject = param.Record;

        if (!placedObject.CollisionLayer.HasValue) return;
        if (placedObject.CollisionLayer.Value != 15) return; // 15 is L_NONCOLLIDABLE, the player activator collision layer
        if (placedObject.Primitive is null) return;
        if (placedObject.Primitive.Type == PlacedPrimitive.TypeEnum.None) return;

        if (!placedObject.Base.TryResolve<IActivatorGetter>(param.LinkCache, out var activator)) return;

        if (string.IsNullOrEmpty(activator.Name?.String))
        {
            param.AddTopic(
                MissingName.Format());
        }

        if (placedObject.Primitive.Type == PlacedPrimitive.TypeEnum.Box)
        {
            if (placedObject.Placement is { Rotation: { X: 0, Y: 0, Z: 0 } })
            {
                param.AddTopic(
                    InvalidRotation.Format());
            }
        }
    }

    public IEnumerable<Func<IPlacedObjectGetter, object?>> FieldsOfInterest()
    {
        yield return x => x.Base;
        yield return x => x.CollisionLayer;
        yield return x => x.Primitive;
    }
}
