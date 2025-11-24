using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;

    private List<string> koleksi = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);   // WAJIB supaya data bertahan

            LoadCollection();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddToCollection(string id)
    {
        if (!koleksi.Contains(id))
        {
            koleksi.Add(id);
            SaveCollection();
            Debug.Log("Kartu " + id + " berhasil ditambahkan ke koleksi!");
        }
        else
        {
            Debug.Log("Kartu " + id + " sudah ada, tidak ditambah.");
        }
    }

    public bool HasCard(string id)
    {
        return koleksi.Contains(id);
    }

    private void SaveCollection()
    {
        string json = JsonUtility.ToJson(new Wrapper(koleksi));
        PlayerPrefs.SetString("koleksi", json);
    }

    private void LoadCollection()
    {
        if (PlayerPrefs.HasKey("koleksi"))
        {
            string json = PlayerPrefs.GetString("koleksi");
            koleksi = JsonUtility.FromJson<Wrapper>(json).items;
        }
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<string> items;
        public Wrapper(List<string> list) { items = list; }
    }

    public List<string> GetAllCollection()
    {
        return new List<string>(koleksi);
    }
}
