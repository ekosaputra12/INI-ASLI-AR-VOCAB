using UnityEngine;
using System.Collections;

public class MascotUIManager_Fade : MonoBehaviour
{
    [Header("UI Panels (Maskot)")]
    public CanvasGroup maskotScanPanel;      // “Ayo scan dulu objeknya!”
    public CanvasGroup maskotRotatePanel;    // “Kamu bisa rotate dan zoom objek ini!”
    public CanvasGroup maskotTranslatePanel; // “Klik tombol Translate & Sound...”

    [Header("Panel Tombol Translate & Audio")]
    public GameObject buttonPanel;           // Panel tombol

    [Header("Panel Utama (setelah maskot terakhir)")]
    public GameObject mainPanel;             // Panel utama yang muncul di akhir

    [Header("Fade Settings")]
    public float fadeDuration = 0.8f;
    public float waitBetweenPanels = 0.3f;

    private bool targetFound = false;
    private bool hasRotated = false;

    void Start()
    {
        // Awal: hanya panel scan aktif
        SetPanel(maskotScanPanel, true, 0);
        SetPanel(maskotRotatePanel, false, 0);
        SetPanel(maskotTranslatePanel, false, 0);
        buttonPanel.SetActive(false);
        if (mainPanel) mainPanel.SetActive(false);

        StartCoroutine(FadeIn(maskotScanPanel));
    }

    void SetPanel(CanvasGroup panel, bool active, float alpha)
    {
        if (panel == null) return;
        panel.gameObject.SetActive(active);
        panel.alpha = alpha;
        panel.interactable = active;
        panel.blocksRaycasts = active;
    }

    IEnumerator FadeIn(CanvasGroup panel)
    {
        panel.gameObject.SetActive(true);
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            panel.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        panel.alpha = 1;
    }

    IEnumerator FadeOut(CanvasGroup panel)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            panel.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        panel.alpha = 0;
        panel.gameObject.SetActive(false);
    }

    // ✅ Dipanggil dari event Vuforia (saat target terdeteksi)
    public void OnTargetFound()
    {
        if (!targetFound)
        {
            targetFound = true;
            StopAllCoroutines();
            StartCoroutine(TransitionToRotatePanel());
        }
    }

    IEnumerator TransitionToRotatePanel()
    {
        // Hilangkan panel scan → tampilkan panel rotate
        yield return StartCoroutine(FadeOut(maskotScanPanel));
        yield return new WaitForSeconds(waitBetweenPanels);
        yield return StartCoroutine(FadeIn(maskotRotatePanel));
    }

    // ✅ Dipanggil dari script rotasi (ketika user memutar objek)
    public void OnObjectRotated()
    {
        if (!hasRotated)
        {
            hasRotated = true;
            StopAllCoroutines();
            StartCoroutine(TransitionToTranslatePanel());
        }
    }

    IEnumerator TransitionToTranslatePanel()
    {
        // Hilangkan panel rotate → tampilkan panel translate
        yield return StartCoroutine(FadeOut(maskotRotatePanel));
        yield return new WaitForSeconds(waitBetweenPanels);
        yield return StartCoroutine(FadeIn(maskotTranslatePanel));
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeOut(maskotTranslatePanel));

        // Setelah selesai → tampilkan tombol translate & audio
        buttonPanel.SetActive(true);

        // 🟢 Setelah tombol tampil → munculkan panel utama
        if (mainPanel != null)
        {
            yield return new WaitForSeconds(0.5f);
            mainPanel.SetActive(true);
        }
    }
}
