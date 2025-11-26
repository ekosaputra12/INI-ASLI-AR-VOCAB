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
        DragHandler dragHandler = draggedObj.GetComponent<DragHandler>();

        string draggedLetter = dragHandler.letter.Trim().ToUpper();
        string expected = expectedLetter.Trim().ToUpper();

        // Jika slot sudah terisi
        if (transform.childCount > 0)
        {
            dragHandler.KembaliKeAsal(true);
            return;
        }

        if (draggedLetter == expected)
        {
            // BENAR
            draggedObj.transform.SetParent(transform, false);
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
