using UnityEngine;
using TMPro;

public class ARWordToggle : MonoBehaviour
{
    [Header("Bahasa Indonesia")]
    public string indoWord = "Meja";
    public AudioClip indoAudio;

    [Header("Bahasa Inggris")]
    public string englishWord = "Table";
    public AudioClip englishAudio;

    [Header("Referensi")]
    public TextMeshProUGUI textDisplay;
    public Transform targetObject; // objek 3D yang ingin di-zoom dan rotate

    [Header("Zoom & Rotate Settings")]
    public float zoomSpeed = 0.5f;
    public float minZoom = 0.3f;
    public float maxZoom = 2.0f;
    public float rotationSpeed = 100f;
    public bool enableSmooth = true;
    public float smoothSpeed = 10f;

    private bool showingEnglish = false;
    private Vector3 initialScale;
    private Quaternion initialRotation;
    private Vector3 targetScale;
    private Quaternion targetRotation;

    void Start()
    {
        if (textDisplay)
            textDisplay.text = indoWord;

        if (targetObject)
        {
            initialScale = targetObject.localScale;
            targetScale = initialScale;
            initialRotation = targetObject.rotation;
            targetRotation = initialRotation;
        }
    }

    void Update()
    {
        HandleZoom();
        HandleRotate();

        if (enableSmooth && targetObject)
        {
            // Smooth transition
            targetObject.localScale = Vector3.Lerp(targetObject.localScale, targetScale, Time.deltaTime * smoothSpeed);
            targetObject.rotation = Quaternion.Lerp(targetObject.rotation, targetRotation, Time.deltaTime * smoothSpeed);
        }
    }

    // ========================
    // 🔠 Toggle Bahasa
    // ========================
    public void ToggleLanguage()
    {
        showingEnglish = !showingEnglish;
        if (textDisplay)
            textDisplay.text = showingEnglish ? englishWord : indoWord;
    }

    // ========================
    // 🔊 Play Audio
    // ========================
    public void PlayAudio()
    {
        AudioClip clipToPlay = showingEnglish ? englishAudio : indoAudio;
        if (clipToPlay != null)
            AudioSource.PlayClipAtPoint(clipToPlay, Camera.main.transform.position);
    }

    // ========================
    // 🔍 Zoom In / Out
    // ========================
    private void HandleZoom()
    {
        if (!targetObject) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Untuk mobile (pinch gesture)
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 t0Prev = t0.position - t0.deltaPosition;
            Vector2 t1Prev = t1.position - t1.deltaPosition;

            float prevDist = (t0Prev - t1Prev).magnitude;
            float currDist = (t0.position - t1.position).magnitude;
            scroll = (currDist - prevDist) * 0.001f;
        }

        if (Mathf.Abs(scroll) > 0.0001f)
        {
            float scaleFactor = 1 + scroll * zoomSpeed;
            targetScale = targetObject.localScale * scaleFactor;
            float scaleClamp = Mathf.Clamp(targetScale.magnitude, initialScale.magnitude * minZoom, initialScale.magnitude * maxZoom);
            targetScale = targetScale.normalized * scaleClamp;
        }
    }

    // ========================
    // 🔄 Rotate Objek
    // ========================
    private void HandleRotate()
    {
        if (!targetObject) return;

        // Mouse drag
        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float rotY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            targetRotation *= Quaternion.Euler(rotY, -rotX, 0);
        }

        // Mobile drag
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                float rotX = t.deltaPosition.x * rotationSpeed * 0.01f * Time.deltaTime;
                float rotY = -t.deltaPosition.y * rotationSpeed * 0.01f * Time.deltaTime;
                targetRotation *= Quaternion.Euler(rotY, -rotX, 0);
            }
        }
    }

    // ========================
    // 🔁 Reset posisi & skala
    // ========================
    public void ResetTransform()
    {
        if (!targetObject) return;
        targetScale = initialScale;
        targetRotation = initialRotation;
    }
}
