using UnityEngine;
using Vuforia;

public class VuforiaCardHandler : MonoBehaviour
{
    public string cardID;



    private ObserverBehaviour observer;

    // Status apakah target sudah terdeteksi
    public bool targetDetected = false;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        observer.OnTargetStatusChanged += OnStatusChanged;
    }

    private void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            Debug.Log("TARGET TERDETEKSI: " + cardID);
            targetDetected = true;
        }
        else
        {
            targetDetected = false;
        }
    }
}
