using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Huruf yang dibawa object ini")]
    public string letter;

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
        parentAsal = transform.parent;
        posisiAwal = rectTransform.anchoredPosition;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // Pindah ke layer tertinggi agar tidak tertutup UI
        transform.SetParent(canvas.transform, true);
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
        transform.SetParent(parentAsal, true);

        if (animasi)
            StartCoroutine(AnimasiBalik());
        else
            rectTransform.anchoredPosition = posisiAwal;
    }

    private IEnumerator AnimasiBalik()
    {
        Vector3 startPos = rectTransform.anchoredPosition;
        float waktu = 0f;
        while (waktu < 1f)
        {
            waktu += Time.deltaTime * 4f;
            rectTransform.anchoredPosition = Vector3.Lerp(startPos, posisiAwal, waktu);
            yield return null;
        }
        rectTransform.anchoredPosition = posisiAwal;
    }
}
