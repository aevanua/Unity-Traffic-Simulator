using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public int first;
    public int second;
    public GameObject obj;
    public OneDirRoad OneDir;
    public bool flag = true;//0 red 1 green
    public float red_period , green_period , curr_period , time;
    

    List <Car> cars = new List<Car>();
    List<Node> nodes = new List<Node>();
    GameObject temp , temp2;

    public void Init(int first, int second, GameObject obj, int red_period, int green_period , int time, OneDirRoad OneDir)
    {
        this.first = first; this.second = second; this.obj = obj; this.red_period = red_period; this.green_period = green_period; this.time = time; this.OneDir = OneDir;
    }

    public float GetTimeToPass()
    {
        if (flag) return 0;
        else return (curr_period - time);
    }

    /*public void CountCurrentCars()
    {
        CurCars = 0;
        for(int i = 0;i < cars.Count;i++)
        {
            if (nodes[first] + cars[i].offset == cars[i].start && nodes[second] + cars[i].offset == cars[i].Aim)
                CurCars++;
        }
    }
    */
    void Start()
    {
        //time = 0;
        curr_period = green_period;
        obj.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        temp = GameObject.Find("path");
        temp2 = GameObject.Find("EventSystem");
    }
    // Update is called once per frame
    void Update()
    {
        nodes = temp.GetComponent<create>().nodes;
        cars = temp.GetComponent<create>().cars;
        time += Time.deltaTime;
        if (!temp2.GetComponent<events>().pause_flag && time > curr_period)
        {
            if (flag == false)
            {
                obj.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else obj.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            flag = !flag;
            time -= curr_period;
            if (curr_period == green_period)
                curr_period = red_period;
            else curr_period = green_period;
        }
        //CountCurrentCars();
    }
}
