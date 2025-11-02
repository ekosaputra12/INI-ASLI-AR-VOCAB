using UnityEngine;
using TMPro;
using System.Collections;

public class MaskotManager : MonoBehaviour
{
    [Header("Referensi UI Maskot")]
    public UnityEngine.UI.Image imageMaskot;
    public TextMeshProUGUI textMaskot;

    [Header("Ekspresi Maskot")]
    public Sprite ekspresiSemangat;
    public Sprite ekspresiSenang;
    public Sprite ekspresiSedih;

    [Header("Jumlah Slot Total")]
    public int totalSlot = 5;
    private int jumlahBenar = 0;

    void Start()
    {
        TampilkanSemangat();
    }

    public void ResetProgres()
    {
        jumlahBenar = 0;
        TampilkanSemangat();
    }

    public void TampilkanSemangat()
    {
        imageMaskot.sprite = ekspresiSemangat;
        textMaskot.text = "Ayo drag huruf ke panel jawaban!";
    }

    public void HurufBenar()
    {
        jumlahBenar++;

        StopAllCoroutines();

        // Hitung batas setengah dari total slot
        int batasSetengah = Mathf.CeilToInt(totalSlot / 2f);

        if (jumlahBenar >= totalSlot)
        {
            // Semua benar
            StartCoroutine(GantiEkspresi(ekspresiSenang, "🔥 Ajgo bangett!! 🔥"));
        }
        else if (jumlahBenar >= batasSetengah)
        {
            // Sudah setengah atau lebih
            StartCoroutine(GantiEkspresi(ekspresiSenang, "Yeay! Dikit lagi! 💪"));
        }
        else
        {
            // Baru mulai, kasih semangat biasa
            StartCoroutine(GantiEkspresi(ekspresiSenang, "Keren! Teruskan ya 😄"));
        }
    }

    public void HurufSalah()
    {
        StopAllCoroutines();
        StartCoroutine(GantiEkspresi(ekspresiSedih, "Kamu salah, coba lagi ya 😅"));
    }

    private IEnumerator GantiEkspresi(Sprite spriteBaru, string teks)
    {
        imageMaskot.sprite = spriteBaru;
        textMaskot.text = teks;
        yield return new WaitForSeconds(2f);
        TampilkanSemangat();
    }
}
