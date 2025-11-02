using UnityEngine;
using System.Collections;

public class MascotUIManager_Fade : MonoBehaviour
{
    [Header("UI Panels (Maskot)")]
    public CanvasGroup maskotScanPanel;         // “Ayo scan dulu objeknya!”
    public CanvasGroup maskotScannedPanel;      // “Nah, kamu sudah ngescan!”
    public CanvasGroup maskotRotatePanel;       // “Coba putar objeknya 🔄”
    public CanvasGroup maskotRotatedPanel;      // 🆕 “Keren! Kamu sudah memutar objeknya 🎉”
    public CanvasGroup maskotTranslatePanel;    // “Klik tombol Translate & Sound...”
    public CanvasGroup maskotAngryPanel;        // 😡 “Kok belum juga di-scan?!”

    [Header("Panel Tombol Translate & Audio")]
    public GameObject buttonPanel;              // Panel tombol

    [Header("Panel Utama (setelah maskot terakhir)")]
    public GameObject mainPanel;                // Panel utama

    [Header("Fade Settings")]
    public float fadeDuration = 0.8f;
    public float waitBetweenPanels = 0.3f;
    public float angryDelay = 8f;
    public float scannedDisplayTime = 2f;
    public float rotatedDisplayTime = 2f;       // 🆕 waktu tampil panel “Kamu sudah memutar!”

    private bool targetFound = false;
    private bool hasRotated = false;

    void Start()
    {
        // Setup awal
        SetPanel(maskotScanPanel, true, 0);
        SetPanel(maskotScannedPanel, false, 0);
        SetPanel(maskotRotatePanel, false, 0);
        SetPanel(maskotRotatedPanel, false, 0);
        SetPanel(maskotTranslatePanel, false, 0);
        SetPanel(maskotAngryPanel, false, 0);
        buttonPanel.SetActive(false);
        if (mainPanel) mainPanel.SetActive(false);

        StartCoroutine(FadeIn(maskotScanPanel));
        StartCoroutine(WaitForAngryPanel());
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

    IEnumerator WaitForAngryPanel()
    {
        yield return new WaitForSeconds(angryDelay);
        if (targetFound) yield break;

        yield return StartCoroutine(FadeOut(maskotScanPanel));
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(FadeIn(maskotAngryPanel));
    }

    public void OnTargetFound()
    {
        if (targetFound) return;
        targetFound = true;

        StopAllCoroutines();
        StartCoroutine(TransitionAfterScan());
    }

    IEnumerator TransitionAfterScan()
    {
        if (maskotScanPanel) yield return StartCoroutine(FadeOut(maskotScanPanel));
        if (maskotAngryPanel) yield return StartCoroutine(FadeOut(maskotAngryPanel));

        yield return new WaitForSeconds(waitBetweenPanels);
        yield return StartCoroutine(FadeIn(maskotScannedPanel));
        yield return new WaitForSeconds(scannedDisplayTime);
        yield return StartCoroutine(FadeOut(maskotScannedPanel));

        yield return new WaitForSeconds(waitBetweenPanels);
        yield return StartCoroutine(FadeIn(maskotRotatePanel));
    }

    public void OnObjectRotated()
    {
        if (hasRotated) return;
        hasRotated = true;

        StopAllCoroutines();
        StartCoroutine(TransitionAfterRotated());
    }

    IEnumerator TransitionAfterRotated()
    {
        yield return StartCoroutine(FadeOut(maskotRotatePanel));
        yield return new WaitForSeconds(waitBetweenPanels);
        yield return StartCoroutine(FadeIn(maskotRotatedPanel));

        yield return new WaitForSeconds(rotatedDisplayTime);
        yield return StartCoroutine(FadeOut(maskotRotatedPanel));

        yield return new WaitForSeconds(waitBetweenPanels);
        yield return StartCoroutine(FadeIn(maskotTranslatePanel));

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeOut(maskotTranslatePanel));

        buttonPanel.SetActive(true);

        if (mainPanel != null)
        {
            yield return new WaitForSeconds(0.5f);
            mainPanel.SetActive(true);
        }
    }
}
