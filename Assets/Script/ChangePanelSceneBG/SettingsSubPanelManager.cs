using UnityEngine;

public class SettingsSubPanelManager : MonoBehaviour
{
    [Header("Sub Panels")]
    public GameObject bgAudio;
    public GameObject bgKredit;

    void Start()
    {
        // Awal: tampilkan Music, sembunyikan Volume
        ShowAudio();
    }

    public void ShowAudio()
    {
        bgAudio.SetActive(true);
        bgKredit.SetActive(false);
    }

    public void ShowKredit()
    {
        bgAudio.SetActive(false);
        bgKredit.SetActive(true);
    }
}
