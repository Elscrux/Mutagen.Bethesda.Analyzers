﻿using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim;

public static class RaceExtensions
{
    public static bool IsChildRace(this IRaceGetter race)
    {
        return (race.Flags & Race.Flag.Child) != 0;
    }
}
