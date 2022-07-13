using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : Enemy
{
    public override void OnGameTick()
    {
        Debug.Log("man attacks");
    }

    private void Start()
    {
        Debug.Log("start man");
    }
}
