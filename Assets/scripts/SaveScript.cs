using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class SaveScript : MonoBehaviour
{
    GameObject temp;    
    public void CreateText()
    {
        string path = Application.dataPath + "/Saves/";
        string name = GameObject.Find("Main Camera").GetComponent<FileNameInput>().input;
        Debug.Log(name);
        path = path + name + ".gra";
        File.WriteAllText(path, "");
        File.AppendAllText(path, temp.GetComponent<create>().nodes.Count.ToString()+"\n");
        for (int i = 0; i<temp.GetComponent<create>().nodes.Count;i++)        
            File.AppendAllText(path, temp.GetComponent<create>().nodes[i].SaveData()+"\n");
        File.AppendAllText(path, temp.GetComponent<create>().ListOfRoads.Count.ToString() + "\n");
        for (int i = 0; i < temp.GetComponent<create>().ListOfRoads.Count; i++)        
            File.AppendAllText(path, temp.GetComponent<create>().ListOfRoads[i].SaveData() + "\n");
        File.AppendAllText(path, temp.GetComponent<create>().ListofHouses.Count.ToString() + "\n");
        for (int i = 0; i < temp.GetComponent<create>().ListofHouses.Count; i++)
            File.AppendAllText(path, temp.GetComponent<create>().ListofHouses[i].SaveData() + "\n");
        File.AppendAllText(path, temp.GetComponent<create>().TrafficLightList.Count.ToString() + "\n");
        for (int i = 0; i < temp.GetComponent<create>().TrafficLightList.Count; i++)
            File.AppendAllText(path, temp.GetComponent<create>().TrafficLightList[i].SaveData() + "\n");
        File.AppendAllText(path, temp.GetComponent<create>().NumberIfInstance.ToString() + "\n");
    }
       
    void Start()
    {        
        temp = GameObject.Find("path");
    }
    void Update()
    {
        if (temp == null)
            temp = GameObject.Find("path");
    }
}
