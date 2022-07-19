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

    private void Awake()
    {
        npcInfo = Instantiate(defaultNpcInfo);
        npcStates = Instantiate(defaultNpcStates);

        spawnTile = Utils.GetTileLocation(transform.position);
        currentTile = spawnTile;
    }

    protected virtual void Start()
    {
        Debug.Log("Register: " + npcInfo.npcName.ToString());
        NpcController.Instance.RegisterNpc(this);

        //TODO grab attack and movement interface here?
        movement = GetComponent<IMovement>();
    }

    public void OnGameTick()
    {
        //regen stats, hp here?

        //these can affect the nextState
        movement.OnGameTick();
        //do attack stuff here, which can override the state set by movement

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
                break;

            default:
                break;
        }
    }

    private bool IsInRange(Vector3Int target)
    {
        int x = target.x - currentTile.x;
        int y = target.z - currentTile.z;
        int range = Mathf.Max(x, y);

        //TODO handle diff enemysizes, walking under?
        return range <= npcInfo.attackRange;
    }
}
