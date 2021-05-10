using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustEffectSoundSlider : MonoBehaviour
{
    private void OnEnable()
    {
        this.GetComponent<Slider>().value = 1f - PlayerPrefs.GetFloat("GameEffectVolume");
    }
    public void SetSoundValue()
    {
        SoundManager.I.SetEffectVolume(this.GetComponent<Slider>().value);
    }
}
