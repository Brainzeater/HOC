using UnityEngine;

// Highlights selected enemy squad and tells its ID
public class SelectTarget : MonoBehaviour
{
    private SpriteRenderer highlight;
    [HideInInspector] public bool Active { get; set; }

    void Awake()
    {
        highlight = GetComponent<SpriteRenderer>();
        Active = false;
    }

    void OnMouseEnter()
    {
        if (Active)
            if (!highlight.enabled)
                highlight.enabled = true;
    }

    void OnMouseExit()
    {
        if (Active)
            if (highlight.enabled)
                highlight.enabled = false;
    }

    void OnMouseDown()
    {
        if (Active)
            BattleEvents.current.TargetSelected(gameObject.GetComponentInParent<Squad>().ID);
    }
}