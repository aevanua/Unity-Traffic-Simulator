using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 pos;
    public int number;    
    public string ID;
    public House HouseLink;
    public GameObject NodeObj;
    void Start()
    {
        
    }
    /*public void LoadData(string data)
    {
        GameObject tmpobj = Instantiate(node, Vector3.zero(), Quaternion.identity, parent_nodes);
        JsonUtility.FromJsonOverwrite(data, tmpobj.GetComponent<Node>());
        tmpobj.GetComponent<Node>().NodeObj = tmpobj;
        tmpobj.transform.position = tmpobj.GetComponent<Node>().pos;
    }
    tmpobj.name = "node" + nodes.Count;
        nodes.Add(tmpobj.GetComponent<Node>());
        way.Add(0f);
        NodeWay.Add(0);
        AdjacencyList.Add(new List<(int, Road)>());
        return tmpobj;
    }*/
public string SaveData()
    {
        return JsonUtility.ToJson(this,false);
    }
    void Update()
    {
        
    }
}
