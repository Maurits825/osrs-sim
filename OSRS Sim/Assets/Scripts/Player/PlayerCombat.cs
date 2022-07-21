using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, ICombat
{
    private Npc npc;
    private Npc npcTarget;
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
        if (npc.npcStates.currentState == NpcStates.States.MovingToNpc && npcTarget.IsInRange(npc.currentTile, npc.npcInfo.attackRange))
        {
            npc.npcStates.currentState = NpcStates.States.AttackingNpc;
        }

        if (npc.npcStates.currentState == NpcStates.States.AttackingNpc)
        {

        }

    }

    public void ReceiveDamage(int amount)
    {
        npc.npcInfo.npcStats.health.current -= amount;
        //TODO auto retaliate here...
    }

    public void SetNpcTarget(Npc npc)
    {
        npcTarget = npc;
        targetCombat = npc.GetComponent<ICombat>();
    }

    public Npc GetNpcTarget()
    {
        return npcTarget;
    }
}
