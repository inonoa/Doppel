using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorStatus
{
    public readonly GeneratedMap map;
    public HeroMover hero;

    public FloorStatus(GeneratedMap map, HeroMover hero)
    {
        this.map = map;
        this.hero = hero;
    }
}
