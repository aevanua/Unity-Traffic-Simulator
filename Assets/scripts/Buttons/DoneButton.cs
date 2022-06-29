using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneButton : MonoBehaviour
{
    public GameObject RoadCanvas;
    public bool flag = false;

    public void Deactivate()
    {
        RoadCanvas.SetActive(false);
        flag = true;
    }
    
}
