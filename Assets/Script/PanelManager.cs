using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject panelMenu;
    public GameObject panelGame;
    public GameObject panelSettings;

    public void ShowPanel(GameObject panelToShow)
    {
        // Nonaktifkan semua panel dulu
        panelMenu.SetActive(false);
        panelGame.SetActive(false);
        panelSettings.SetActive(false);

        // Aktifkan panel yang dipilih
        panelToShow.SetActive(true);
    }
}
