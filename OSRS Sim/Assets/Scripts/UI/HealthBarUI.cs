using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera cam;
    
    private Image barImage;
    private GameObject healthBar;
    
    private Npc npc;

    private float offset;

    private void Start()
    {
        offset = GetComponent<MeshRenderer>().bounds.size.y;
        npc = GetComponent<Npc>();

        healthBar = Instantiate(healthBarPrefab, canvas.transform);
        barImage = healthBar.transform.Find("health").GetComponent<Image>();
    }

    private void Update()
    {
        healthBar.transform.position = cam.WorldToScreenPoint(transform.position + (offset * Vector3.up));
        SetHealth((float)npc.npcInfo.npcStats.health.current / npc.npcInfo.npcStats.health.initial);
    }

    private void SetHealth(float amount)
    {
        barImage.fillAmount = amount;
    }
}
