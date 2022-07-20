using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTileIndicator : MonoBehaviour
{
    [SerializeField] private GameObject tileMarkerModel;
    private GameObject tileMarker;
    private IMovement movement;


    private void Awake()
    {
        movement = GetComponent<IMovement>();
    }

    private void Start()
    {
        tileMarker = Instantiate(tileMarkerModel, transform);
        tileMarker.transform.position = Utils.GetWorldLocation(movement.GetTargetTile());
    }

    private void LateUpdate()
    {
        tileMarker.transform.position = Utils.GetWorldLocation(movement.GetTargetTile());
        tileMarker.transform.rotation = Quaternion.identity;
    }
}
