using UnityEngine;

public class ARRotateZoom : MonoBehaviour
{
    public float rotateSpeed = 5f;
    public float zoomSpeed = 0.01f;
    public float minScale = 0.3f;
    public float maxScale = 2f;

    void Update()
    {
        // ROTATE (1 jari)
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                float rotX = t.deltaPosition.x * rotateSpeed;
                transform.Rotate(0, -rotX, 0, Space.World);
            }
        }

        // ZOOM (2 jari)
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float prevDist = (t0.position - t0.deltaPosition - (t1.position - t1.deltaPosition)).magnitude;
            float curDist = (t0.position - t1.position).magnitude;
            float delta = curDist - prevDist;

            Vector3 scale = transform.localScale + Vector3.one * (delta * zoomSpeed);
            scale = Vector3.Max(scale, Vector3.one * minScale);
            scale = Vector3.Min(scale, Vector3.one * maxScale);
            transform.localScale = scale;
        }
    }
}
