using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseActiveSkillButton : MonoBehaviour
{
    private SpriteRenderer highlight;
    public bool activateRegularHit;

    private Color highlightColor;
    private Color disabledColor;
    private bool pressed;

    void Awake()
    {
        highlight = GetComponent<SpriteRenderer>();
        highlightColor = Color.white;
        highlightColor.a = .3f;
        disabledColor = Color.black;
        disabledColor.a = .3f;
        pressed = false;
    }

    void OnMouseEnter()
    {
//        if (!highlight.enabled && !pressed)
        if (!pressed)
            highlight.color = highlightColor;
    }

    void OnMouseExit()
    {
//        if (highlight.enabled && !pressed)
        if (!pressed)
            highlight.color = disabledColor;
    }

    void OnMouseDown()
    {
        if (!activateRegularHit)
        {
            BattleEvents.current.ActiveSkill();
        }
        else
        {
            BattleEvents.current.RegularHit();
        }
    }

    public void SetPressed(bool enable)
    {
        if (enable)
        {
            pressed = true;
//            highlight.color = disabledColor;
            highlight.enabled = false;
        }
        else
        {
            pressed = false;
            highlight.color = disabledColor;
            highlight.enabled = true;
        }
    }
}