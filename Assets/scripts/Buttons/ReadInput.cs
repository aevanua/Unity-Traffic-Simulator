using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadInput : MonoBehaviour
{
    public InputField inputText;
    public string input = "2";
    void Start()
    {
        input = "2";
    }
    public void ReadStringInput()
    {
        input = inputText.text;
        Debug.Log(input);
    }
}
