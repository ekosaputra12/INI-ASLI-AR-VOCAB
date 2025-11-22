using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Pindah ke scene berdasarkan nama
    public void GantiScene(string namaScene)
    {
        SceneManager.LoadScene(namaScene);
    }

    // Contoh: keluar dari game (kalau diperlukan)
    public void KeluarGame()
    {
        Debug.Log("Keluar dari game...");
        Application.Quit();
    }
}
