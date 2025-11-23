using UnityEngine;
using System.Collections;

public class CardSpawner : MonoBehaviour
{
    public static CardSpawner Instance;

    [Header("Tempat Spawn Kartu")]
    public Transform spawnPoint;

    [Header("List Prefab Kartu Berdasarkan ID")]
    public CardItem[] cards;

    [System.Serializable]
    public class CardItem
    {
        public string id;
        public GameObject prefab;
        public Sprite sprite; // sprite flat untuk UI
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

        foreach (var item in cards)
        {
            if (item.id == id)
            {
                Debug.Log("Spawn kartu: " + id);

                GameObject newCard = Instantiate(item.prefab, spawnPoint.position, Quaternion.identity);

                // jalankan animasi scale pop-in
                StartCoroutine(ScalePop(newCard));

                return;
            }
        }

        Debug.LogWarning("Kartu dengan ID: " + id + " tidak ditemukan di CardSpawner!");
    }

    private IEnumerator ScalePop(GameObject go)
    {
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        float duration = 0.35f;
        float t = 0f;

        go.transform.localScale = startScale;

        while (t < duration)
        {
            t += Time.deltaTime;
            go.transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            yield return null;
        }

        go.transform.localScale = endScale;
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
