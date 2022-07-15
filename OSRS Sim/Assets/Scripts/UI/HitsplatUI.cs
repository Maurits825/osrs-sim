using TMPro;
using UnityEngine;

public class HitsplatUI : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Transform obj;
    private Camera cam;

    public void Setup(int damageAmount, Transform obj, Camera cam)
    {
        this.cam = cam;
        this.obj = obj;

        text = transform.GetComponent<TextMeshProUGUI>();
        text.SetText(damageAmount.ToString());
        
        transform.position = cam.WorldToScreenPoint(obj.transform.position);

        gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.position = cam.WorldToScreenPoint(obj.transform.position);
    }
}
