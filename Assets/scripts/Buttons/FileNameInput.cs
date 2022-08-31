using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileNameInput : MonoBehaviour
{
    public InputField inputText;
    public string input = "Save";
    public GameObject Save2;
    void Start()
    {
        input = "Save";
    }
    public void ReadStringInput()
    {
        input = inputText.text;
        Save2.SetActive(false);
    }
}
