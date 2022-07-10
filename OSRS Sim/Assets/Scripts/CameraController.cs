using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Vector3 playerOffset = Vector3.up * 1f;

    private float rotateSpeed = 0.2f;
    private Vector2 previousMousePos;
    private Vector2 currentMousePos;

    private float zoom = -5f;
    private float zoomSpeed = 0.9f;
    private float minZoom = -100f;
    private float maxZoom = -2f;

    private void Start()
    {
        transform.LookAt(player.transform.position + playerOffset);
    }
    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        if (Input.GetMouseButtonDown(2))
        {
            previousMousePos = new Vector2(mousePos.x, mousePos.y);
        }
        
        if (Input.GetMouseButton(2))
        { 
            currentMousePos = new Vector2(mousePos.x, mousePos.y);

            Vector2 mousePosDiff = currentMousePos - previousMousePos;
            transform.RotateAround(player.transform.position + playerOffset, Vector3.up, mousePosDiff.x * rotateSpeed);
            transform.RotateAround(player.transform.position + playerOffset, -transform.right, mousePosDiff.y * rotateSpeed);
            previousMousePos = currentMousePos;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            zoom += Input.mouseScrollDelta.y * zoomSpeed;
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        }

        transform.position = player.transform.position + playerOffset + (zoom * transform.forward);
    }
}
