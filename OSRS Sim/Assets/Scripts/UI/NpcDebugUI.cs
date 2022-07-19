using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcDebugUI : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera cam;

    private TextMeshProUGUI debugText;
    private GameObject debugObj;
    private Npc npc;

    private float offset;
    private void Awake()
    {
        npc = GetComponent<Npc>();
        offset = GetComponent<MeshRenderer>().bounds.size.y / 2;
    }

    private void Start()
    {
        debugObj = Instantiate(prefab, canvas.transform);
        debugText = debugObj.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        debugObj.transform.position = cam.WorldToScreenPoint(transform.position + (offset * Vector3.up));
        debugText.SetText(npc.npcStates.currentState.ToString());
    }
}
