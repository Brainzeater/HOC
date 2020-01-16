using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public List<int> goblins;
    public List<int> skeltons;
    public List<int> shootingBlobs;
    public List<int> knightBlobs;

    public Unit goblin;
    public Unit skeleton;
    public Unit shootingBlob;
    public Unit knightBlob;

    // and methods for them
    public int EnemyArmiesLeft { get; set; }
    private const int enemyArmies = 3;
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameData");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        EnemyArmiesLeft = enemyArmies;
        for (int i = 0; i < goblins.Count; i++)
        {
            goblins[i] *= goblin.hp;
        }
        for (int i = 0; i < skeltons.Count; i++)
        {
            skeltons[i] *= skeleton.hp;
        }
        for (int i = 0; i < shootingBlobs.Count; i++)
        {
            shootingBlobs[i] *= shootingBlob.hp;
        }
        for (int i = 0; i < knightBlobs.Count; i++)
        {
            knightBlobs[i] *= knightBlob.hp;
        }
    }
    
}
