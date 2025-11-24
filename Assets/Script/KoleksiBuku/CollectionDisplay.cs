using UnityEngine;
using UnityEngine.UI;

public class CollectionDisplay : MonoBehaviour
{
    [Header("Parent Grid Koleksi (Content di ScrollView)")]
    public Transform gridParent;

    [Header("Prefab UI Kartu")]
    public GameObject cardUIPrefab;

    void OnEnable()
    {
        TampilkanKoleksi();
    }

    void TampilkanKoleksi()
    {
        if (CollectionManager.Instance == null)
        {
            Debug.LogError("CollectionManager tidak ditemukan! Pastikan tidak terhapus saat pindah scene.");
            return;
        }

        // 🧹 Bersihkan dulu isi lama
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        var koleksi = CollectionManager.Instance.GetAllCollection();

        foreach (var id in koleksi)
        {
            // 🔹 Buat UI kartu baru
            GameObject obj = Instantiate(cardUIPrefab, gridParent);

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;
            rt.anchoredPosition = Vector2.zero;

            // 🔹 Masukkan sprite gambar kartu
            Image img = obj.GetComponent<Image>();
            if (img != null)
            {
                Sprite s = CardSpawner.Instance.GetCardSprite(id);
                if (s != null)
                    img.sprite = s;
                else
                    Debug.LogWarning($"Sprite untuk ID {id} tidak ditemukan.");
            }
            else
            {
                Debug.LogWarning("Prefab UI tidak punya komponen Image!");
            }
        }
    }
}
