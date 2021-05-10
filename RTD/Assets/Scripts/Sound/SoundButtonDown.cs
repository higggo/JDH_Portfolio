using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtonDown : MonoBehaviour
{
    public AudioClip Audio_Button1;
    public AudioClip Audio_Button2;

    public void AudioButton2()
    {
        SoundManager.I.PlayEffectSound(Audio_Button2);
    }
    public void AudioButton1()
    {
        SoundManager.I.PlayEffectSound(Audio_Button1);
    }
}
