using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
//    public Transform target;
    public float speed;

//    Vector3[] path;
    int targetIndex;

    public Vector3[] Path { set; get; }

    void Start()
    {
    }

    public void BeginMovement(Vector3[] path)
    {
        Path = path;
        targetIndex = 0;
        foreach (Vector3 vector3 in Path)
        {
            print(vector3);
        }
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

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
}