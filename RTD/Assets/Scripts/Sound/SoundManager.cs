using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    AudioSource musicPlayer = null;
    AudioSource effectsound = null;

    float musicVolume = 1.0f;
    float effectVolume = 1.0f;

    void Awake()
    {
        // 값이 존재하지 않으면 0.0f를 return한다.
        musicVolume = 1f - PlayerPrefs.GetFloat("GameMusicVolume");
        effectVolume = 1f - PlayerPrefs.GetFloat("GameEffectVolume");
    }

    // 기본적인 싱글톤 패턴을 사용한다.
    public static SoundManager I
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;
                if (instance == null)
                {
                    GameObject obj = Instantiate(Resources.Load("SoundManager")) as GameObject;
                    obj.name = "SoundManager";
                    instance = obj.GetComponent<SoundManager>();
                }
            }
            return instance;
        }
    }

    // BGM Player와 Effect Player를 나눠놓는게 좋다.
    public AudioSource MusicPlayer
    {
        get
        {
            if (musicPlayer == null)
            {
                musicPlayer = Camera.main.GetComponent<AudioSource>();
            }
            return musicPlayer;
        }
    }

    public AudioSource EffectSound
    {
        get
        {
            if (effectsound == null)
            {
                effectsound = GetComponent<AudioSource>();
            }
            return effectsound;
        }
    }

    public void PlayBGM(AudioClip source, bool bLoop = true)
    {
        MusicPlayer.Stop();
        MusicPlayer.clip = source;
        MusicPlayer.loop = bLoop;
        MusicPlayer.Play();
    }

    public void SetMusicVolume(float val)
    {
        val = Mathf.Clamp(val, 0f, 1f);
        musicVolume = val;
        MusicPlayer.volume = val;

        // 게임이 끝나도 저장되게끔 레지스트리를 생성하여 저장한다. (보안 문제 해결할 수 없음)
        PlayerPrefs.SetFloat("GameMusicVolume", 1f - val);
    }

    public void SetEffectVolume(float val)
    {
        val = Mathf.Clamp(val, 0f, 1f);
        effectVolume = val;
        EffectSound.volume = val;
        PlayerPrefs.SetFloat("GameEffectVolume", 1f - val);
    }

    public void PlayEffectSound(AudioClip source, float pitch = 1.0f)
    {
        EffectSound.pitch = pitch;
        EffectSound.PlayOneShot(source);
    }

    public void PlayEffectSound(GameObject obj, AudioClip eff)
    {
        AudioSource audio = obj.GetComponent<AudioSource>();
        if (audio == null)
        {
            audio = obj.AddComponent<AudioSource>();
        }
        audio.spatialBlend = 0.8f;
        audio.PlayOneShot(eff, effectVolume);
    }

    public void PlayEffectSound(GameObject obj, AudioClip eff, float spatialBlend)
    {
        AudioSource audio = obj.GetComponent<AudioSource>();
        if (audio == null)
        {
            audio = obj.AddComponent<AudioSource>();
        }
        audio.spatialBlend = spatialBlend;
        audio.PlayOneShot(eff, effectVolume);
    }

    public void PlayEffectSound(GameObject obj, AudioClip eff, float spatialBlend, float volume)
    {
        AudioSource audio = obj.GetComponent<AudioSource>();
        if (audio == null)
        {
            audio = obj.AddComponent<AudioSource>();
        }
        audio.spatialBlend = spatialBlend;
        volume *= effectVolume;
        audio.PlayOneShot(eff, (volume != 0.0f) ? volume : effectVolume);
    }
}
