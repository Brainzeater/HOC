using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public void PlayRightLeg()
    {
        FindObjectOfType<Movement>().PlayRightLeg();
    }

    public void PlayLeftLeg()
    {
        FindObjectOfType<Movement>().PlayLeftLeg();
    }
}
