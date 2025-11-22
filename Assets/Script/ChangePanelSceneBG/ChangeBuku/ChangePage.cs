using UnityEngine;
using UnityEngine.UI;

public class ChangePage : MonoBehaviour
{
    public GameObject[] pages; // Semua halaman buku
    public Button nextButton;
    public Button backButton;

    private int currentPage = 0;

    void Start()
    {
        UpdatePages();
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePages();
        }
    }

    public void BackPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePages();
        }
    }

    void UpdatePages()
    {
        // Loop untuk hidup/matikan halaman
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }

        // Button control
        backButton.interactable = currentPage > 0;
        nextButton.interactable = currentPage < pages.Length - 1;
    }
}
