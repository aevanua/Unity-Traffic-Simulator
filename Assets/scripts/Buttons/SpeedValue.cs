using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedValue : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject temp;
    public Text Button_Text;
    void Start()
    {
        temp = GameObject.Find("EventSystem");
    }

    void Update()
    {
        Button_Text.text = temp.GetComponent<events>().worldspeed.ToString("F1");
    }
    
}
