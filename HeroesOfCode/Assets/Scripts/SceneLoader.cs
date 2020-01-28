﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public float fadeOutSpeed;
    public float waitTime;
    public float fadeInSpeed;
    public string backgroundMusicName;

    private AudioManager audioManager;

    //    void Awake()
    [HideInInspector] public bool FinalBattle;

    void Start()
    {
        transition.SetFloat("FadeInMultiplier", fadeInSpeed);
        transition.SetFloat("FadeOutMultiplier", fadeOutSpeed);
        StartCoroutine(loadCurrentScene());
        audioManager = FindObjectOfType<AudioManager>();

//        Debug.Log(audioManager.currentBackgroundMusic.name);
        // TODO: CHECK
        if (audioManager.currentBackgroundMusic != null)
            audioManager.FinishAudioSource();

        audioManager.SetBackgroundMusicAndPlayIt(backgroundMusicName);
        
    }

    IEnumerator loadCurrentScene()
    {
        yield return new WaitForSeconds(waitTime);
        transition.enabled = true;
    }

    public void LoadStartScene()
    {
        StartCoroutine(PlayTransition(0));
        StartCoroutine(
            AudioManager.FadeAudioSource.StartFade(audioManager.currentBackgroundMusic.source, fadeOutSpeed, 0));
    }


    public void LoadMapScene()
    {
        StartCoroutine(PlayTransition(1));
        StartCoroutine(
            AudioManager.FadeAudioSource.StartFade(audioManager.currentBackgroundMusic.source, fadeOutSpeed, 0));
        //        StartCoroutine(LoadSceneAsynchronously(1));

        //        SceneManager.LoadScene(1);
    }

    public void LoadBattleScene()
    {
        StartCoroutine(PlayTransition(2));
        StartCoroutine(
            AudioManager.FadeAudioSource.StartFade(audioManager.currentBackgroundMusic.source, fadeOutSpeed, 0));
    }

    public void SetFinalBattleMusic()
    {
        audioManager.FinishAudioSource();
        audioManager.SetBackgroundMusicAndPlayIt("FinalBattle");
    }

    public void LoadBadEndingScene()
    {
        StartCoroutine(PlayTransition(3));
        StartCoroutine(
            AudioManager.FadeAudioSource.StartFade(audioManager.currentBackgroundMusic.source, fadeOutSpeed, 0));
    }

    public void LoadGoodEndingScene()
    {
        StartCoroutine(PlayTransition(4));
        StartCoroutine(
            AudioManager.FadeAudioSource.StartFade(audioManager.currentBackgroundMusic.source, fadeOutSpeed, 0));
    }

    IEnumerator PlayTransition(int sceneIndex)
    {
        transition.SetTrigger("EndScene");
        yield return new WaitForSeconds(fadeOutSpeed);
        Debug.Log("Played");
        StartCoroutine(LoadSceneAsynchronously(sceneIndex));
    }

    IEnumerator LoadSceneAsynchronously(int sceneIndex)
    {
        Debug.Log("Loading...");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}