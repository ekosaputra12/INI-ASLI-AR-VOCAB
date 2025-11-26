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

    [Header("ID Kartu Setelah Puzzle Selesai")]
    public string kartuID;

    [Header("Handler Vuforia Card")]
    public VuforiaCardHandler handler;   // 🔥 drag image target ke field ini di Inspector

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

        int batasSetengah = Mathf.CeilToInt(totalSlot / 2f);

        if (jumlahBenar >= totalSlot)
        {
            StartCoroutine(GantiEkspresi(ekspresiSenang, "🔥 Ajgo bangett!! 🔥"));

            if (!string.IsNullOrEmpty(kartuID))
            {
                Debug.Log("Puzzle selesai, spawn kartu: " + kartuID);

                if (handler != null && handler.targetDetected)
                {
                    CardSpawner.Instance?.SpawnCard(kartuID);
                    CollectionManager.Instance?.AddToCollection(kartuID);
                }
                else
                {
                    Debug.Log("Puzzle benar, tapi target belum discan");
                }
            }
        }
        else if (jumlahBenar >= batasSetengah)
        {
            StartCoroutine(GantiEkspresi(ekspresiSenang, "Yeay! Dikit lagi! 💪"));
        }
        else
        {
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
