using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : Enemy
{
    public override void OnGameTick()
    {
        Debug.Log("man attacks");
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("start man");
    }
}
