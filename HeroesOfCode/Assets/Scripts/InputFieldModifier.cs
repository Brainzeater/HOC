using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldModifier : MonoBehaviour
{
    public GameObject placehoder;
    public TMP_InputField text;

    public void Select()
    {
        text.text = placehoder.GetComponent<TextMeshProUGUI>().text;
        placehoder.SetActive(false);
    }
}