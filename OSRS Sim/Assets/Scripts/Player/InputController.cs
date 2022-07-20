using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour, IGameTick
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask enemyMask;

    private IMovement movement;
    private ICombat combat;
    private Npc npc;

    private Npc npcClicked;
    private ICombat combatNpcClicked;
    private Vector2Int tileClicked;
    private NpcStates.States nextState;

    private bool isInput = false;

    public void OnGameTick()
    {
        if (isInput)
        {
            switch (nextState)
            {
                case NpcStates.States.Moving:
                    //TODO if click unwalkable, still moving for one tick
                    if (tileClicked == npc.currentTile)
                    {
                        npc.npcStates.nextState = NpcStates.States.Idle;
                    }
                    else
                    {
                        movement.SetTargetTile(tileClicked);
                        npc.npcStates.nextState = nextState;
                    }
                    break;

                case NpcStates.States.MovingToNpc:
                    combat.SetNpcTarget(npcClicked);
                    movement.SetTargetNpc(npcClicked);
                    //combatNpcClicked.SetNpcTarget(npc); //TODO set ourselves as target, is this the place to do it?
                    npc.npcStates.nextState = nextState;
                    break;
            }

            isInput = false;
        }
    }

    private void Start()
    {
        movement = GetComponent<IMovement>();
        combat = GetComponent<ICombat>();
        npc = GetComponent<Npc>();
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        //TODO refactor to maybe use one raycast? inverse layermask with unwalkable?
        if (Physics.Raycast(ray, out raycastHit, float.MaxValue, enemyMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
                npcClicked = raycastHit.collider.transform.root.GetComponent<Npc>();
                combatNpcClicked = raycastHit.collider.transform.root.GetComponent<ICombat>();

                nextState = NpcStates.States.MovingToNpc;
                isInput = true;
            }
        }
        else if (Physics.Raycast(ray, out raycastHit, float.MaxValue, groundMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
                tileClicked = Utils.GetTileLocation(raycastHit.point);

                nextState = NpcStates.States.Moving;
                isInput = true;
            }
        }
    }
}
