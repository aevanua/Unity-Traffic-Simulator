using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondState : MonoBehaviour
{
    public GameObject First_Canvas , Second_Canvas;
    void Start()
    {
    
    }

    public void change_states()
    {
        First_Canvas.SetActive(true);
        Second_Canvas.SetActive(false);
    }
}
