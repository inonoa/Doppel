using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorStatus
{
    public readonly GeneratedMap map;
    public readonly HeroMover hero;
    public readonly List<DoppelMover> doppels;
    public readonly bool[][] seen;

    public FloorStatus(GeneratedMap map, HeroMover hero, List<DoppelMover> doppels)
    {
        this.map = map;
        this.hero = hero;
        this.doppels = doppels;
        seen = new bool[map.Height][];
        foreach(int i in Enumerable.Range(0, seen.Length))
        {
            seen[i] = new bool[map.Width];
        }
    }
}
