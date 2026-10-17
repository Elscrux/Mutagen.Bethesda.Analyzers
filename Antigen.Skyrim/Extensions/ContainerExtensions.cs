using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace Antigen.Skyrim.Extensions;

public static class ContainerExtensions
{
    extension(IContainerGetter container)
    {
        public IEnumerable<IItemGetter> SampleItems(ILinkCache linkCache, int level, bool useChanceNone, Random random)
        {
            if (container.Items is null) return [];

            return container.Items
                .SelectMany(entry =>
                {
                    return entry.Item.Item.TryResolve(linkCache) switch
                    {
                        ILeveledItemGetter leveledItem => leveledItem.SampleItems(linkCache, level, useChanceNone, random),
                        {} item => Enumerable.Repeat(item, entry.Item.Count),
                        _ => []
                    };
                })
                .WhereNotNull();
        }
    }
}
