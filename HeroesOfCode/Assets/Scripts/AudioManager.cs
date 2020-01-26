using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [HideInInspector]
    public Sound currentBackgroundMusic;

    void Awake()
    {
        // This object should be unique in a scene
        GameObject[] objs = GameObject.FindGameObjectsWithTag("AudioManager");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);


        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
        }

        currentBackgroundMusic = null;
    }

    public void SetBackgroundMusicAndPlayIt(string trackName)
    {
        Sound s = Array.Find(sounds, sound => sound.name.Equals(trackName));
        if (s == null)
            return;
        currentBackgroundMusic = s;
        currentBackgroundMusic.volume = s.volume;
        Debug.Log($"Now playing: {currentBackgroundMusic.name}");
//        if (currentBackgroundMusic.name.Equals("Theme"))
//        {
//            currentBackgroundMusic.source.time = 2.5f;
//        }
        currentBackgroundMusic.source.Play();
    }

    public void Play(string trackName)
    {
        Sound s = Array.Find(sounds, sound => sound.name.Equals(trackName));
        s?.source.Play();
    }

    public static class FadeAudioSource
    {

        public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            
            yield break;
        }
    }

    public void FinishAudioSource()
    {
        currentBackgroundMusic.source.Stop();
        currentBackgroundMusic.source.volume = currentBackgroundMusic.volume;
    }
}