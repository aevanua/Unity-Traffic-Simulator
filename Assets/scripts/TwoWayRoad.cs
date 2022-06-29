using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayRoad : MonoBehaviour
{
    int StartNode, FinishNode;
    public List<OneDirRoad> OneDirList = new List<OneDirRoad>();
    Vector3 StartNodeVec, FinishNodeVec;
    Geometry Geometry = new Geometry();
    OneDirRoad OneDirRoad = new OneDirRoad();

    public TwoWayRoad SpawnTwoWayRoad(int StartNode, int FinishNode, Vector3 StartNodeVec, Vector3 FinishNodeVec , int LanesNum , Transform parent)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("TwoWayRoad"), Vector3.zero, Quaternion.Euler(0, 0, 0) , parent);
        obj.GetComponent<TwoWayRoad>().StartNode = StartNode;
        obj.GetComponent<TwoWayRoad>().StartNodeVec = StartNodeVec;
        obj.GetComponent<TwoWayRoad>().FinishNode = FinishNode;
        obj.GetComponent<TwoWayRoad>().FinishNodeVec = FinishNodeVec;
        obj.GetComponent<TwoWayRoad>().OneDirList.Add(OneDirRoad.SpawnOneRoad(StartNode, FinishNode, LanesNum ,StartNodeVec, FinishNodeVec,obj , false));
        obj.GetComponent<TwoWayRoad>().OneDirList.Add(OneDirRoad.SpawnOneRoad(FinishNode , StartNode, LanesNum , FinishNodeVec, StartNodeVec , obj , false));
        return obj.GetComponent<TwoWayRoad>();
    }
}
