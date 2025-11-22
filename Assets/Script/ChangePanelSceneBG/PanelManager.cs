using UnityEngine;
using System.Collections;

public class PanelManager : MonoBehaviour
{
    public CanvasGroup splashGroup;

    public GameObject panelSplash;
    public GameObject panelCutsceneIntro;
    public GameObject panelMenu;

    public float fadeDuration = 1.5f;
    public float splashWait = 1.5f;

    void Start()
    {
        StartCoroutine(SplashSequence());
    }

    IEnumerator SplashSequence()
    {
        // Aktif/nonaktif panel
        panelSplash.SetActive(true);
        panelCutsceneIntro.SetActive(false);
        panelMenu.SetActive(false);

        // Fade in & fade out
        splashGroup.alpha = 0;
        yield return StartCoroutine(FadeCanvasGroup(splashGroup, 0, 1, fadeDuration));
        yield return new WaitForSeconds(splashWait);
        yield return StartCoroutine(FadeCanvasGroup(splashGroup, 1, 0, fadeDuration));

        StartCutsceneIntro();
    }

    void StartCutsceneIntro()
    {
        panelSplash.SetActive(false);
        panelCutsceneIntro.SetActive(true);
        panelMenu.SetActive(false);
    }

    // 🎮 Masuk ke menu utama
    public void StartMenu()
    {
        panelCutsceneIntro.SetActive(false);
        panelMenu.SetActive(true);
    }

    // 🔙 BACK (jika ada tombol kembali)
    public void BackToHome()
    {
        panelCutsceneIntro.SetActive(false);
        panelMenu.SetActive(true);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        cg.alpha = to;
    }
}
