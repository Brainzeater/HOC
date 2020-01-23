using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHighlighter : MonoBehaviour
{
    private SpriteRenderer highlight;

    void Awake()
    {
        highlight = GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        print("Hello");
        if (!highlight.enabled)
            highlight.enabled = true;
    }

    void OnMouseExit()
    {
        print("Bye");
        if (highlight.enabled)
            highlight.enabled = false;
    }

    void OnMouseDown()
    {
        FindObjectOfType<SceneLoader>().LoadStartScene();
    }
}