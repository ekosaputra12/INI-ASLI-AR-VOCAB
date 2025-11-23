using UnityEngine;
using UnityEngine.UI;

public class CollectionDisplay : MonoBehaviour
{
    public Transform gridParent;  // tempat spawn card UI
    public GameObject cardUIPrefab; // prefab kartu versi UI (flat)

    void Start()
    {
        RefreshCollection();
    }

    public void RefreshCollection()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        foreach (string id in CollectionManager.Instance.collectedCards)
        {
            GameObject newCard = Instantiate(cardUIPrefab, gridParent);

            // set nama atau gambar sesuai ID
            newCard.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = id;
        }
    }
}
