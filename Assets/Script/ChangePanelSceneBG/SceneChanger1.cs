using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahPnaeldiscene : MonoBehaviour
{
    public void GantiSceneKePanel(string namaScene, string panelTujuan)
    {
        SceneBridge.panelToOpen = panelTujuan;
        SceneManager.LoadScene(namaScene);
    }
}
