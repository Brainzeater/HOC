using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTarget : MonoBehaviour
{
    private SpriteRenderer highlight;
    [HideInInspector] public bool Selected { get; set; }

    void Start()
    {
        highlight = GetComponent<SpriteRenderer>();
        Selected = false;
    }

    void OnMouseEnter()
    {
        if (!highlight.enabled)
            highlight.enabled = true;
    }

    void OnMouseExit()
    {
        if (highlight.enabled)
            highlight.enabled = false;
    }

    void OnMouseDown()
    {
        if (!Selected)
        {
            Selected = false;

            BattleEvents.current.TargetSelected(1);
        }
    }
}