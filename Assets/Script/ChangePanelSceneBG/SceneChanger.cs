using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public AudioSource audioSource;   // tempat SFX button
    public AudioClip clickSound;      // suara tombol

    public void GantiScene(string namaScene)
    {
        StartCoroutine(PlayAndChange(namaScene));
    }

    IEnumerator PlayAndChange(string namaScene)
    {
        // Mainkan suara
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);

            // Tunggu sampai suara selesai
            yield return new WaitForSeconds(clickSound.length);
        }

        // Pindah scene
        SceneManager.LoadScene(namaScene);
    }

    public void KeluarGame()
    {
        Application.Quit();
    }
}
