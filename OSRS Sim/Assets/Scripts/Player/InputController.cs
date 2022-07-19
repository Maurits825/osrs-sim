using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask enemyMask;

    // TODO refactor to movement maybe? or maybe thing about how to handle "plugins"
    [SerializeField] private GameObject tileMarker;

    private IMovement movement;
    private ICombat combat;
    private Npc npc;

    private void Start()
    {
        movement = GetComponent<IMovement>();
        combat = GetComponent<ICombat>();
        npc = GetComponent<Npc>();
    }

    private void Update() //TODO buffer the next state transition in a var?
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        //TODO refactor to maybe use one raycast? inverse layermask with unwalkable?
        if (Physics.Raycast(ray, out raycastHit, float.MaxValue, enemyMask))
        {
            Vector2Int tileLocation = Utils.GetTileLocation(raycastHit.point);
            tileMarker.transform.position = Utils.GetWorldLocation(tileLocation);

            if (Input.GetMouseButtonDown(0))
            {
                Npc npcClicked = raycastHit.collider.transform.root.GetComponent<Npc>();
                ICombat combatNpcClicked = raycastHit.collider.transform.root.GetComponent<ICombat>();

                combat.SetNpcTarget(npcClicked);
                movement.SetTargetTile(npcClicked.currentTile);
                combatNpcClicked.SetNpcTarget(npc); //TODO set ourselves as target, is this the place to do it?

                npc.npcStates.nextState = NpcStates.States.MovingToNpc;
            }
        }
        else if (Physics.Raycast(ray, out raycastHit, float.MaxValue, groundMask))
        {
            Vector2Int tileLocation = Utils.GetTileLocation(raycastHit.point);
            tileMarker.transform.position = Utils.GetWorldLocation(tileLocation); //TODO repeated

            if (Input.GetMouseButtonDown(0))
            {
                movement.SetTargetTile(tileLocation);
                npc.npcStates.nextState = NpcStates.States.Moving;
            }
        }
    }
}
