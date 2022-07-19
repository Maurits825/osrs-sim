using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, ICombat
{
    private Npc npc;
    private Npc targetNpc;
    private ICombat targetCombat;

    private void Start()
    {
        npc = GetComponent<Npc>();
    }

    public void Attack()
    {
        int damage = npc.npcInfo.maxHit;
        targetCombat.ReceiveDamage(damage);
    }

    public void OnGameTick()
    {
        
    }

    public void ReceiveDamage(int amount)
    {
        npc.npcInfo.npcStats.health.current -= amount;
        //TODO auto retaliate here...
    }

    public void SetNpcTarget(Npc npc)
    {
        targetNpc = npc;
        targetCombat = npc.GetComponent<ICombat>();
    }
}
