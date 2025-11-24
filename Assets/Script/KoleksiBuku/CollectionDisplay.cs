using UnityEngine;

public class CollectionDisplay : MonoBehaviour
{
    [Header("Parent Grid Koleksi")]
    public Transform gridParent;

    [Header("Prefab UI Kartu (Flat)")]
    public GameObject cardUIPrefab;

    void Start()
    {
        TampilkanKoleksi();
    }

    void TampilkanKoleksi()
    {
        if (CollectionManager.Instance == null)
        {
            Debug.LogError("CollectionManager tidak ditemukan! Pastikan ada di Scene Kuis, dan jangan destroy on load.");
            return;
        }

        var koleksi = CollectionManager.Instance.GetAllCollection();

        foreach (var id in koleksi)
        {
            // 🔹 Buat UI kartu baru
            GameObject obj = Instantiate(cardUIPrefab, gridParent);

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;

            // 🔹 Isi gambar kartu
            var img = obj.GetComponent<UnityEngine.UI.Image>();
            if (img != null)
            {
                img.sprite = CardSpawner.Instance.GetCardSprite(id);
            }
        }
    }
}
