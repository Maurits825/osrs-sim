using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCombat : MonoBehaviour, ICombat
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
        //TODO is it correct to switch through states here, similar to movement
        switch (npc.npcStates.currentState)
        {
            case NpcStates.States.Idle:
            case NpcStates.States.Moving:
                if (npc.npcInfo.isAggresive)
                {
                    //TODO search for target and stuff when both idle or moving?
                }
                break;

            case NpcStates.States.MovingToNpc:
                break;
            case NpcStates.States.AttackingNpc:
                break;
            default:
                break;
        }
    }

    public void ReceiveDamage(int amount)
    {
        npc.npcInfo.npcStats.health.current -= amount;

        if (npc.npcStates.currentState == NpcStates.States.Idle || npc.npcStates.currentState == NpcStates.States.Moving)
        {
            //TODO assume that target has been set
            npc.npcStates.nextState = NpcStates.States.MovingToNpc;
        }
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
