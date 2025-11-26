using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Keluar Aplikasi...");

        // Khusus APK / Build
        Application.Quit();

        // Agar bekerja di editor (opsional)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
