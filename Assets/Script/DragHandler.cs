using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAsal;
    [HideInInspector] public Vector3 posisiAwal;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Simpan posisi & parent asal
        parentAsal = transform.parent;
        posisiAwal = rectTransform.anchoredPosition;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // Pindah ke atas layer lain biar nggak ketutupan
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void KembaliKeAsal(bool animasi = true)
    {
        transform.SetParent(parentAsal);
        if (animasi)
            StartCoroutine(AnimasiBalik());
        else
            rectTransform.anchoredPosition = posisiAwal;
    }

    private IEnumerator AnimasiBalik()
    {
        Vector3 startPos = rectTransform.anchoredPosition;
        float waktu = 0f;
        while (waktu < 0.25f)
        {
            waktu += Time.deltaTime * 4f; // kecepatan animasi
            rectTransform.anchoredPosition = Vector3.Lerp(startPos, posisiAwal, waktu);
            yield return null;
        }
        rectTransform.anchoredPosition = posisiAwal;
    }
}
