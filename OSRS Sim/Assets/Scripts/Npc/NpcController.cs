using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public static NpcController Instance { get; private set; }

    private List<Npc> npcs = new();

    private void Awake()
    {
        if (NpcController.Instance == null)
        {
            NpcController.Instance = this;
        }
    }

    public void RegisterNpc(Npc npc)
    {
        npcs.Add(npc);
    }

    public void OnGameTick()
    {
        foreach (Npc npc in npcs)
        {
            npc.OnGameTick();
        }
    }
}
