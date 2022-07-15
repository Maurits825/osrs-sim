using UnityEngine;

public class HitsplatUIController : MonoBehaviour
{
    public Camera cam;

    [SerializeField] private ObjectPooler hitsplatPool;

    private void Start()
    {
        hitsplatPool.CreatePool();

        EventController.Instance.OnSpawnHitsplat += SpawnHitsplat;
    }

    private void SpawnHitsplat(int damageAmount, Transform position)
    {
        GameObject hitsplat = hitsplatPool.GetPooledObject();
        hitsplat.transform.SetParent(transform);
        hitsplat.GetComponent<HitsplatUI>().Setup(damageAmount, position, cam);
    }
}
