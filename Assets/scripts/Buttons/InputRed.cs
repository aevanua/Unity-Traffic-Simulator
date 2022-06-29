using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputRed : MonoBehaviour
{
    public InputField inputText;
    public string input = "10";

    void Start()
    {
        input = "10";
    }
    public void ReadStringInput()
    {
        input = inputText.text;
        Debug.Log(input);
    }
}
