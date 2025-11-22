using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;

    public List<string> collectedCards = new List<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void AddToCollection(string cardID)
    {
        if (!collectedCards.Contains(cardID))
        {
            collectedCards.Add(cardID);
            Debug.Log("Kartu ditambahkan ke koleksi: " + cardID);
        }
    }
}
