using UnityEngine;
using TMPro;
using System.Collections;

public class TMPTypewriter : MonoBehaviour
{
    public TMP_Text tmpText;           // Tempat teks TMP
    [TextArea] public string fullText; // Teks penuh
    public float typeSpeed = 0.03f;    // Kecepatan muncul huruf

    private bool isTyping = false;
    private bool skip = false;

    void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        skip = false;

        tmpText.text = "";
        int charIndex = 0;

        while (charIndex < fullText.Length)
        {
            if (skip)
            {
                // Jika klik untuk skip, langsung tampilkan semua
                tmpText.text = fullText;
                break;
            }

            tmpText.text = fullText.Substring(0, charIndex + 1);
            charIndex++;

            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    // Dipanggil saat user klik / tekan tombol
    public void SkipOrNext()
    {
        if (isTyping)
        {
            skip = true; // Skip typing
        }
        else
        {
            Debug.Log("Lanjut ke dialog berikut");
        }
    }
}
