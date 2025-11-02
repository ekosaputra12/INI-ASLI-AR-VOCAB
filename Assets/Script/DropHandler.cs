using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    [Header("Huruf yang benar untuk slot ini")]
    public string expectedLetter;

    [Header("Maskot Manager")]
    public MaskotManager maskotManager;

    private bool sudahBenar = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        GameObject draggedObj = eventData.pointerDrag;
        string draggedLetter = draggedObj.name.Replace("Image_", "");
        DragHandler dragHandler = draggedObj.GetComponent<DragHandler>();

        // Jika slot sudah terisi, abaikan
        if (transform.childCount > 0)
        {
            dragHandler.KembaliKeAsal(true);
            return;
        }

        if (draggedLetter == expectedLetter)
        {
            // BENAR
            draggedObj.transform.SetParent(transform);
            draggedObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            if (!sudahBenar)
            {
                sudahBenar = true;
                maskotManager?.HurufBenar();
            }
        }
        else
        {
            // SALAH
            dragHandler.KembaliKeAsal(true);
            maskotManager?.HurufSalah();
        }
    }
}
