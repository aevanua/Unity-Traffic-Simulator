using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public string ID;
    public Vector3 pos;
    public Quaternion Angle;
    public int first;
    public int second;
    public int StartRoad;
    public int FinishRoad;
    public GameObject obj;    
    public OneDirRoad OneDir;
    public Road Road;
    public string RoadID;
    public bool flag = true;//0 red 1 green
    public float red_period , green_period , curr_period , time;
    

    List <Car> cars = new List<Car>();
    List<Node> nodes = new List<Node>();
    GameObject temp , temp2;

    public void Init(int first, int second, GameObject obj, int red_period, int green_period , int time, OneDirRoad OneDir, Road Road)
    {
        this.first = first; this.second = second; this.obj = obj; this.red_period = red_period; this.green_period = green_period; this.time = time; this.OneDir = OneDir;this.Road = Road;
        this.StartRoad = Road.FirstNode; this.FinishRoad =Road.SecondNode;
    }

    public float GetTimeToPass()
    {
        if (flag) return 0;
        else return (curr_period - time);
    }

    void Start()
    {
    
        curr_period = green_period;
        obj.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        temp = GameObject.Find("path");
        temp2 = GameObject.Find("EventSystem");
    }
    void Update()
    {
        if (temp == null)
            temp = GameObject.Find("path");
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
    public string SaveData()
    {
        this.RoadID = Road.ID;
        return JsonUtility.ToJson(this, false);
    }
    
}
