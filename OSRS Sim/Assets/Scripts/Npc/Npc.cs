using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO does it need to be abstract? for simple npc like man, cow prob not right?
public abstract class Npc : MonoBehaviour
{
    //TODO kind of scuffed with having to instatiate... is it worth?
    public NpcStates defaultNpcStates;
    [HideInInspector] public NpcStates npcStates;
    public NpcInfo defaultNpcInfo;
    [HideInInspector] public NpcInfo npcInfo;

    public Vector3Int spawnTile;
    public Vector3Int currentTile;

    protected IMovement movement;
    protected ICombat combat;

    private void Awake()
    {
        npcInfo = Instantiate(defaultNpcInfo);
        npcStates = Instantiate(defaultNpcStates);

        spawnTile = Utils.GetTileLocation(transform.position);
        currentTile = spawnTile;

        movement = GetComponent<IMovement>();
        combat = GetComponent<ICombat>();
    }

    protected virtual void Start()
    {
        Debug.Log("Register: " + npcInfo.npcName.ToString());
        NpcController.Instance.RegisterNpc(this);        
    }

    public void OnGameTick()
    {
        //regen stats, hp here?

        //these can affect the nextState
        movement.OnGameTick();
        combat.OnGameTick(); //can override the state set by movement, intended...

        //then update the current state?
        npcStates.currentState = npcStates.nextState;

        switch (npcStates.currentState)
        {
            case NpcStates.States.Idle:
                break;

            case NpcStates.States.Moving:
                movement.Move(); //do we care about target here or where?
                break;

            case NpcStates.States.MovingToNpc:
                movement.Move(); //TODO test this?
                break;

            case NpcStates.States.AttackingNpc:
                combat.Attack();
                break;

            default:
                break;
        }
    }

    public bool IsInRange(Vector3Int target)
    {
        int x = target.x - currentTile.x;
        int y = target.z - currentTile.z;
        int range = Mathf.Max(x, y);

        //TODO handle diff enemysizes, walking under? also do some chekc here for melee i think?, cant be diagonal?
        return range <= npcInfo.attackRange;
    }
}
