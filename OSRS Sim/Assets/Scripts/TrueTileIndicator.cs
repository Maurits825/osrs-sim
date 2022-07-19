using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueTileIndicator : MonoBehaviour
{
    [SerializeField] private GameObject tileMarkerModel;
    private GameObject tileMarker;
    private Npc npc;


    private void Awake()
    {
        npc = GetComponent<Npc>();
    }

    private void Start()
    {
        tileMarker = Instantiate(tileMarkerModel, transform);
        tileMarker.transform.position = Utils.GetWorldLocation(npc.currentTile);
    }

    private void LateUpdate()
    {
        tileMarker.transform.position = Utils.GetWorldLocation(npc.currentTile);
        tileMarker.transform.rotation = Quaternion.identity;
    }
}
