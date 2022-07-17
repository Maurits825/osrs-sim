using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Npc
{
    //override to prevent registering to npccontroller...
    protected override void Start()
    {
        movement = GetComponent<IMovement>();
    }
}
