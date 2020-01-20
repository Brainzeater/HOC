using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseActiveSkillButton : MonoBehaviour
{
    private SpriteRenderer highlight;
    public bool activateRegularHit;
    void Awake()
    {
        highlight = GetComponent<SpriteRenderer>();
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
        if(!activateRegularHit)
        {
            BattleEvents.current.ActiveSkill();
        }
        else
        {
            BattleEvents.current.RegularHit();
        }
    }
}