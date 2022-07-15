using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private GameObject obj;
    private Image barImage;

    private float offset;

    private void Start()
    {
        offset = obj.GetComponent<MeshRenderer>().bounds.size.y;
        barImage = transform.Find("health").GetComponent<Image>();
    }

    private void LateUpdate()
    {
        transform.position = obj.transform.position + (offset * Vector3.up);
        transform.LookAt(transform.position + cam.forward);
    }

    protected void SetHealth(float amount)
    {
        barImage.fillAmount = amount;
    }
}
