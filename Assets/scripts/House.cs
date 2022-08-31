using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public Vector3 pos;
    public int HNode;
    public Road road;
    public string RoadID;
    public bool IsRight;    
    public GameObject HouseObj;
    public string ID;
    public Quaternion Angle;
    void Start()
    {
        
    }   
    public string SaveData()
    {
        this.RoadID = road.ID;
        return JsonUtility.ToJson(this, false);
    }
    void Update()
    {
        
    }
}
