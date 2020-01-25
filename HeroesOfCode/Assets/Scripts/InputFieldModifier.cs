using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldModifier : MonoBehaviour
{
    public GameObject placehoder;
    public TMP_InputField text;
    public Unit unit;

    void Awake()
    {
        placehoder.GetComponent<TextMeshProUGUI>().text = unit.defaultNumber.ToString();
    }

    public void Select()
    {
        text.text = placehoder.GetComponent<TextMeshProUGUI>().text;
        placehoder.SetActive(false);
    }

    public void Validate()
    {
        if (string.IsNullOrEmpty(text.text) || int.Parse(text.text) <= 0)
        {
            text.text = placehoder.GetComponent<TextMeshProUGUI>().text;
        }
    }
}