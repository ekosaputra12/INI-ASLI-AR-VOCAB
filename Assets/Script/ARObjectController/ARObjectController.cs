using UnityEngine;

public class ARRotateZoom : MonoBehaviour
{
    public float rotateSpeed = 0.2f;
    public float zoomSpeed = 0.01f;
    public float minScale = 0.3f;
    public float maxScale = 2f;

    private bool hasSentRotateUI = false;   // Untuk memastikan UI cuma terpanggil sekali

    void Update()
    {
        // ---------------- ROTATE with Touch (1 Finger) ----------------
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                float rotX = t.deltaPosition.x * rotateSpeed;
                float rotY = t.deltaPosition.y * rotateSpeed;

                transform.Rotate(Vector3.up, -rotX, Space.World);
                transform.Rotate(Vector3.right, rotY, Space.Self);

                TriggerRotateUIEvent();
            }
        }

        // ---------------- ZOOM with Touch (Pinch) ----------------
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float prevDist = ((t0.position - t0.deltaPosition) -
                              (t1.position - t1.deltaPosition)).magnitude;
            float curDist = (t0.position - t1.position).magnitude;
            float delta = curDist - prevDist;

            float scaleVal = Mathf.Clamp(transform.localScale.x + delta * zoomSpeed,
                                            minScale, maxScale);
            transform.localScale = Vector3.one * scaleVal;
        }

        // ---------------- ROTATE with Mouse (Right Click + Drag) ----------------
        if (Input.GetMouseButton(1))
        {
            float rotX = Input.GetAxis("Mouse X") * rotateSpeed * 10f;
            float rotY = Input.GetAxis("Mouse Y") * rotateSpeed * 10f;

            transform.Rotate(Vector3.up, -rotX, Space.World);
            transform.Rotate(Vector3.right, rotY, Space.Self);

            TriggerRotateUIEvent();
        }

        // ---------------- ZOOM with Mouse Scroll Wheel ----------------
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float scaleVal = Mathf.Clamp(transform.localScale.x + scroll * zoomSpeed * 50f,
                                                minScale, maxScale);
            transform.localScale = Vector3.one * scaleVal;
        }
    }

    // ---------------- Trigger UI event once ----------------
    void TriggerRotateUIEvent()
    {
        if (hasSentRotateUI) return;
        hasSentRotateUI = true;

        var ui = FindObjectOfType<MascotUIManager_Fade>();
        if (ui != null)
        {
            ui.OnObjectRotated();
        }
    }
}
