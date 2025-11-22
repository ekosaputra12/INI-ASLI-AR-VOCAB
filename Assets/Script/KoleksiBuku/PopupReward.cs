// PopupReward.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PopupReward : MonoBehaviour
{
    public static PopupReward Instance;

    [Header("UI References")]
    public CanvasGroup canvasGroup;       // set di Inspector (Canvas Group pada root popup)
    public RectTransform panelPopup;      // panel utama (scale anim)
    public Image cardImage;               // image untuk menampilkan sprite kartu
    public TextMeshProUGUI textNamaCard;  // text nama kartu
    public Button buttonOK;               // tombol OK

    [Header("Anim Settings")]
    public float fadeDuration = 0.35f;
    public float popDuration = 0.45f;
    public float visibleDuration = 0f; // jika mau auto-hide (0 = tidak auto-hide)

    private void Awake()
    {
        // Singleton ringan
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        // awal tersembunyi
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        if (buttonOK != null)
        {
            buttonOK.onClick.AddListener(ClosePopup);
        }
    }

    public void Show(string cardID, Sprite sprite)
    {
        Debug.Log("📩 [PopupReward] Show() DIPANGGIL untuk kartu: " + cardID);

        StopAllCoroutines();

        // Update UI text & image
        if (textNamaCard != null)
        {
            textNamaCard.text = "Kamu mendapatkan: " + cardID;
            Debug.Log("📝 [PopupReward] Text berhasil diubah");
        }
        else Debug.LogWarning("⚠ textNamaCard belum terhubung!");

        if (cardImage != null)
        {
            cardImage.sprite = sprite;
            Debug.Log("🖼 [PopupReward] Sprite kartu berhasil di-set");
        }
        else Debug.LogWarning("⚠ cardImage belum terhubung!");

        // Aktifkan panel popup
        if (panelPopup != null)
        {
            Debug.Log("📦 [PopupReward] Panel popup diaktifkan");
            panelPopup.gameObject.SetActive(true);
            panelPopup.localScale = Vector3.zero;
        }
        else Debug.LogWarning("⚠ panelPopup belum terhubung!");

        // Fade & Pop Anim
        if (canvasGroup != null)
        {
            Debug.Log("✨ [PopupReward] Mulai fade-in dan pop animation");
            canvasGroup.gameObject.SetActive(true);
            canvasGroup.alpha = 0;
            StartCoroutine(FadeInAndPop());
        }
        else
        {
            Debug.LogWarning("⚠ CanvasGroup belum terhubung! Menampilkan tanpa animasi.");
            if (panelPopup != null)
                panelPopup.localScale = Vector3.one;
        }
    }


    private IEnumerator FadeInAndPop()
    {
        Debug.Log("🎞 [PopupReward] FadeInAndPop coroutine mulai");

        float duration = 0.3f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, time / duration);
            panelPopup.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time / duration);
            yield return null;
        }

        Debug.Log("🎉 [PopupReward] Fade & Pop selesai, popup tampil penuh");
    }


    public void ClosePopup()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutAndClose());
    }

    IEnumerator FadeOutAndClose()
    {
        // fade out
        if (canvasGroup != null)
        {
            float t = 0f;
            float start = canvasGroup.alpha;
            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(start, 0f, t / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        // kecilkan panel
        if (panelPopup != null)
        {
            float elapsed = 0f;
            Vector3 from = panelPopup.localScale;
            Vector3 to = Vector3.zero;
            float dur = popDuration * 0.6f;
            while (elapsed < dur)
            {
                elapsed += Time.unscaledDeltaTime;
                panelPopup.localScale = Vector3.Lerp(from, to, elapsed / dur);
                yield return null;
            }
            panelPopup.localScale = to;
        }
    }
}
