using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script that makes the player object move along the path.
public class Movement : MonoBehaviour
{
    public float speed;

    int targetIndex;

    public Vector3[] Path { set; get; }

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

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            //TODO: Destroy past cell
            yield return null;
        }
    }

    public void SetSpeed()
    {
        // TODO: Put colliders on the map which will trigger an event
        // TODO: causing this method to increase or decrease current movement speed.
    }
}