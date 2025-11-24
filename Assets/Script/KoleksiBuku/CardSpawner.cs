using UnityEngine;
using System.Collections;

public class CardSpawner : MonoBehaviour
{
    public static CardSpawner Instance;

    [Header("Overlay Pop-Up (aktif saat kartu muncul)")]
    public GameObject popupOverlay;

    [Header("Tempat Spawn Kartu (UI / RectTransform)")]
    public Transform spawnPoint;

    [Header("List Prefab Kartu Berdasarkan ID")]
    public CardItem[] cards;

    [System.Serializable]
    public class CardItem
    {
        public string id;
        public GameObject prefab;   // Prefab UI (tanpa Canvas)
        public Sprite sprite;       // Sprite untuk koleksi
    }

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnCard(string id)
    {
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint belum diassign! Pastikan isi di Inspector.");
            return;
        }

        // 🔥 Aktifkan overlay saat kartu muncul
        if (popupOverlay != null)
            popupOverlay.SetActive(true);

        foreach (var item in cards)
        {
            if (item.id == id)
            {
                Debug.Log("Spawn kartu: " + id);

                // Spawn sebagai child dari spawnPoint
                GameObject newCard = Instantiate(item.prefab, spawnPoint, false);

                RectTransform rt = newCard.GetComponent<RectTransform>();
                rt.anchoredPosition = Vector2.zero;
                rt.localScale = Vector3.one;
                rt.localRotation = Quaternion.identity;

                StartCoroutine(ScalePop(newCard));
                return;
            }
        }

        Debug.LogWarning("Kartu dengan ID: " + id + " tidak ditemukan!");
    }

    public void ClosePopup()
    {
        // Hapus kartu yang muncul
        foreach (Transform child in spawnPoint)
        {
            Destroy(child.gameObject);
        }

        // Matikan overlay
        popupOverlay.SetActive(false);
    }

    private IEnumerator ScalePop(GameObject go)
    {
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.one;
        float duration = 0.35f;
        float t = 0f;

        go.transform.localScale = start;

        while (t < duration)
        {
            t += Time.deltaTime;
            go.transform.localScale = Vector3.Lerp(start, end, t / duration);
            yield return null;
        }

        go.transform.localScale = end;
    }

    public Sprite GetCardSprite(string id)
    {
        foreach (var item in cards)
        {
            if (item.id == id)
                return item.sprite;
        }

        Debug.LogWarning("Sprite untuk kartu ID " + id + " tidak ditemukan!");
        return null;
    }
}
