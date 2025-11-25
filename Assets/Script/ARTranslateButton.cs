using UnityEngine;

public class ARTranslateButton : MonoBehaviour
{
    public void OnTranslate()
    {
        if (ARWordToggle.activeTarget != null)
            ARWordToggle.activeTarget.ToggleLanguage();
    }

    public void OnPlaySound()
    {
        if (ARWordToggle.activeTarget != null)
            ARWordToggle.activeTarget.PlayAudio();
    }
}
