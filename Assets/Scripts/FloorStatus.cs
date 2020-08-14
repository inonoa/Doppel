using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorStatus
{
    public readonly GeneratedMap map;
    public readonly HeroMover hero;
    public readonly List<DoppelMover> doppels;

    public FloorStatus(GeneratedMap map, HeroMover hero, List<DoppelMover> doppels)
    {
        this.map = map;
        this.hero = hero;
        this.doppels = doppels;
    }
}
