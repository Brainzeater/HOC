using UnityEngine;

// Highlights selected enemy squad and tells its ID
public class SelectTarget : MonoBehaviour
{
    private SpriteRenderer highlight;
    [HideInInspector] public bool Active { get; set; }
    [HideInInspector] public bool DamageAll { get; set; }
    private Color defaultColor;
    private Color damageAllColor;
    private BattleSystem battleSystem;

    void Awake()
    {
        highlight = GetComponent<SpriteRenderer>();
        Active = false;

        defaultColor = highlight.color;
        damageAllColor = Color.red;
        damageAllColor.a = .4f;
        BattleEvents.current.OnDamageAllHighlight += HighlightAll;
        BattleEvents.current.OnUnhighlight += Unhighlight;
        DamageAll = false;
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    void OnMouseEnter()
    {
        if (Active && !battleSystem.paused)
            if (DamageAll)
            {
                BattleEvents.current.HighlightAll();
            }
            else if (!highlight.enabled)
                highlight.enabled = true;
    }

    void OnMouseExit()
    {
        if (Active && !battleSystem.paused)
            if (DamageAll)
            {
                BattleEvents.current.Unhighlight();
            }
            else if (highlight.enabled)
                highlight.enabled = false;
    }

    void OnMouseOver()
    {
        if (Active && !battleSystem.paused)
            if (DamageAll)
            {
                BattleEvents.current.HighlightAll();
            }
            else if (!highlight.enabled)
                highlight.enabled = true;
    }

    void OnMouseDown()
    {
        if (Active && !battleSystem.paused)
        {
            highlight.enabled = false;
            if (!DamageAll)
            {
                BattleEvents.current.TargetSelected(gameObject.GetComponentInParent<Squad>().ID);
            }
            else
            {
                BattleEvents.current.DamageAllSquads();
            }
        }
    }

    public void DamageAllSetup()
    {
        highlight.color = damageAllColor;
        Active = true;
        DamageAll = true;
    }

    public void FinishDamageAll()
    {
        highlight.enabled = false;
        highlight.color = defaultColor;
        Active = false;
        DamageAll = false;
    }

    void HighlightAll()
    {
        if (!battleSystem.paused)
        {
            // Highlight only alive squads
            if (!GetComponentInParent<Squad>().IsDead)
            {
                if (!highlight.enabled)
                {
                    highlight.enabled = true;
                }
            }
        }
    }

    void Unhighlight()
    {
        if (highlight.enabled)
        {
            highlight.enabled = false;
        }
    }
}