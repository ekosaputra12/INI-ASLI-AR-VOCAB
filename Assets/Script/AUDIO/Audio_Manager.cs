using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager Instance { get; private set; }

    public AudioSource BMGSource;
    public AudioSource SFXSource;

    private void Awake()
    {
        BMGSource.playOnAwake = true;
        BMGSource.loop = true;

        SFXSource.playOnAwake = false;
        SFXSource.loop = false;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add your audio management methods here
    public void OnClick_BtnSFX()
    {
        SFXSource.Play();
    }
}
