using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSquadButton : MonoBehaviour
{
    public Image highlight;
    [HideInInspector]
    public int ID { get; set; }

    void Awake()
    {
        ID = -1;
    }
    public void Highlight()
    {
        if (!highlight.enabled)
            highlight.enabled = true;
    }

    public void Unhighlight()
    {
        if (highlight.enabled)
            highlight.enabled = false;
    }

    public void DeleteSquad()
    {
        FindObjectOfType<ArmyManager>().RemoveSquad(ID);
    }

    public void Response()
    {
        FindObjectOfType<AudioManager>().Play("Click");
    }
}
