using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject obj;
    private Image barImage;

    private float offset;

    public virtual void Start()
    {
        offset = obj.GetComponent<MeshRenderer>().bounds.size.y;
        barImage = transform.Find("health").GetComponent<Image>();
    }

    public virtual void Update()
    {
        transform.position = cam.WorldToScreenPoint(obj.transform.position + (offset * Vector3.up));
    }

    protected void SetHealth(float amount)
    {
        barImage.fillAmount = amount;
    }
}
