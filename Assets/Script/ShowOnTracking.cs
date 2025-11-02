using UnityEngine;
using Vuforia;

public class ShowOnTracking : MonoBehaviour
{
    [Header("UI atau Objek yang ingin dimunculkan saat target terdeteksi")]
    public GameObject uiCanvas; // drag Canvas kamu ke sini

    private ObserverBehaviour observer;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        if (observer)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        if (uiCanvas)
            uiCanvas.SetActive(false); // sembunyikan di awal
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (uiCanvas == null) return;

        // Kalau target terdeteksi / sedang dilacak
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            uiCanvas.SetActive(true);
        }
        else
        {
            uiCanvas.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (observer)
            observer.OnTargetStatusChanged -= OnTargetStatusChanged;
    }
}
