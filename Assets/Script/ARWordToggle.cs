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
    public Transform targetObject;

    [Header("Zoom & Rotate Settings")]
    public float zoomSpeed = 0.5f;
    public float minZoom = 0.3f;
    public float maxZoom = 2.0f;
    public float rotationSpeed = 100f;
    public bool enableSmooth = true;
    public float smoothSpeed = 10f;

    private bool showingEnglish = false; // mulai dari Indonesia
    private Vector3 initialScale;
    private Quaternion initialRotation;
    private Vector3 targetScale;
    private Quaternion targetRotation;

    [Header("Tambahan")]
    public MascotUIManager_Fade mascotUIManager;
    private bool hasRotated = false;

    void Start()
    {
        // ⛔ Tidak tampil teks di awal
        if (textDisplay)
            textDisplay.text = "";

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
            targetObject.localScale = Vector3.Lerp(targetObject.localScale, targetScale, Time.deltaTime * smoothSpeed);
            targetObject.rotation = Quaternion.Lerp(targetObject.rotation, targetRotation, Time.deltaTime * smoothSpeed);
        }
    }

    private void HandleZoom()
    {
        if (!targetObject) return;
        float scroll = Input.GetAxis("Mouse ScrollWheel");

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

    private void HandleRotate()
    {
        if (!targetObject) return;
        bool rotated = false;

        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float rotY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            targetRotation *= Quaternion.Euler(rotY, -rotX, 0);
            rotated = Mathf.Abs(rotX) > 0.01f || Mathf.Abs(rotY) > 0.01f;
        }

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                float rotX = t.deltaPosition.x * rotationSpeed * 0.01f * Time.deltaTime;
                float rotY = -t.deltaPosition.y * rotationSpeed * 0.01f * Time.deltaTime;
                targetRotation *= Quaternion.Euler(rotY, -rotX, 0);
                rotated = true;
            }
        }

        if (rotated && !hasRotated)
        {
            hasRotated = true;
            Debug.Log("✅ Objek sudah di-rotate — panggil maskot ke-3");

            if (mascotUIManager != null)
                mascotUIManager.OnObjectRotated();
            else
                Debug.LogWarning("⚠️ mascotUIManager belum di-assign di inspector!");
        }
    }

    // 🔄 Tombol untuk menampilkan & berganti bahasa
    public void ToggleLanguage()
    {
        if (!textDisplay) return;

        // Jika teks belum muncul, mulai dari Indonesia
        if (string.IsNullOrEmpty(textDisplay.text))
        {
            textDisplay.text = indoWord;
            showingEnglish = false;
        }
        else
        {
            // Toggle antara Indonesia ↔ Inggris
            showingEnglish = !showingEnglish;
            textDisplay.text = showingEnglish ? englishWord : indoWord;
        }
    }

    public void PlayAudio()
    {
        AudioClip clipToPlay = showingEnglish ? englishAudio : indoAudio;
        if (clipToPlay != null && Camera.main)
            AudioSource.PlayClipAtPoint(clipToPlay, Camera.main.transform.position);
    }

    public void ResetTransform()
    {
        if (!targetObject) return;
        targetScale = initialScale;
        targetRotation = initialRotation;
        hasRotated = false;
    }
}
