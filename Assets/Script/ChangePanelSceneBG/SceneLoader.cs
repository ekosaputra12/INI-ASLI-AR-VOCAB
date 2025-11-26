using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    public AudioSource clickSound;   // drag audio click ke sini
    public float delay = 0.2f;       // waktu tunggu sebelum pindah scene

    public void LoadScene()
    {
        StartCoroutine(PlaySoundAndLoad());
    }

    IEnumerator PlaySoundAndLoad()
    {
        if (clickSound != null)
            clickSound.Play();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }
}
