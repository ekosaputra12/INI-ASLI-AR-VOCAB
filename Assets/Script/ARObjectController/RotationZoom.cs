// ARObjectManipulation.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class ARObjectManipulation : MonoBehaviour
{
    [Header("Rotation (1 finger)")]
    public bool rotateAroundYOnly = true;
    public float rotateSpeed = 0.25f;          // besar rotasi per piksel drag (device) / per piksel mouse
    public bool requireTouchOnObject = true;   // mulai rotasi bila tap mengenai collider objek

    [Header("Scaling (2 fingers)")]
    public float scaleSpeed = 0.005f;          // responsivitas pinch
    public float minScale = 0.2f;              // kelipatan dari skala awal
    public float maxScale = 3.0f;

    Camera cam;
    bool isDragging;
    Vector2 lastPos;

    // Untuk pinch
    bool isPinching;
    float startPinchDistance;
    Vector3 startScale;
    float initialScaleMultiplier = 1f; // faktor skala relatif terhadap scale awal saat Start()

    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        // Simpan skala awal supaya batas min/max relatif ke skala ini
        startScale = transform.localScale;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    // ======================= TOUCH (device) =======================
    void HandleTouch()
    {
        if (Input.touchCount == 0)
        {
            isDragging = false;
            isPinching = false;
            return;
        }

        // Pinch (2 jari) => Scale
        if (Input.touchCount >= 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            if (IsTouchOverUI(t0) || IsTouchOverUI(t1)) return;
            if (requireTouchOnObject && !(HitThisObject(t0.position) || HitThisObject(t1.position))) return;

            float dist = Vector2.Distance(t0.position, t1.position);

            if (!isPinching || t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
            {
                isPinching = true;
                startPinchDistance = Mathf.Max(dist, 0.001f);
                startScale = transform.localScale;
            }
            else
            {
                float scaleFactor = 1f + (dist - startPinchDistance) * scaleSpeed;
                Vector3 target = startScale * scaleFactor;

                // Clamp relatif terhadap skala awal (Start)
                float currentFactor = target.x / startScale.x; // asumsi uniform scale
                float clampedFactor = Mathf.Clamp(currentFactor, minScale, maxScale);
                float uniform = startScale.x * clampedFactor;
                transform.localScale = new Vector3(uniform, uniform, uniform);
            }

            // Saat pinch aktif, jangan proses drag 1 jari
            isDragging = false;
            return;
        }

        // Rotasi (1 jari)
        Touch t = Input.GetTouch(0);
        if (IsTouchOverUI(t)) return;

        if (t.phase == TouchPhase.Began)
        {
            if (!requireTouchOnObject || HitThisObject(t.position))
            {
                isDragging = true;
                lastPos = t.position;
            }
        }
        else if (t.phase == TouchPhase.Moved && isDragging)
        {
            Vector2 delta = t.position - lastPos;
            ApplyRotation(delta);
            lastPos = t.position;
        }
        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
        {
            isDragging = false;
        }
    }

    // ======================= MOUSE (Editor) =======================
    void HandleMouse()
    {
        // Scroll untuk scale (nyaman saat testing di Editor)
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            Vector3 cur = transform.localScale;
            float s = cur.x + scroll * (scaleSpeed * 100f); // percepat sedikit di editor
            float baseX = startScale.x;
            float factor = Mathf.Clamp(s / baseX, minScale, maxScale);
            float uniform = baseX * factor;
            transform.localScale = new Vector3(uniform, uniform, uniform);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUIObject()) return;
            if (!requireTouchOnObject || HitThisObject(Input.mousePosition))
            {
                isDragging = true;
                lastPos = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastPos;
            ApplyRotation(delta);
            lastPos = Input.mousePosition;
        }
    }

    // ======================= Helpers =======================
    void ApplyRotation(Vector2 screenDelta)
    {
        float yaw = -screenDelta.x * rotateSpeed;

        if (rotateAroundYOnly)
        {
            transform.Rotate(0f, yaw, 0f, Space.World);
        }
        else
        {
            float pitch = screenDelta.y * rotateSpeed;
            Vector3 e = transform.eulerAngles;
            float newX = ClampAngle(e.x + pitch, -80f, 80f);
            transform.eulerAngles = new Vector3(newX, e.y + yaw, e.z);
        }
    }

    bool HitThisObject(Vector2 screenPos)
    {
        if (cam == null) cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            return hit.transform == transform || hit.transform.IsChildOf(transform);
        }
        return false;
    }

    static float ClampAngle(float angle, float min, float max)
    {
        angle = (angle > 180) ? angle - 360 : angle;
        return Mathf.Clamp(angle, min, max);
    }

    // Cegah gesture saat menyentuh UI
    bool IsTouchOverUI(Touch t)
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(t.fingerId);
    }
    bool IsPointerOverUIObject()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
