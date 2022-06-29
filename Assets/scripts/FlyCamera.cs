using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float zspeed = 2, xyspeed = 0.5f;    
    Plane hPlane = new Plane(Vector3.forward, Vector3.zero);
    public float distance = 0,k,maximum;
    public Vector3 intersectionpoint,helpvec;
    Ray ray;
    Camera cum;
    GameObject temp;
    void GetCoors()
    {
        ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));        
        ray.GetPoint(distance);
        hPlane.Raycast(ray, out distance);
        
        k = ray.origin.z / ray.direction.z;        
        if (maximum==transform.position.z)
        {
            intersectionpoint = ray.origin;
            intersectionpoint.z = 0;
        }
        else 
            intersectionpoint = new Vector3(ray.origin.x + k * ray.direction.x, ray.origin.y + k * ray.direction.y, 0);        
    }
    void Start()
    {
        temp = GameObject.Find("EventSystem");
        maximum = transform.position.z;
        GetCoors();      
    }
    
    void FixedUpdate()
    {
        if(temp.GetComponent<events>().FPS)
        {
            if (Input.GetKey("up"))
            {
                transform.RotateAround(transform.position, Vector3.left, 1);
                //transform.LookAt(intersectionpoint);
            }
            if (Input.GetKey("down"))
            {
                transform.RotateAround(transform.position, Vector3.right, 1);
            }
            if (Input.GetKey("right"))
            {
                //transform.LookAt(intersectionpoint);                        
                transform.RotateAround(transform.position, Vector3.back, 1);
            }
            if (Input.GetKey("left"))
            {
                //transform.LookAt(intersectionpoint);            
                transform.RotateAround(transform.position, Vector3.forward, 1);
            }
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.z > -50)
            {
                transform.position += new Vector3(0, 0, -zspeed);
                maximum -= zspeed;
                GetCoors();
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.z < -2)
            {
                transform.position += new Vector3(0, 0, zspeed);
                maximum += zspeed;
                GetCoors();
            }
        }
        
        
        if (Input.GetKey("d"))
        {
            GetCoors();
            helpvec.x = ray.direction.y;
            helpvec.y = -ray.direction.x;
            helpvec.z = 0;
            if (maximum == transform.position.z)
                helpvec.x = 1;
            transform.position += helpvec;
        }
        if (Input.GetKey("a"))
        {
            
            GetCoors();
            helpvec.x = -ray.direction.y;
            helpvec.y = ray.direction.x;
            helpvec.z = 0;
            if (maximum == transform.position.z)
                helpvec.x = -1;
            transform.position += helpvec;
        }
        if (Input.GetKey("s"))
        {
            
            GetCoors();
            helpvec = ray.direction;
            helpvec.z = 0;
            if (maximum == transform.position.z)
                helpvec.y = 1;
            transform.position -= helpvec;
        }
        if (Input.GetKey("w"))
        {
            
            GetCoors();
            helpvec = ray.direction;
            helpvec.z = 0;
            if (maximum == transform.position.z)
                helpvec.y = 1;
            transform.position += helpvec;
        }
    }
}
