using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstState : MonoBehaviour
{
    public GameObject First_Canvas , Second_Canvas;
    void Start()
    {
        Second_Canvas.SetActive(false);
    }

    public void change_states()
    {
        First_Canvas.SetActive(false);
        Second_Canvas.SetActive(true);
    }
    
}