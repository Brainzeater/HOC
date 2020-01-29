using System.Collections;
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
    
    [HideInInspector] public bool isReady;

    void Start()
    {
        transition.SetFloat("FadeInMultiplier", fadeInSpeed);
        transition.SetFloat("FadeOutMultiplier", fadeOutSpeed);
        audioManager = FindObjectOfType<AudioManager>();
        StartCoroutine(loadCurrentScene());
        isReady = false;

    }

    IEnumerator loadCurrentScene()
    {
        yield return new WaitForSeconds(waitTime);
        transition.enabled = true;
        if (audioManager.currentBackgroundMusic != null)
            audioManager.FinishAudioSource();

        audioManager.SetBackgroundMusicAndPlayIt(backgroundMusicName);
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
        isReady = false;
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

    public void Quit()
    {
        Application.Quit();
    }
}