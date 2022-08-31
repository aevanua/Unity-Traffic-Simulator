using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDirRoad : MonoBehaviour
{
    public string ID;
    public int OneDirCars = 0;
    public int StartNode, FinishNode;
    public Vector3 StartNodeVec, FinishNodeVec;
    public List<Lane> LaneList = new List<Lane>();
    public TrafficLight TrafficLight;
    public GameObject obj;
    public GameObject SaveObj;
    Geometry Geometry = new Geometry();
    Lane Lane = new Lane();
    

    public OneDirRoad SpawnOneRoad(int StartNode, int FinishNode, int lanes_num, Vector3 StartNodeVec, Vector3 FinishNodeVec, GameObject parent, bool IsOneWay)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("OneDirRoad"), Vector3.zero, Quaternion.Euler(0, 0, 0), parent.transform);
        obj.GetComponent<OneDirRoad>().StartNode = StartNode;
        obj.GetComponent<OneDirRoad>().StartNodeVec = StartNodeVec;
        obj.GetComponent<OneDirRoad>().FinishNode = FinishNode;
        obj.GetComponent<OneDirRoad>().FinishNodeVec = FinishNodeVec;
        obj.GetComponent<OneDirRoad>().obj = obj;
        for (int i = 1; i <= lanes_num; i++)
        {
            float offset = 0;
            if (IsOneWay)
                offset = -0.25f * lanes_num / 2 + 0.25f * (i - 1) + 0.125f;
            else
                offset = -0.25f * lanes_num + 0.25f * (i - 1) + 0.125f;
            if (lanes_num == 1 && IsOneWay)
                offset = 0;
            obj.GetComponent<OneDirRoad>().LaneList.Add(Lane.SpawnLane(StartNode, FinishNode, Geometry.GetPerpendicularPointTemp(FinishNodeVec - StartNodeVec, StartNodeVec, offset), Geometry.GetPerpendicularPointTemp(FinishNodeVec - StartNodeVec, FinishNodeVec, offset), i, offset, parent));

        }
        return obj.GetComponent<OneDirRoad>();
    }

    public bool IfFilled()
    {
        for (int i = 0; i < LaneList.Count; i++)
            if (!LaneList[i].IfFilled())
                return false;
        return true;
    }

    public void Calculate_cars()
    {
        OneDirCars = 0;
        for (int i = 0; i < LaneList.Count; i++)
        {
            //Debug.Log(LaneList[i].Calc_Cars);
            OneDirCars += LaneList[i].Calc_Cars;
        }
    }

    void Update()
    {
        Calculate_cars();
    }
}
