using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AdjustBGMSoundSlider : MonoBehaviour
{
    private void OnEnable()
    {
        this.GetComponent<Slider>().value = 1f - PlayerPrefs.GetFloat("GameMusicVolume");
    }
    public void SetSoundValue()
    {
        SoundManager.I.SetMusicVolume(this.GetComponent<Slider>().value);
    }
}
