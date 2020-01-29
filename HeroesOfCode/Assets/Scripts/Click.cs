using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    public void Response()
    {
        FindObjectOfType<AudioManager>().Play("Click");
    }
}
