using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private float maxCameraSpeed = 2.5f;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = player.transform.position + new Vector3(0, 5, -5);
        transform.position = Vector3.Slerp(transform.position, newPos, maxCameraSpeed * Time.deltaTime);
    }
}
