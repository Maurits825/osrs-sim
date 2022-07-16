using System;
using UnityEngine;

[Serializable]
public class NpcStatsValue
{
    public int current;
    public int initial;

    public NpcStatsValue()
    {
        current = initial;
    }
}

