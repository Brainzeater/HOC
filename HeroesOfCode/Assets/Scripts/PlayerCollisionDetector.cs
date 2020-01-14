using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        SceneLoader.LoadBattleScene();
    }
}
