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
    private Npc npc;

    private void Start()
    {
        movement = GetComponent<IMovement>();
        npc = GetComponent<Npc>();
    }

    private void Update() //TODO buffer the next state transition in a var?
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        //TODO refactor to maybe use one raycast? inverse layermask with unwalkable?
        if (Physics.Raycast(ray, out raycastHit, float.MaxValue, enemyMask))
        {
            //TODO get tile location from enemy
            Vector3Int tileLocation = Utils.GetTileLocation(raycastHit.point);
            tileMarker.transform.position = tileLocation;

            if (Input.GetMouseButtonDown(0))
            {
                //TODO enemy target with cmb controller?
                //enemyTileClicked = tileLocation;
                //enemyClicked = raycastHit.collider.transform.root.GetComponent<Enemy>();
                npc.npcStates.nextState = NpcStates.States.MovingToNpc;
            }
        }
        else if (Physics.Raycast(ray, out raycastHit, float.MaxValue, groundMask))
        {
            Vector3Int tileLocation = Utils.GetTileLocation(raycastHit.point);
            tileMarker.transform.position = tileLocation;

            if (Input.GetMouseButtonDown(0))
            {
                movement.SetTargetTile(tileLocation);
                npc.npcStates.nextState = NpcStates.States.Moving;
            }
        }
    }
}
