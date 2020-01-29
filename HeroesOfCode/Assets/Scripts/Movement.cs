using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Surface
{
    Snow,
    Road,
    Ice
}

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

    private Animator animator;
    private float animationSpeedModifier;

    private Surface surface;
    private AudioManager audioManager;

    void Awake()
    {
        surface = Surface.Snow;
        animator = GetComponentInChildren<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }

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
        Vector3 direction;
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= Path.Length)
                {
                    animator.SetFloat("HorizontalSpeed", 0f);
                    animator.SetFloat("VerticalSpeed", 0f);
                    GameEvents.current.MovementFinished();
                    yield break;
                }

                currentWaypoint = Path[targetIndex];
            }

//            print(currentWaypoint - transform.position);
            direction = (currentWaypoint - transform.position);
            animator.SetFloat("HorizontalSpeed", detectDirection(direction.x) * (1 - animationSpeedModifier));
            animator.SetFloat("VerticalSpeed", detectDirection(direction.y) * (1 - animationSpeedModifier));
            transform.position =
                Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

//            print(transform.position);
            //TODO: Destroy past cell
            yield return null;
        }
    }

    void Start()
    {
        speed = defaultSpeed;
        animationSpeedModifier = 0.39f;
    }

    void Update()
    {
        if (Physics2D.OverlapCircle(gameObject.transform.position, radius, roadMask))
        {
            speed = increasedSpeed;
            animationSpeedModifier = 0f;
            surface = Surface.Road;
        }
        else if (Physics2D.OverlapCircle(gameObject.transform.position, radius, iceMask))
        {
            speed = decreasedSpeed;
            animationSpeedModifier = 0.4f;
            surface = Surface.Ice;
        }
        else
        {
            speed = defaultSpeed;
            animationSpeedModifier = 0.3f;
            surface = Surface.Snow;
        }
    }

    float detectDirection(float number)
    {
        if (number > 0)
        {
            return 1;
        }
        else if (number < 0)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public void PlayRightLeg()
    {
        switch (surface)
        {
            case Surface.Snow:
                audioManager.Play("Snow1");
                break;
            case Surface.Ice:
                audioManager.Play("Ice1");
                break;
            case Surface.Road:
                audioManager.Play("Road1");
                break;
        }
    }

    public void PlayLeftLeg()
    {
        switch (surface)
        {
            case Surface.Snow:
                audioManager.Play("Snow2");
                break;
            case Surface.Ice:
                audioManager.Play("Ice2");
                break;
            case Surface.Road:
                audioManager.Play("Road2");
                break;
        }
    }
}