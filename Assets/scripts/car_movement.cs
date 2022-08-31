using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car_movement : MonoBehaviour
{
    public Transform parent_nodes;
    GameObject temp, temp2;
    List<Node> nodes = new List<Node>();
    List<Transform> edges = new List<Transform>();
    List<Car> cars = new List<Car>();
    List<float> CarStopTime = new List<float>();
    List<List<(int, Road)>> AdjacencyList = new List<List<(int, Road)>>();
    List<TrafficLight> TrafficLightList = new List<TrafficLight>();
    List<float> way = new List<float>();
    public const int Nmax = 100000;
    public const int MaxW = 200900099;
    bool ishouse = false;
    Queue<int> VList = new Queue<int>();
    public List<int> NodeWay = new List<int>();
    List<House> ListofHouses = new List<House>();
    int nodes_first;
    House first, second;
    Geometry Geometry = new Geometry();

    int SearchRandomPoint(int start)
    {
        for (int x = 0; x < nodes.Count; x++)
            NodeWay[x] = -1;
        NodeWay[start] = 0;
        VList.Enqueue(start);
        while (VList.Count != 0)
        {
            start = VList.Dequeue();
            for (int i = 0; i < AdjacencyList[start].Count; i++)
            {
                if (NodeWay[AdjacencyList[start][i].Item1] == -1)
                {
                    NodeWay[AdjacencyList[start][i].Item1] = NodeWay[start] + 1;
                    VList.Enqueue(AdjacencyList[start][i].Item1);
                }
            }
        }
        List<int> RandomList = new List<int>();
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].HouseLink != null && NodeWay[i] != -1)            
                RandomList.Add(i);         
        }
        
        return RandomList[Random.Range(0, RandomList.Count)];
    }

    float length(Vector3 node, Vector3 MousePos)
    {
        return Mathf.Sqrt(Mathf.Pow(node.x - MousePos.x, 2) + Mathf.Pow(node.y - MousePos.y, 2));
    }

    public Vector3 RotateRound(Vector3 position, Vector3 center, float angle)
    {
        Vector3 axis = new Vector3(0, 0, 1);
        Vector3 point = Quaternion.AngleAxis(angle, axis) * position;
        Vector3 resultVec3 = center - point;
        return resultVec3;
    }

    public int CatchNode(Vector3 pos)
    {
        for (int i = 0; i < nodes.Count; i++)
            if (length(nodes[i].pos, pos) <= 0.5f)
                return i;
        return -1;
    }

    void Start()
    {
        temp = GameObject.Find("path");
        temp2 = GameObject.Find("EventSystem");
    }

    void Update()
    {
        if(temp==null)
            temp = GameObject.Find("path");
        nodes = temp.GetComponent<create>().nodes;
        ListofHouses = temp.GetComponent<create>().ListofHouses;
        way = temp.GetComponent<create>().way;
        NodeWay = temp.GetComponent<create>().NodeWay;
        cars = temp.GetComponent<create>().cars;
        AdjacencyList = temp.GetComponent<create>().AdjacencyList;
        TrafficLightList = temp.GetComponent<create>().TrafficLightList;
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].Finished())
            {
                if (cars[i].WayList.Count == 0)
                {
                    cars[i].VList.Clear();
                    cars[i].WayList.Clear();
                    int finish = SearchRandomPoint(cars[i].second_node);
                    cars[i].deixtra(cars[i].second_node, finish, cars[i].SpeedLimit * temp2.GetComponent<events>().worldspeed);
                    cars[i].ConvertDeixtra(cars[i].second_node, finish);
                    cars[i].FirstNodeWay = cars[i].second_node;
                    cars[i].SecondNodeWay = finish;
                }
                if (cars[i].WayList.Count != 0)
                {
                    OneWayRoad tmpOneWay = new OneWayRoad();
                    OneDirRoad tmp = cars[i].WayList.Pop();                    
                    tmpOneWay = tmp.obj.transform.parent.GetComponent<OneWayRoad>();
                    
                    cars[i].first_node = tmp.StartNode;
                    cars[i].second_node = tmp.FinishNode;
                    cars[i].start = tmp.StartNodeVec;
                    cars[i].Current = tmp.StartNodeVec;
                    cars[i].Aim = tmp.FinishNodeVec;                                        
                    Lane tmpLane = tmp.LaneList[cars[i].CurLane.num_lane - 1];
                    if (tmpLane.IfFilled() && tmpOneWay != cars[i].CurOneDir.obj.transform.parent.GetComponent<OneWayRoad>())
                    {                    
                        int cnt = 0;
                        while (tmp.LaneList[cnt].IfFilled() && cnt < tmp.LaneList.Count)
                            cnt++;
                        tmpLane = tmp.LaneList[cnt];
                    }
                    cars[i].CurrOneWay = tmpOneWay;
                    cars[i].CurLane = tmpLane;                   
                    cars[i].UpdateDirection();
                    cars[i].Current = tmpLane.StartNodeVec;
                    cars[i].Aim = tmpLane.FinishNodeVec;
                    cars[i].start = tmpLane.StartNodeVec;
                    cars[i].CurOneDir = tmp;
                }

            }

        }
    }



}