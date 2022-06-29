using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonText : MonoBehaviour
{
    public bool flag = true;
    public Text Button_Text;
    public void Set_Text()
    {
        if (flag)
        {
            Button_Text.text = "Play";
            flag = false;
        }
        else
        {
            Button_Text.text = "Pause";
            flag = true;
        }
        
    }
}