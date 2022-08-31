using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public bool OneWay;
    public int FirstNode, SecondNode;
    public OneWayRoad RightRoad = new OneWayRoad();
    public OneWayRoad LeftRoad = new OneWayRoad();
    public OneWayRoad RightRoadTraffic = new OneWayRoad();
    public OneWayRoad LeftRoadTraffic = new OneWayRoad();
    public float Len;
    public float Width;
    public GameObject obj;
    public int Calc_Cars = 0;
    public float Road_coefficient;
    public Vector3 StartNodeVec, FinishNodeVec;   
    public int LanesNum;
    public string ID;
    GameObject temp, temp2;
    Geometry Geometry = new Geometry();
    public Road SpawnRoad(int LanesNum, int StartNode, int FinishNode, Vector3 StartNodeVec, Vector3 FinishNodeVec, bool OneWay, float Road_coefficient, Transform parent)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Road"), Vector3.zero, Quaternion.Euler(0, 0, 0), parent);
        obj.GetComponent<Road>().obj = obj;
        obj.GetComponent<Road>().StartNodeVec = StartNodeVec; obj.GetComponent<Road>().FinishNodeVec = FinishNodeVec; obj.GetComponent<Road>().LanesNum = LanesNum; obj.GetComponent<Road>().Road_coefficient = Road_coefficient; obj.GetComponent<Road>().FirstNode = StartNode; obj.GetComponent<Road>().SecondNode = FinishNode; obj.GetComponent<Road>().OneWay = OneWay;
        obj.GetComponent<Road>().Len = Vector3.Distance(StartNodeVec, FinishNodeVec);

        if (OneWay)
        {
            obj.GetComponent<Road>().RightRoad = RightRoad.SpawnOneWayRoad(StartNode, FinishNode, StartNodeVec, FinishNodeVec, LanesNum, obj, Road_coefficient, true , false);
            obj.transform.GetChild(0).name = "RightRoad";
            obj.GetComponent<Road>().RightRoadTraffic = RightRoad.SpawnOneWayRoad(StartNode, FinishNode, StartNodeVec, FinishNodeVec, LanesNum, obj, Road_coefficient, true , true);
            obj.transform.GetChild(1).name = "RightRoadTraffic";
            obj.GetComponent<Road>().LeftRoad = null;
        }
        else
        {
            obj.GetComponent<Road>().RightRoad = RightRoad.SpawnOneWayRoad(StartNode, FinishNode, StartNodeVec, FinishNodeVec, LanesNum, obj, Road_coefficient, false , false);
            obj.transform.GetChild(0).name = "RightRoad";
            obj.GetComponent<Road>().LeftRoad = LeftRoad.SpawnOneWayRoad(FinishNode, StartNode, FinishNodeVec, StartNodeVec, LanesNum, obj, Road_coefficient, false , false);
            obj.transform.GetChild(1).name = "LeftRoad";
            obj.GetComponent<Road>().RightRoadTraffic = RightRoad.SpawnOneWayRoad(StartNode, FinishNode, StartNodeVec, FinishNodeVec, LanesNum, obj, Road_coefficient, false , true);
            obj.transform.GetChild(2).name = "RightRoadTraffic";
            obj.GetComponent<Road>().LeftRoadTraffic = LeftRoad.SpawnOneWayRoad(FinishNode, StartNode, FinishNodeVec, StartNodeVec, LanesNum, obj, Road_coefficient, false , true);
            obj.transform.GetChild(3).name = "LeftRoadTraffic";
        }
        return obj.GetComponent<Road>();
    }
    public Road SpawnRoad(string data, Transform parent)
    {

        GameObject obj = Instantiate(Resources.Load<GameObject>("Road"), Vector3.zero, Quaternion.Euler(0, 0, 0), parent);
        JsonUtility.FromJsonOverwrite(data, obj.GetComponent<Road>());
        obj.GetComponent<Road>().obj = obj;

        if (obj.GetComponent<Road>().OneWay)
        {
            obj.GetComponent<Road>().RightRoad = RightRoad.SpawnOneWayRoad(obj.GetComponent<Road>().FirstNode, obj.GetComponent<Road>().SecondNode, obj.GetComponent<Road>().StartNodeVec, obj.GetComponent<Road>().FinishNodeVec, obj.GetComponent<Road>().LanesNum, obj, obj.GetComponent<Road>().Road_coefficient, true, false);
            obj.transform.GetChild(0).name = "RightRoad";
            obj.GetComponent<Road>().RightRoadTraffic = RightRoad.SpawnOneWayRoad(obj.GetComponent<Road>().FirstNode, obj.GetComponent<Road>().SecondNode, obj.GetComponent<Road>().StartNodeVec, obj.GetComponent<Road>().FinishNodeVec, obj.GetComponent<Road>().LanesNum, obj, obj.GetComponent<Road>().Road_coefficient, true, true);
            obj.transform.GetChild(1).name = "RightRoadTraffic";
            obj.GetComponent<Road>().LeftRoad = null;
        }
        else
        {
            obj.GetComponent<Road>().RightRoad = RightRoad.SpawnOneWayRoad(obj.GetComponent<Road>().FirstNode, obj.GetComponent<Road>().SecondNode, obj.GetComponent<Road>().StartNodeVec, obj.GetComponent<Road>().FinishNodeVec, obj.GetComponent<Road>().LanesNum, obj, obj.GetComponent<Road>().Road_coefficient, false, false);
            obj.transform.GetChild(0).name = "RightRoad";
            obj.GetComponent<Road>().LeftRoad = LeftRoad.SpawnOneWayRoad(obj.GetComponent<Road>().SecondNode, obj.GetComponent<Road>().FirstNode, obj.GetComponent<Road>().FinishNodeVec, obj.GetComponent<Road>().StartNodeVec, obj.GetComponent<Road>().LanesNum, obj, obj.GetComponent<Road>().Road_coefficient, false, false);
            obj.transform.GetChild(1).name = "LeftRoad";            
            obj.GetComponent<Road>().RightRoadTraffic = RightRoad.SpawnOneWayRoad(obj.GetComponent<Road>().FirstNode, obj.GetComponent<Road>().SecondNode, obj.GetComponent<Road>().StartNodeVec, obj.GetComponent<Road>().FinishNodeVec, obj.GetComponent<Road>().LanesNum, obj, obj.GetComponent<Road>().Road_coefficient, false, true);
            obj.transform.GetChild(2).name = "RightRoadTraffic";
            obj.GetComponent<Road>().LeftRoad = LeftRoad.SpawnOneWayRoad(obj.GetComponent<Road>().SecondNode, obj.GetComponent<Road>().FirstNode, obj.GetComponent<Road>().FinishNodeVec, obj.GetComponent<Road>().StartNodeVec, obj.GetComponent<Road>().LanesNum, obj, obj.GetComponent<Road>().Road_coefficient, false, true);
            obj.transform.GetChild(3).name = "LeftRoadTraffic";
        }
        return obj.GetComponent<Road>();
    }
    void Start()
    {
        temp = GameObject.Find("EventSystem");
    }
    void Update()
    {
        if (temp.GetComponent<events>().FlagTrafficColor)
        {
            if (OneWay)
            {
                obj.transform.GetChild(0).gameObject.SetActive(false);
                obj.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                obj.transform.GetChild(0).gameObject.SetActive(false);
                obj.transform.GetChild(1).gameObject.SetActive(false);
                obj.transform.GetChild(2).gameObject.SetActive(true);
                obj.transform.GetChild(3).gameObject.SetActive(true);
            }
            //Set_color();
        }
        else
        {
            if (OneWay)
            {
                obj.transform.GetChild(0).gameObject.SetActive(true);
                obj.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                obj.transform.GetChild(0).gameObject.SetActive(true);
                obj.transform.GetChild(1).gameObject.SetActive(true);
                obj.transform.GetChild(2).gameObject.SetActive(false);
                obj.transform.GetChild(3).gameObject.SetActive(false);
            }
        }
    }
    public string SaveData()
    {
        return JsonUtility.ToJson(this, false);
    }



}

