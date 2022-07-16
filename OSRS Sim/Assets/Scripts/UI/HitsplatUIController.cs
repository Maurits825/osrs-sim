using System.Collections.Generic;
using UnityEngine;

public class HitsplatUIController : MonoBehaviour
{
    public Camera cam;

    [SerializeField] private ObjectPooler hitsplatPool;
    [SerializeField] private GameStates state;

    private Dictionary<Transform, int> activeHitsplats = new();
    private const int maxHitsplats = 4;

    private void Start()
    {
        hitsplatPool.CreatePool();

        EventController.Instance.OnSpawnHitsplat += SpawnHitsplat;
    }

    private void SpawnHitsplat(int damageAmount, Transform position)
    {
        activeHitsplats.TryGetValue(position, out int count);
        int newCount = count + 1;
        activeHitsplats[position] = newCount;

        if (newCount <= maxHitsplats)
        {
            GameObject hitsplat = hitsplatPool.GetPooledObject();
            hitsplat.transform.SetParent(transform);

            hitsplat.GetComponent<HitsplatUI>().Setup(damageAmount, position, cam, state.tickLength, GetOffset(newCount), RemoveHitSplat);
        } 
    }

    private Vector2 GetOffset(int count)
    {
        if (count == 1)
        {
            return Vector2.down;
        }
        else if (count == 2)
        {
            return Vector2.up;
        }
        else if (count == 3)
        {
            return Vector2.left;
        }
        else if (count == 4)
        {
            return Vector2.right;
        }

        return Vector2.zero;
    }

    private void RemoveHitSplat(Transform position)
    {
        activeHitsplats[position] -= 1;
        if (activeHitsplats[position] == 0)
        {
            activeHitsplats.Remove(position);
        }
    }
}
