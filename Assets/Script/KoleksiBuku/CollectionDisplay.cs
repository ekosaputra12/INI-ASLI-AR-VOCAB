using UnityEngine;
using UnityEngine.UI;

public class CollectionDisplay : MonoBehaviour
{
    [Header("Parent Grid Koleksi (Content di ScrollView)")]
    public Transform gridParent;

    [System.Serializable]
    public class CardUIPrefabItem
    {
        public string id;
        public GameObject prefabUI;
    }

    [Header("List Prefab UI untuk Koleksi berdasarkan ID")]
    public CardUIPrefabItem[] uiPrefabs;

    void OnEnable()
    {
        TampilkanKoleksi();
    }

    void TampilkanKoleksi()
    {
        if (CollectionManager.Instance == null)
        {
            Debug.LogError("CollectionManager tidak ditemukan!");
            return;
        }

        // Bersihkan isi lama
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        var koleksi = CollectionManager.Instance.GetAllCollection();

        foreach (var id in koleksi)
        {
            // 🔍 Ambil prefab UI yang sesuai ID
            GameObject prefabToSpawn = GetPrefabByID(id);
            if (prefabToSpawn == null)
            {
                Debug.LogWarning($"Prefab UI untuk ID {id} tidak ditemukan!");
                continue;
            }

            // Spawn prefab UI
            GameObject obj = Instantiate(prefabToSpawn, gridParent);

            // Setup posisi UI
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;
            rt.anchoredPosition = Vector2.zero;

            // 🔹 Masukkan sprite kartu
            Image img = obj.GetComponent<Image>();
            if (img != null)
            {
                Sprite s = CardSpawner.Instance.GetCardSprite(id);
                if (s != null)
                    img.sprite = s;
            }
        }
    }

    GameObject GetPrefabByID(string id)
    {
        foreach (var item in uiPrefabs)
        {
            if (item.id == id)
                return item.prefabUI;
        }
        return null;
    }
}
