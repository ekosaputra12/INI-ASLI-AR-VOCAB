using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject[] panels;

    private void Start()
    {
        string targetPanel = SceneBridge.panelToOpen;

        foreach (var p in panels)
        {
            if (p.name == targetPanel)
                p.SetActive(true);
            else
                p.SetActive(false);
        }
    }
}
