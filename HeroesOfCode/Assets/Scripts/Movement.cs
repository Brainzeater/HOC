using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script that makes the player object move along the path.
public class Movement : MonoBehaviour
{
    public float defaultSpeed;
    public float increasedSpeed;
    public float decreasedSpeed;

    private float speed;

    int targetIndex;

    public LayerMask roadMask;
    public LayerMask iceMask;

    public Vector3[] Path { set; get; }

    private const float radius = 0.01f;

    // Starts a coroutine that moves the player object.
    public void BeginMovement(Vector3[] path)
    {
        Path = path;
        targetIndex = 0;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

    // A coroutine that makes the player object change its position
    // according to a list of nodes in the path. 
    // The player object moves between the nodes one by one and
    // generates an event when it's done.
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = Path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= Path.Length)
                {
                    GameEvents.current.MovementFinished();
                    yield break;
                }

                currentWaypoint = Path[targetIndex];
            }

            transform.position =
                Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            //TODO: Destroy past cell
            yield return null;
        }
    }

    void Start()
    {
        speed = defaultSpeed;
    }

    void Update()
    {
        if (Physics2D.OverlapCircle(gameObject.transform.position, radius, roadMask))
        {
            speed = increasedSpeed;
        }
        else if(Physics2D.OverlapCircle(gameObject.transform.position, radius, iceMask))
        {
            speed = decreasedSpeed;
        }
        else
        {
            speed = defaultSpeed;
        }
    }
}