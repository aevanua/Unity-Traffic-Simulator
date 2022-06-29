using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Vector3 Current, Aim, start;
    public int first_node, second_node;
    public int FirstNodeWay, SecondNodeWay;
    public float SpeedLimit;
    bool stopped = false;
    public GameObject obj;
    GameObject temp, temp2;
    public float eps = 0.01f;
    public Lane CurLane;
    public OneDirRoad CurOneDir;
    public Vector3 Direction;
    public float Speed;
    List<TrafficLight> TrafficLightsList = new List<TrafficLight>();
    public List<Node> nodes = new List<Node>();
    public Stack<OneDirRoad> WayList = new Stack<OneDirRoad>();
    Geometry Geometry = new Geometry();
    public OneWayRoad CurrOneWay;
    List<List<(int, Road)>> AdjacencyList = new List<List<(int, Road)>>();
    public List<int> NodeWay = new List<int>();
    const int MaxW = 200900099;
    public List<float> way = new List<float>();
    public Queue<int> VList = new Queue<int>();
    public Vector3 GetCurrent()
    {
        return Current;
    }

    public void Init(Vector3 start, Vector3 Current, Vector3 Aim, GameObject obj, float SpeedLimit, Lane CurLane, OneDirRoad CurOneDir, OneWayRoad CurrOneWay, int first_node, int second_node)
    {

        this.start = start; this.Current = Current; this.Aim = Aim; this.SpeedLimit = SpeedLimit; this.obj = obj; this.CurLane = CurLane; this.first_node = first_node; this.second_node = second_node; this.CurOneDir = CurOneDir; this.CurrOneWay = CurrOneWay; this.FirstNodeWay = first_node; this.SecondNodeWay = second_node;
    }
    public int FindNextOneDir()
    {
        for (int i = 0; i < CurrOneWay.OneDirList.Count; i++)
        {
            if (CurOneDir == CurrOneWay.OneDirList[CurrOneWay.OneDirList.Count - 1])
                return -1;
            if (CurOneDir.FinishNode == CurrOneWay.OneDirList[i].StartNode)
                return i;
        }
        return -2;
    }
    public bool Finished()
    {
        return (Vector3.Distance(Aim, Current) <= eps);
    }

    public void UpdateDirection()
    {
        Direction = (Aim - Current).normalized;
    }

    public int check_traffic_light()
    {
        for (int i = 0; i < TrafficLightsList.Count; i++)
        {
            if (TrafficLightsList[i].first == CurLane.StartNode && TrafficLightsList[i].second == CurLane.FinishNode)
                return i;
        }
        return -1;
        /*for (int i = 0; i < TrafficLightsList.Count; i++)
        {
            if (Geometry.IsBetweenPoints(nodes[TrafficLightsList[i].first], nodes[TrafficLightsList[i].second], Current))
                return i;
        }
        return -1;*/
    }

    /*public void UpdateSpeed()
    {
        if (Vector3.Distance(Aim, Current) < 0.3f * Vector3.Distance(Aim, start))
        {
            float a = SpeedLimit / 2800f;
            Speed -= a;
        }
    }*/

    public OneDirRoad GetODR(int first, int second)
    {
        for (int i = 0; i < temp2.GetComponent<create>().ListOfRoads.Count; i++)
        {
            Road tmpRoad = temp2.GetComponent<create>().ListOfRoads[i];
            OneWayRoad tmpOneWay;
            if (tmpRoad.OneWay || (!tmpRoad.OneWay && Geometry.IsRightDirection(nodes[first].pos, nodes[second].pos)))
                tmpOneWay = tmpRoad.RightRoad;
            else
                tmpOneWay = tmpRoad.LeftRoad;

            for (int k = 0; k < tmpOneWay.OneDirList.Count; k++)
            {

                if ((first == tmpOneWay.OneDirList[k].StartNode && second == tmpOneWay.OneDirList[k].FinishNode) || (first == tmpOneWay.OneDirList[k].FinishNode && second == tmpOneWay.OneDirList[k].StartNode))
                    return tmpOneWay.OneDirList[k];
            }
        }
        return null;
    }

    public void deixtra(int position, int finish, float CarSpeed)
    {

        for (int x = 0; x < nodes.Count; x++)
        {
            way[x] = MaxW;
            NodeWay[x] = -1;
        }
        VList.Enqueue(position);
        way[position] = 0f;
        while (VList.Count != 0)
        {
            position = VList.Dequeue();
            for (int i = 0; i < AdjacencyList[position].Count; i++)
            {

                int index = AdjacencyList[position][i].Item1;
                float time = 0f;
                OneDirRoad tmp = GetODR(position, index);
                TrafficLight temp = null;
                for (int j = 0; j < TrafficLightsList.Count; j++)
                    if (TrafficLightsList[j].OneDir == tmp)
                        temp = TrafficLightsList[j];
                if (temp != null && !temp.flag)
                    time = temp.curr_period - temp.time;
                if (AdjacencyList[position][i].Item2 != null && way[position] + Vector3.Distance(nodes[position].pos, nodes[index].pos) / CarSpeed + time < way[index])
                {
                    NodeWay[index] = position;
                    way[index] = Vector3.Distance(nodes[position].pos, nodes[index].pos) / CarSpeed + time + way[position];
                    VList.Enqueue(index);
                }
            }

        }
    }

    public void ConvertDeixtra(int start, int finish)
    {
        while (finish != start)
        {
            WayList.Push(GetODR(NodeWay[finish], finish));
            finish = NodeWay[finish];
        }

    }

    void Start()
    {
        temp = GameObject.Find("EventSystem");
        temp2 = GameObject.Find("path");
    }
    public Road GetRoad(int first, int second)
    {
        for (int i = 0; i < temp2.GetComponent<create>().AdjacencyList[first].Count; i++)
            if (temp2.GetComponent<create>().AdjacencyList[first][i].Item2.SecondNode == second)
                return temp2.GetComponent<create>().AdjacencyList[first][i].Item2;
        return temp2.GetComponent<create>().AdjacencyList[first][0].Item2;
    }
    void Update()
    {
        AdjacencyList = temp2.GetComponent<create>().AdjacencyList;
        eps = SpeedLimit * temp.GetComponent<events>().worldspeed;
        TrafficLightsList = temp2.GetComponent<create>().TrafficLightList;
        nodes = temp2.GetComponent<create>().nodes;
        way = temp2.GetComponent<create>().way;
        NodeWay = temp2.GetComponent<create>().NodeWay;
        if (Vector3.Distance(Current, Aim) < Vector3.Distance(start, Aim) * 0.1 && second_node != SecondNodeWay)
        {
            VList.Clear();
            WayList.Clear();
            deixtra(second_node, SecondNodeWay, SpeedLimit * temp.GetComponent<events>().worldspeed);
            ConvertDeixtra(second_node, SecondNodeWay);
        }
        int TrafficLight = check_traffic_light();
        if (!temp.GetComponent<events>().pause_flag)
        {
            if ((TrafficLight > -1 && TrafficLight < TrafficLightsList.Count && !TrafficLightsList[TrafficLight].flag) || (CurLane.CurCarStop != 0 && !stopped) || (WayList.Count > 0 && WayList.Peek().IfFilled()) || (WayList.Count == 0 && FindNextOneDir() >= 0 && CurrOneWay.OneDirList[FindNextOneDir()].IfFilled()))
            {


                Vector3 StopPos = Aim;
                StopPos -= (Aim - start).normalized * (CurLane.CurCarStop * 0.7f + 0.65f);
                if ((Vector3.Distance(StopPos, Current) < eps) || (Vector3.Distance(StopPos, Aim) > Vector3.Distance(Current, Aim)))
                {
                    Speed = 0;
                    if ((Vector3.Distance(StopPos, Current) < eps) && !stopped)
                        CurLane.CurCarStop++;
                    stopped = true;
                }
            }
            else
            {
                if (stopped)
                    CurLane.CurCarStop = 0;
                stopped = false;
            }

        }
        if (!stopped)
            Speed = SpeedLimit * temp.GetComponent<events>().worldspeed;
        if (temp.GetComponent<events>().FlagEdit)
            Destroy(this);
        UpdateDirection();
        obj.transform.right = Aim - Current;
        Current += Speed * Direction * CurrOneWay.obj.transform.parent.GetComponent<Road>().Road_coefficient;
        obj.transform.position = Current;
    }
}