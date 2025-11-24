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
            DontDestroyOnLoad(gameObject);

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
        Wrapper wrapper = new Wrapper(koleksi);
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString("koleksi", json);
        PlayerPrefs.Save();
    }

    private void LoadCollection()
    {
        if (PlayerPrefs.HasKey("koleksi"))
        {
            string json = PlayerPrefs.GetString("koleksi");
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);

            if (wrapper != null && wrapper.items != null)
                koleksi = wrapper.items;
        }
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<string> items = new List<string>();

        // HARUS ADA untuk JsonUtility
        public Wrapper() { }

        public Wrapper(List<string> list)
        {
            items = list;
        }
    }

    public List<string> GetAllCollection()
    {
        return new List<string>(koleksi);
    }
}
