using UnityEngine;
using System.Collections;

public class CutsceneIconSwap : MonoBehaviour
{
    public RectTransform iconA;
    public RectTransform iconB;

    public float swapDuration = 0.5f;  // Waktu perpindahan icon
    public float cutsceneDuration = 5f;

    void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        float timer = 0f;

        // Loop selama cutscene berlangsung
        while (timer < cutsceneDuration)
        {
            yield return StartCoroutine(SwapIcons());
            timer += swapDuration;
        }

        // Setelah selesai cutscene
        Debug.Log("Cutscene selesai");
    }

    IEnumerator SwapIcons()
    {
        Vector3 startA = iconA.anchoredPosition;
        Vector3 startB = iconB.anchoredPosition;

        float t = 0;

        while (t < swapDuration)
        {
            t += Time.deltaTime;
            float lerp = t / swapDuration;

            iconA.anchoredPosition = Vector3.Lerp(startA, startB, lerp);
            iconB.anchoredPosition = Vector3.Lerp(startB, startA, lerp);

            yield return null;
        }
    }
}
