using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class ObjectMenu : MonoBehaviour
{
    bool Flag_Road = true , Flag_Car = true , Flag_TL = true;
    public GameObject Menu_button;
    public GameObject parent_obj;
    public GameObject temp;
    Car temp_car;
    Road temp_edge;
    TrafficLight temp_TL;

    void Start()
    {
        temp = GameObject.Find("EventSystem");
    }

    Vector3 GetCoorByClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.forward, Vector3.zero);
        float rayDistance;
        groundPlane.Raycast(ray, out rayDistance);
        Vector3 point = ray.GetPoint(rayDistance);
        return point;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && temp.GetComponent<events>().pause_flag)
        {
            Vector3 pos = GetCoorByClick();
            Collider2D Hit = Physics2D.OverlapPoint(pos);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray , out hit , 1000f) && hit.collider.tag == "Car" && Flag_Car)
            {
                GameObject temp = GameObject.Find(hit.collider.name);
                temp_car = temp.GetComponent<Car>();
                Debug.Log(temp_car.Speed);
                GameObject Car_button = (GameObject)Instantiate(Menu_button);
                Car_button.transform.SetParent(parent_obj.transform, false);
                Car_button.transform.position = new Vector3(1700 , 400 , 0);
                Car_button.name = "Car_menu";
                Flag_Car = false;
            }
            else if (Physics.Raycast(ray , out hit , 1000f) && hit.collider.tag == "TL" && Flag_TL)
            {
                GameObject temp = GameObject.Find(hit.collider.name);
                temp_TL = temp.GetComponent<TrafficLight>();
                Debug.Log(temp_TL.flag);
                GameObject TL_button = (GameObject)Instantiate(Menu_button);
                TL_button.transform.SetParent(parent_obj.transform, false);
                TL_button.transform.position = new Vector3(1700 , 600 , 0);
                TL_button.name = "TL_menu";
                Flag_TL = false;
            }

            else if (Hit != null && Hit.tag == "Edge" && Flag_Road)
            {

                GameObject temp = GameObject.Find(Hit.name);
                temp_edge = temp.GetComponent<Road>();
                Debug.Log(temp_edge.Road_coefficient);
                GameObject Road_button = (GameObject)Instantiate(Menu_button);
                Road_button.transform.SetParent(parent_obj.transform, false);
                Road_button.transform.position = new Vector3(1700 , 200 , 0);
                Road_button.name = "Road_menu";
                Flag_Road = false;
            }

        }
        if (GameObject.Find("Road_menu") != null)
        {
            string type = "OneWay";
            if (!temp_edge.OneWay) type = "TwoWay";
            GameObject.Find("Road_menu").GetComponentInChildren<Text>().text = "Object name: " + temp_edge.name + '\n' + "Road type: " + type + '\n' + "Road coefficient = " + temp_edge.Road_coefficient + '\n' + "Amount of cars: " + temp_edge.Calc_Cars + '\n';
        }

        /*if (GameObject.Find("TL_menu") != null)
        {
            string color = "green";
            if (!temp_TL.flag) color = "red";
            GameObject.Find("TL_menu").GetComponentInChildren<Text>().text = "Object name: " + temp_TL.name + '\n' + "Current color: " + color + '\n' + "Period = " + temp_TL.period.ToString("0") + '\n';
        }*/

        if (GameObject.Find("Car_menu") != null)
            GameObject.Find("Car_menu").GetComponentInChildren<Text>().text = "Object name: " + temp_car.name + '\n' + "Speed = " + temp_car.Speed.ToString("F2") + '\n' + "Car lane = " + temp_car.CurLane.num_lane.ToString();

        if (Input.GetKeyDown("c") && GameObject.Find("Car_menu") != null)
        {
            GameObject object_to_del = GameObject.Find("Car_menu");
            Destroy(object_to_del);
            Flag_Car = true;
        }

        if (Input.GetKeyDown("t") && GameObject.Find("TL_menu") != null)
        {
            GameObject object_to_del = GameObject.Find("TL_menu");
            Destroy(object_to_del);
            Flag_TL = true;
        }

        if (Input.GetKeyDown("r") && GameObject.Find("Road_menu") != null)
        {
            GameObject object_to_del = GameObject.Find("Road_menu");
            Destroy(object_to_del);
            Flag_Road = true;
        }
    }
}
