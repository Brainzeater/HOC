using UnityEngine;

// Highlights selected enemy squad and tells its ID
public class SelectTarget : MonoBehaviour
{
    private SpriteRenderer highlight;
    [HideInInspector] public bool Active { get; set; }
    [HideInInspector] public bool DamageAll { get; set; }
    private Color defaultColor;
    private Color damageAllColor;

    void Awake()
    {
        highlight = GetComponent<SpriteRenderer>();
        Active = false;

        defaultColor = highlight.color;
        damageAllColor = Color.red;
        damageAllColor.a = .4f;
        BattleEvents.current.OnDamageAllHighlight += HighlightAll;
        DamageAll = false;
    }

    void OnMouseEnter()
    {
        if (Active)
            if (DamageAll)
            {
                BattleEvents.current.HighlightAll();
            }
            else if (!highlight.enabled)
                highlight.enabled = true;
    }

    void OnMouseExit()
    {
        if (Active)
            if (DamageAll)
            {
                BattleEvents.current.HighlightAll();
            }
            else if (highlight.enabled)
                highlight.enabled = false;
    }

    void OnMouseOver()
    {
        if (Active)
            if (!highlight.enabled)
                highlight.enabled = true;
    }

    void OnMouseDown()
    {
        if (Active)
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
        // Highlight only alive squads
        if(!GetComponentInParent<Squad>().IsDead)
        {
            highlight.enabled = !highlight.enabled;
        }
    }
}