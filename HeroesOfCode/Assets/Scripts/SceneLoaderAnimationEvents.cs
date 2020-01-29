using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderAnimationEvents : MonoBehaviour
{
    public void SetSceneLoaderReady()
    {
        FindObjectOfType<SceneLoader>().isReady = true;
    }
}
