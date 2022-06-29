using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public int Calc_Cars;
    public int StartNode, FinishNode;
    public int CurCarStop = 0;
    public Vector3 StartNodeVec, FinishNodeVec;
    public GameObject LaneObj;
    Geometry Geometry = new Geometry();
    List<Car> cars = new List<Car>();
    GameObject temp, temp2;
    public int num_lane;
    public float Offset;
    public Quaternion rotation;

    public Lane SpawnLane(int StartNode, int FinishNode, Vector3 StartNodeVec, Vector3 FinishNodeVec, int number, float offset, GameObject parent)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Lane"), Vector3.zero, Geometry.CalculateRotation(StartNodeVec, FinishNodeVec, 0f), parent.transform);
        obj.GetComponent<Lane>().StartNode = StartNode;
        obj.GetComponent<Lane>().FinishNode = FinishNode;
        obj.GetComponent<Lane>().StartNodeVec = StartNodeVec;
        obj.GetComponent<Lane>().FinishNodeVec = FinishNodeVec;
        obj.GetComponent<Lane>().num_lane = number;
        obj.GetComponent<Lane>().rotation = Geometry.CalculateRotation(this.StartNodeVec, this.FinishNodeVec, 0f);
        obj.GetComponent<Lane>().Offset = offset;
        obj.name = "Lane" + number;
        return obj.GetComponent<Lane>();
    }

    public void Calculate_cars()
    {
        Calc_Cars = 0;
        for (int i = 0; i < cars.Count; i++)
        {
            if (Geometry.IsOnLane(StartNodeVec, FinishNodeVec, cars[i].obj.transform.position) && Vector3.Distance(Geometry.GetPerpendicularIntersection(StartNodeVec, FinishNodeVec, cars[i].obj.transform.position), cars[i].obj.transform.position) <= 0.1f)
                Calc_Cars++;
        }
    }

    public void Set_color()
    {
        //GetComponent<Renderer>().material.color = new Color(Calc_Cars / Vector3.Distance(StartNode , FinishNode), 1, 0);
    }

    public void Remove_color()
    {
        //GetComponent<Renderer>().material.color = new Color(120f / 255f, 120f / 255f, 120f / 255f);
    }

    void Start()
    {
        temp = GameObject.Find("path");
        temp2 = GameObject.Find("EventSystem");
    }

    public bool IfFilled()
    {
        return (CurCarStop * 0.4f + 1f >= Vector3.Distance(StartNodeVec, FinishNodeVec));
    }

    void Update()
    {
        cars = temp.GetComponent<create>().cars;
        Calculate_cars();
    }
}
