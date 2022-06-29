using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float zspeed=2,xyspeed=0.5f;

    void Update () 
    {
        if (Input.GetAxis ("Mouse ScrollWheel") < 0 && transform.position.z>-50) 
            transform.position += new Vector3(0, 0, -zspeed);
        if (Input.GetAxis ("Mouse ScrollWheel") > 0 && transform.position.z < -2) 
            transform.position += new Vector3(0, 0, zspeed);
        if (Input.GetKey("d"))
            transform.position += new Vector3(xyspeed, 0, 0);
        if (Input.GetKey("a"))
            transform.position += new Vector3(-xyspeed, 0, 0);
        if (Input.GetKey("s"))
            transform.position += new Vector3(0,-xyspeed, 0);
        if (Input.GetKey("w"))
            transform.position += new Vector3(0,xyspeed, 0);
    }
}
