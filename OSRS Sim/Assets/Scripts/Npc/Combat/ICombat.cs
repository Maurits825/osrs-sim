using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombat : IGameTick
{
    public void Attack();

    //TODO have to figure out if amount is raw damage, or with reduction and stuff, also when to check dmg type?
    public void ReceiveDamage(int amount);

    public void SetNpcTarget(Npc npc);
    public Npc GetNpcTarget();
}
