using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Transform objectTransform;
    public float Rotate = 100f;

    private Vector3 previousMousePosition;

    private Quaternion defaultRotation;
    private Vector3 defaultPosition;

    void Start()
    {
        if (objectTransform != null)
        {
            defaultRotation = objectTransform.rotation;
            defaultPosition = objectTransform.position;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //Reset();
            Debug.Log("Jawa");
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 deltaMousePosition = Input.mousePosition - previousMousePosition;
            float rotationX = deltaMousePosition.y * Rotate * Time.deltaTime;
            float rotationY = -deltaMousePosition.x * Rotate * Time.deltaTime;

            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            objectTransform.rotation = rotation * objectTransform.rotation;

            previousMousePosition = Input.mousePosition;
        }
    }

    void Reset()
    {
        objectTransform.position = defaultPosition;
        objectTransform.rotation = defaultRotation;
    }
}
