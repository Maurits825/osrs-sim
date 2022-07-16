using TMPro;
using UnityEngine;

public class HitsplatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private Transform obj;
    private Camera cam;
    private Vector3 offset;
    private float durationTimer;

    private System.Action<Transform> removeHitsplat;

    public void Setup(int damageAmount, Transform obj, Camera cam, float tickLength, Vector2 offset, System.Action<Transform> removeHitsplat)
    {
        this.cam = cam;
        this.obj = obj;
        this.offset = new Vector3(offset.x, offset.y, 0);
        this.removeHitsplat = removeHitsplat;

        durationTimer = tickLength * 2.5f;

        text.SetText(damageAmount.ToString());

        transform.position = cam.WorldToScreenPoint(obj.transform.position + this.offset);

        gameObject.SetActive(true);
    }

    private void Update()
    {
        durationTimer -= Time.deltaTime;
        if (durationTimer <= 0)
        {
            gameObject.SetActive(false);
            removeHitsplat(obj);
        }

        transform.position = cam.WorldToScreenPoint(obj.transform.position + offset);
    }
}
