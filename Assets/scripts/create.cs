using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Random = UnityEngine.Random;
//GetPosOStartNodeVecode
/*public int FindFinishNodeVecode(int first_node, int second_node)
{
    for (int i = 0; i < AdjacencyList[first_node].Count; i++)
    {
        if (AdjacencyList[first_node][i].Item1 == second_node)
            return i;
    }
    return -1;
}*/
/*
    priority_queue<pair<int, int>> VList;
    void deixtra(, int position)
    {
        VList.push(make_pair(0, position));
        way[position] = 0;
        while (!VList.empty())
        {
            position = VList.top().second;
            VList.pop();
            //while(VList.top().second==position &&!VList.empty())
            //  VList.pop();
            vector<pair<int, int>>::iterator it = ListOfVertex[position].begin();
            for (; it != ListOfVertex[position].end(); it++)
                if (way[position] + ((*it).first) < way[(*it).second])
                {
                    way[(*it).second] = ((*it).first) + way[position];
                    VList.push(make_pair(-way[(*it).second], (*it).second));
                }
        }
    }
    */
public class create : MonoBehaviour
{
    public int lines;
    GameObject temp;
    public GameObject TmpObjEdg;
    public GameObject car_prefab;
    public GameObject node;
    public GameObject house;
    public GameObject Rect;
    public GameObject trafficlightpref;
    public Transform parent_nodes, parent_edges, parent_cars, parent_trafficLights, parent_houses, CanvasParent;
    public List<Node> nodes = new List<Node>();
    public List<Car> cars = new List<Car>();
    public Vector3 pos;
    int nfirst_node, nsecond_node, first_node, second_node, TLTemp1, TLTemp2;
    Vector3 start, finish;
    int FlagDrawEdge = 0, FlagClickSyncTL = 0, FlagClickDesyncTL = 0,FlagDelClicks=0;
    Camera cum;
    public List<List<(int, Road)>> AdjacencyList = new List<List<(int, Road)>>();
    public List<TrafficLight> TrafficLightList = new List<TrafficLight>();
    GameObject TmpObjNod;
    float k = 1;
    public int roads = 1, houses = 1, cars_amount = 1, TL_amount = 1;
    House first, second;
    public List<float> way = new List<float>();
    const int Nmax = 100000;
    const int MaxW = 2009000999;
    Queue<int> VList = new Queue<int>();
    public List<int> NodeWay = new List<int>();
    Road tmproad = new Road();
    public List<Road> ListOfRoads = new List<Road>();
    int remf, rems;
    GameObject remnode;
    public List<House> ListofHouses = new List<House>();
    Geometry Geometry = new Geometry();    
    void Start()
    {
        cum = Camera.main;
        temp = GameObject.Find("EventSystem");
        TmpObjNod = GameObject.Find("Curs");
    }
    Vector3 GetCoorByClick()
    {
        Ray ray = cum.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.forward, Vector3.zero);
        float rayDistance;
        groundPlane.Raycast(ray, out rayDistance);
        Vector3 point = ray.GetPoint(rayDistance);
        return point;
    }
    float FindNearestEdge()
    {
        if (AdjacencyList.Count == 0) return float.MaxValue;
        float minim = float.MaxValue;
        for (int i = 0; i < ListOfRoads.Count; i++)
            if (Geometry.IsOnLane(ListOfRoads[i].StartNodeVec, ListOfRoads[i].FinishNodeVec, pos))            
                if (minim > Geometry.GetHeight(pos, ListOfRoads[i].StartNodeVec, ListOfRoads[i].FinishNodeVec) && Geometry.IsOnLane(nodes[ListOfRoads[i].FirstNode].pos, nodes[ListOfRoads[i].SecondNode].pos, pos))
                {                    

                    minim = Geometry.GetHeight(pos, ListOfRoads[i].StartNodeVec, ListOfRoads[i].FinishNodeVec);
                    first_node = ListOfRoads[i].FirstNode;
                    second_node = ListOfRoads[i].SecondNode;
                } 
        return minim;

    }
    public void SpawnNode(Vector3 v)
    {
        if (remnode != null)
            Destroy(remnode);
        GameObject tmpobj = Instantiate(node, v, Quaternion.identity, parent_nodes);
        tmpobj.GetComponent<Node>().pos = v;
        tmpobj.GetComponent<Node>().number = nodes.Count;
        tmpobj.GetComponent<Node>().NodeObj = tmpobj;
        tmpobj.name = "node" + nodes.Count;
        nodes.Add(tmpobj.GetComponent<Node>());
        way.Add(0f);
        NodeWay.Add(0);
        AdjacencyList.Add(new List<(int, Road)>());

    }
    public void EdgeChange(Vector3 start, Vector3 finish, GameObject TmpObjEdg)
    {                
        TmpObjEdg.transform.position = (start+finish)/2;
        if (!temp.GetComponent<events>().OneWay)
            TmpObjEdg.GetComponent<SpriteRenderer>().size = new Vector2(Vector3.Distance(start, finish), 1f);
        else
            TmpObjEdg.GetComponent<SpriteRenderer>().size = new Vector2(Vector3.Distance(start, finish), 0.5f);
        
        TmpObjEdg.transform.rotation = Geometry.CalculateRotation(start, finish, 0f);
    }
    public Road SpawnEdge(int first_node, int second_node, Road ParentRoad)
    {
       
        Road TmpRoad = new Road();
        bool OneWay = false;
        if(ParentRoad!=null)
        {
            k = ParentRoad.Road_coefficient;
            OneWay = ParentRoad.OneWay;
        }
        else
        {
            if (temp.GetComponent<events>().OneWay) OneWay = true;
            if (temp.GetComponent<events>().FlagAsphalt)
                k = 1;
            else if (temp.GetComponent<events>().FlagStone)
                k = 0.6f;
            else
                k = 0.3f;
        }    
        if (!OneWay &&!Geometry.IsRightDirection(nodes[first_node].pos, nodes[second_node].pos))
            Swap(ref first_node, ref second_node);
        TmpRoad = TmpRoad.SpawnRoad(int.Parse(GameObject.Find("Main Camera").GetComponent<ReadInput>().input) , first_node, second_node, nodes[first_node].pos, nodes[second_node].pos, OneWay, k, parent_edges);        
        if (OneWay)
            AdjacencyList[first_node].Add((second_node, TmpRoad));
        else
        {
            AdjacencyList[first_node].Add((second_node, TmpRoad));
            AdjacencyList[second_node].Add((first_node, TmpRoad));
        }
        TmpRoad.obj.name = "Road" + roads++;
        ListOfRoads.Add(TmpRoad);
        return TmpRoad;
    }
    public void SpawnTrafficLight(int first_node, int second_node)
    {
        GameObject tmpObj = Instantiate(trafficlightpref, TmpObjNod.transform.position, TmpObjNod.transform.rotation, parent_trafficLights);
        tmpObj.AddComponent<TrafficLight>();
        TrafficLight tmp = tmpObj.GetComponent<TrafficLight>();
        Road tmpRoad = GetRoad(first_node, second_node);
        OneWayRoad tmpOneWay;
        if (tmpRoad.OneWay || (!tmpRoad.OneWay && Geometry.IsRightDirection(nodes[first_node].pos, nodes[second_node].pos)))
            tmpOneWay = tmpRoad.RightRoad;
        else
            tmpOneWay = tmpRoad.LeftRoad;
        OneDirRoad tmpOneDir = tmpOneWay.OneDirList[tmpOneWay.OneDirList.Count - 1];
        tmp.Init(tmpOneDir.StartNode, second_node, tmpObj, int.Parse(GameObject.Find("Main Camera").GetComponent<InputRed>().input), int.Parse(GameObject.Find("Main Camera").GetComponent<InputGreen>().input), 0, tmpOneDir);
        
        TrafficLightList.Add(tmp);
        tmp.name = "TrafficLight" + TL_amount++;
    }

    public Road GetRoad(int first, int second)
    {
        for (int i = 0; i < ListOfRoads.Count; i++)
            if (ListOfRoads[i] != null && ((ListOfRoads[i].SecondNode == first && ListOfRoads[i].FirstNode == second) || (ListOfRoads[i].SecondNode == second && ListOfRoads[i].FirstNode == first)))
                return ListOfRoads[i];
        return null;
    }

    public GameObject SpawnCar(int start, int finish, Vector3 middlePos)
    {
        Road tmpRoad = GetRoad(start, finish);
        Lane tmpLane;
        int tmpLaneNumber = Random.Range(1, tmpRoad.LanesNum + 1);
        OneWayRoad tmpOneWay;
        if (tmpRoad.OneWay || (!tmpRoad.OneWay && Geometry.IsRightDirection(nodes[start].pos, nodes[finish].pos)))
            tmpOneWay = tmpRoad.RightRoad;
        else
            tmpOneWay = tmpRoad.LeftRoad;
        OneDirRoad tmpOneDir = new OneDirRoad();
        for (int i = 0; i < tmpOneWay.OneDirList.Count; i++)
        {
            if (Geometry.IsBetweenPoints(tmpOneWay.OneDirList[i].StartNodeVec, tmpOneWay.OneDirList[i].FinishNodeVec, middlePos))
            {
                tmpOneDir = tmpOneWay.OneDirList[i];
                break;
            }
        }
        finish = tmpOneDir.FinishNode;
        tmpLane = tmpOneDir.LaneList[tmpLaneNumber - 1];       
        GameObject TmpObj = Instantiate(car_prefab, Geometry.GetPerpendicularIntersection(tmpLane.StartNodeVec, tmpLane.FinishNodeVec, middlePos), tmpLane.rotation, parent_cars);
        Car TmpCar = TmpObj.GetComponent<Car>();
        TmpCar.Init(tmpLane.StartNodeVec, Geometry.GetPerpendicularIntersection(tmpLane.StartNodeVec, tmpLane.FinishNodeVec, middlePos), tmpLane.FinishNodeVec, TmpObj, 0.1f, tmpLane, tmpOneDir, tmpOneWay, start, finish);
        cars.Add(TmpCar);
        TmpCar.name = "Car" + cars_amount++;
        return TmpObj;
    }
    void AddRoad(int CurPos,Road road, bool IsRight)
    {
        Debug.Log(road);
        OneDirRoad OneDirRoad = new OneDirRoad();
        int first,second;
        OneWayRoad Temp=road.RightRoad;
        if(!IsRight && !road.OneWay)
            Temp=road.LeftRoad;
        first=Temp.StartNode;
        int i=0;
        second=Temp.OneDirList[i++].FinishNode;
        while (second!=Temp.FinishNode && !Geometry.IsBetweenPoints(nodes[first].pos,nodes[second].pos,nodes[CurPos].pos))
        {
             first = second;
             second=Temp.OneDirList[i++].FinishNode;
        }    
        AdjacencyList[first].Remove((second, road));
        AdjacencyList[first].Add((CurPos, road));
        AdjacencyList[CurPos].Add((second, road));        

        int TL = -1;
        for(int j = 0; j < TrafficLightList.Count; j++)
        {
            if (TrafficLightList[j].first == Temp.OneDirList[i - 1].StartNode && TrafficLightList[j].second == Temp.OneDirList[i - 1].FinishNode)
                TL = j;
        }
        Destroy(Temp.OneDirList[i-1].obj);
        Temp.OneDirList.RemoveAt(i-1);
        Temp.OneDirList.Insert(i - 1, OneDirRoad.SpawnOneRoad(first, CurPos, road.LanesNum, nodes[first].pos, nodes[CurPos].pos, Temp.obj, road.OneWay));
        Temp.OneDirList.Insert(i, OneDirRoad.SpawnOneRoad(CurPos, second, road.LanesNum, nodes[CurPos].pos, nodes[second].pos, Temp.obj, road.OneWay));

        if (TL != -1)
        {
            TrafficLightList[TL].OneDir = Temp.OneDirList[i];
            TrafficLightList[TL].first = Temp.OneDirList[i].StartNode;
        }

    }
    public void SpawnHouse(Vector3 pos, bool IsRight)
    {
        GameObject TmpObj = Instantiate(house, TmpObjNod.transform.position, TmpObjNod.transform.rotation, parent_houses);
        ListofHouses.Add(TmpObj.GetComponent<House>());
        TmpObj.GetComponent<House>().road = GetRoad(first_node, second_node);
        TmpObj.GetComponent<House>().HNode = nodes.Count;
        TmpObj.GetComponent<House>().IsRight = IsRight;
        TmpObj.GetComponent<House>().HouseObj = TmpObj;
        SpawnNode(pos);
        nodes[nodes.Count - 1].HouseLink = TmpObj.GetComponent<House>();
        AdjacencyList.Add(new List<(int, Road)>());        
        AddRoad(nodes.Count - 1,GetRoad(first_node,second_node),IsRight);
        nodes[nodes.Count - 1].NodeObj.SetActive(false);
        TmpObj.name = "House"+houses++;
    }
    public bool EdgeExist(int first_node, int second_node)
    {
        for (int i = 0; i < AdjacencyList[first_node].Count; ++i)
            if (AdjacencyList[first_node][i].Item1 == second_node) return true;
        return false;
    }

    public int CatchNode(Vector3 pos)
    {
        for (int i = 0; i < nodes.Count; i++)
            if (nodes[i].HouseLink==null && Vector3.Distance(nodes[i].pos, pos) <= 0.5f)
                return i;
        return -1;
    }
    public int TrafficLightExist(Vector3 first_pos, Vector3 second_pos)
    {
        for (int i = 0; i < TrafficLightList.Count; i++)
            if (nodes[TrafficLightList[i].first].pos == first_pos && nodes[TrafficLightList[i].second].pos == second_pos)
                return i;
        return -1;
    }
    void DelWayRoad(OneWayRoad origin, OneWayRoad copyone,Road copy,int start,int finish)
    {
        Destroy(copyone.OneDirList[0].obj);
        copyone.OneDirList.Clear();
        for (int i = 0; i < origin.OneDirList.Count; i++)        
            if (Geometry.IsBetweenPoints(nodes[start].pos, nodes[finish].pos, nodes[origin.OneDirList[i].StartNode].pos) && Geometry.IsBetweenPoints(nodes[start].pos, nodes[finish].pos, nodes[origin.OneDirList[i].FinishNode].pos))
            {
                copyone.OneDirList.Add(origin.OneDirList[i]);
                origin.OneDirList[i].obj.transform.parent = copyone.transform;
            }        
    }
    Road CopyNRewriteRoad(Road origin, Road copy)
    {        
        if(origin.RightRoad!=null)        
            DelWayRoad(origin.RightRoad, copy.RightRoad, copy, copy.FirstNode, copy.SecondNode);                   
        if (origin.LeftRoad != null)       
            DelWayRoad(origin.LeftRoad, copy.LeftRoad, copy,copy.FirstNode, copy.SecondNode);       
        return copy;
    }
    void RewriteEdges(int first_node, int second_node, int inserted_node)
    {        
        Road TmpRoad = GetRoad(first_node, second_node);       
        AddRoad(inserted_node, TmpRoad, true);
        AddRoad(inserted_node, TmpRoad, false);
        Road tmp=SpawnEdge(first_node, inserted_node,TmpRoad);      
        tmp.Road_coefficient = TmpRoad.Road_coefficient;        
        CopyNRewriteRoad(TmpRoad,tmp);
        tmp = SpawnEdge(inserted_node,second_node,TmpRoad);
        tmp.Road_coefficient = TmpRoad.Road_coefficient;
        CopyNRewriteRoad(TmpRoad,tmp);
        AdjacencyList[first_node].Remove((inserted_node, TmpRoad));
        AdjacencyList[inserted_node].Remove((first_node, TmpRoad));
        AdjacencyList[inserted_node].Remove((second_node, TmpRoad));
        AdjacencyList[second_node].Remove((inserted_node, TmpRoad));
        ListOfRoads.Remove(TmpRoad);       
        Destroy(TmpRoad.obj);     
            
    }
    public int CatchTL(Vector3 pos)
    {
        for (int i = 0; i < TrafficLightList.Count; i++)
            if (Vector3.Distance(TrafficLightList[i].obj.transform.position, pos) <= 0.3f)
                return i;
        return -1;
    }
    public void ChangeCurs(GameObject ChangeTo)
    {
        if (ChangeTo == house && TmpObjNod.tag != "House")
        {
            Vector3 rem = TmpObjNod.transform.position;
            Destroy(TmpObjNod);
            TmpObjNod = Instantiate(ChangeTo, rem, Quaternion.Euler(0, 0, 0), CanvasParent);
            TmpObjNod.name = "Curs";
            TmpObjNod.tag = "House";

        }
        else if (ChangeTo == node && TmpObjNod.tag != "Node")
        {
            Vector3 rem = TmpObjNod.transform.position;
            Destroy(TmpObjNod);
            TmpObjNod = Instantiate(ChangeTo, rem, Quaternion.Euler(0, 0, 0), CanvasParent);
            TmpObjNod.name = "Curs";
            TmpObjNod.tag = "Node";
        }
        else if (ChangeTo == car_prefab && TmpObjNod.tag != "Car")
        {
            Vector3 rem = TmpObjNod.transform.position;
            Destroy(TmpObjNod);
            TmpObjNod = Instantiate(ChangeTo, rem, Quaternion.Euler(0, 0, 0), CanvasParent);
            Destroy(TmpObjNod.GetComponent<Car>());
            Destroy(TmpObjNod.GetComponent<BoxCollider>());
            Destroy(TmpObjNod.GetComponent<Rigidbody>());
            TmpObjNod.name = "Curs";
            TmpObjNod.tag = "Car";            
        }
        else if (ChangeTo == trafficlightpref && TmpObjNod.tag != "Traffic")
        {
            Vector3 rem = TmpObjNod.transform.position;
            Destroy(TmpObjNod);
            TmpObjNod = Instantiate(ChangeTo, rem, Quaternion.Euler(0, 0, 0), CanvasParent);
            Destroy(TmpObjNod.GetComponent<BoxCollider>());
            TmpObjNod.name = "Curs";
            TmpObjNod.tag = "TL";
        }
        else if (ChangeTo == null)
        {
            Vector3 rem = TmpObjNod.transform.position;
            Destroy(TmpObjNod);
            TmpObjNod = Instantiate(node, rem, Quaternion.Euler(0, 0, 0), CanvasParent);
            TmpObjNod.transform.localScale = new Vector3(0f, 0f, 0);
            TmpObjNod.name = "Curs";
            TmpObjNod.tag = "NULL";
        }
    }
    static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }
    void DelSomeShit(Road tmp, bool IsLeft)
    {
        OneWayRoad tmponeway = tmp.RightRoad;
        if(IsLeft)
            tmponeway = tmp.LeftRoad;
        for (int i = 0; i < tmponeway.OneDirList.Count; i++)
        {
            AdjacencyList[tmponeway.OneDirList[i].StartNode].Remove((tmponeway.OneDirList[i].FinishNode, tmp));
            if (nodes[tmponeway.OneDirList[i].FinishNode].HouseLink != null)
            {
                ListofHouses.Remove(nodes[tmponeway.OneDirList[i].FinishNode].HouseLink);
                Destroy(nodes[tmponeway.OneDirList[i].FinishNode].HouseLink.HouseObj);
                Destroy(nodes[tmponeway.OneDirList[i].FinishNode].HouseLink);
            }
            if (tmponeway.OneDirList[i].FinishNode != tmponeway.FinishNode)
            {
                Destroy(nodes[tmponeway.OneDirList[i].FinishNode].NodeObj);
                Destroy(nodes[tmponeway.OneDirList[i].FinishNode]);
            }
        }
    }
    void DeleteEdge(int first,int second)
    {
        Road tmp = GetRoad(first, second);
        if (tmp == null)
        {
            tmp = GetRoad(second,first);
            if (tmp == null)
                return;
        }
        ListOfRoads.Remove(tmp);
        DelSomeShit(tmp, false);
        if(tmp.LeftRoad!=null)        
            DelSomeShit(tmp, true);            
        if(AdjacencyList[tmp.FirstNode].Count==0)
        {
            Destroy(nodes[tmp.FirstNode].NodeObj);
            Destroy(nodes[tmp.FirstNode]);           
        }
        if (AdjacencyList[tmp.SecondNode].Count == 0)
        {
            Destroy(nodes[tmp.SecondNode].NodeObj);
            Destroy(nodes[tmp.SecondNode]);            
        }
        Destroy(tmp.obj);
        Destroy(tmp);       
    }
    bool IfIntersected(Vector3 pos)
    {
        return (AdjacencyList.Count != 0 && Geometry.IsOnLane(nodes[first_node].pos, nodes[second_node].pos, pos) && Vector3.Distance(Geometry.GetPerpendicularIntersection(nodes[first_node].pos, nodes[second_node].pos, pos), pos) <= 0.5f);
    }
    void Update()
    {
        
        if (EventSystem.current.IsPointerOverGameObject()) return;
        pos = GetCoorByClick();
        TmpObjNod.transform.position = pos;
        if (temp.GetComponent<events>().pause_flag)
            ChangeCurs(null);
        else if (temp.GetComponent<events>().FlagEdges)
        {
            ChangeCurs(node);
            if (Input.GetMouseButtonDown(0))
            {
                if (AdjacencyList.Count != 0 && Geometry.IsOnLane(nodes[first_node].pos, nodes[second_node].pos, pos) && Vector3.Distance(Geometry.GetPerpendicularIntersection(nodes[first_node].pos, nodes[second_node].pos, pos), pos) <= 1f)
                    pos = Geometry.GetPerpendicularIntersection(nodes[first_node].pos, nodes[second_node].pos, pos);
                if (FlagDrawEdge == 0)
                {
                    nfirst_node = CatchNode(pos);
                    if (nfirst_node == -1)
                    {
                        remf = -1;
                        rems = -1;                      
                        remnode = Instantiate(node, pos, Quaternion.identity);
                        nfirst_node = nodes.Count;
                        if (IfIntersected(pos))
                        {
                            remf = first_node;
                            rems = second_node;
                        }
                    }                 
                    
                    FlagDrawEdge = 1;
                    TmpObjEdg = Instantiate(Rect, pos, Geometry.CalculateRotation(pos, pos, 0f));
                    if (temp.GetComponent<events>().FlagAsphalt)
                        TmpObjEdg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Asphalt");
                    else if (temp.GetComponent<events>().FlagStone)
                        TmpObjEdg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Stone");
                    else
                        TmpObjEdg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("OffRoad");
                    TmpObjEdg.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
                    if(temp.GetComponent<events>().OneWay)
                        TmpObjEdg.GetComponent<SpriteRenderer>().size = new Vector2(0f, 0.5f);
                    else
                        TmpObjEdg.GetComponent<SpriteRenderer>().size = new Vector2(0f, 1f);
                }
                else if (FlagDrawEdge == 1 && (remnode==null || Vector3.Distance(remnode.transform.position,pos)>=1))
                {
                    Vector3 v;
                    if (remnode != null)
                    {                       
                        v = remnode.transform.position;
                        SpawnNode(v);
                    }                                                            
                    nsecond_node = CatchNode(pos);
                    Destroy(TmpObjEdg);                        
                    if(remf!=-1)
                    {

                        RewriteEdges(remf, rems, nfirst_node);
                        remf = -1;
                        rems = -1;
                    }
                    if (nsecond_node == -1)
                    {                     
                        nsecond_node = nodes.Count;
                        SpawnNode(pos);
                        if (IfIntersected(pos))
                            RewriteEdges(first_node, second_node, nsecond_node);                        
                    }
                    FlagDrawEdge = 0;
                    if (GetRoad(nfirst_node, nsecond_node)==null)
                        SpawnEdge(nfirst_node, nsecond_node,null);                                        
                }               
            }
            FindNearestEdge();
            if (AdjacencyList.Count != 0 && Geometry.IsOnLane(nodes[first_node].pos, nodes[second_node].pos, pos) && Vector3.Distance(Geometry.GetPerpendicularIntersection(nodes[first_node].pos, nodes[second_node].pos, pos), pos) <= 1f)
                TmpObjNod.transform.position = Geometry.GetPerpendicularIntersection(nodes[first_node].pos, nodes[second_node].pos, pos);
            else
                TmpObjNod.transform.position = pos;
            if(FlagDrawEdge == 1)
            {
                if (TmpObjEdg!=null && remnode != null)
                    EdgeChange(remnode.transform.position, TmpObjNod.transform.position, TmpObjEdg);
                else if(TmpObjEdg!=null)
                    EdgeChange(nodes[nfirst_node].pos, TmpObjNod.transform.position, TmpObjEdg);
            }
            

        }
        else if(!temp.GetComponent<events>().FlagEdges)
        {
            remf = -1;
            rems = -1;
            FlagDrawEdge = 0;
            if (remnode != null)           
                Destroy(remnode);            
            if(TmpObjEdg!=null)
                Destroy(TmpObjEdg);
        }
        
        if (temp.GetComponent<events>().FlagCars )
        {
            ChangeCurs(car_prefab);           
            FindNearestEdge();
            if (Input.GetMouseButtonDown(0) && CatchNode(TmpObjNod.transform.position) != -1)
                SpawnCar(CatchNode(TmpObjNod.transform.position), CatchNode(TmpObjNod.transform.position), nodes[CatchNode(TmpObjNod.transform.position)].pos);
            else if (AdjacencyList.Count != 0 && Geometry.IsOnLane(nodes[first_node].pos, nodes[second_node].pos, pos) && Vector3.Distance(Geometry.GetPerpendicularIntersection(nodes[first_node].pos, nodes[second_node].pos, pos), pos) <= 1f)
            {
                Vector3 Res = Geometry.GetPerpendicularIntersection(nodes[first_node].pos, nodes[second_node].pos, pos);
                TmpObjNod.transform.rotation = Geometry.CalculateRotation(nodes[second_node].pos, nodes[first_node].pos, 0f);
                TmpObjNod.transform.position = Geometry.GetPerpendicularPoint(Res, pos,0.25f);
                if (Input.GetMouseButtonDown(0))
                {
                    int endNode= second_node, startNode= first_node;
                    if (Geometry.Vector_Multi(nodes[second_node].pos - nodes[first_node].pos, TmpObjNod.transform.position - nodes[first_node].pos) > 0)
                    {
                        endNode = first_node;
                        startNode = second_node;
                    }
                    SpawnCar(startNode, endNode, Res);

                }
            }
        }
        else if (temp.GetComponent<events>().FlagTrafficLights )
        {
            ChangeCurs(trafficlightpref);
            FindNearestEdge();
            if (AdjacencyList.Count != 0 && Geometry.IsOnLane(nodes[first_node].pos, nodes[second_node].pos, pos) && Vector3.Distance(Geometry.GetPerpendicularIntersection(nodes[first_node].pos, nodes[second_node].pos, pos), pos) <= 0.5f)
            {
                
                Vector3 Res = Geometry.GetPerpendicularIntersection(nodes[first_node].pos, nodes[second_node].pos, pos);
                TmpObjNod.transform.rotation = Geometry.CalculateRotation(nodes[second_node].pos, nodes[first_node].pos, 0f);
                TmpObjNod.transform.Rotate(0, 0, 90, Space.Self);
                if (Vector3.Distance(nodes[first_node].pos, pos) <= 1f)
                {
                    pos = nodes[first_node].pos;
                    Vector3 offset = new Vector3(0, 0, 0);
                    offset = Geometry.RotateRound((nodes[first_node].pos - nodes[second_node].pos).normalized, new Vector3(0, 0, 0), 90) * 1 / 4 + (nodes[first_node].pos - nodes[second_node].pos).normalized * 0.4f;
                    TmpObjNod.transform.position =pos- offset;
                    Swap<int>(ref first_node, ref second_node);
                }
                    
                else if (Vector3.Distance(nodes[second_node].pos, pos) <= 1f)
                {
                    pos = nodes[second_node].pos;
                    Vector3 offset = new Vector3(0, 0, 0);
                    offset = Geometry.RotateRound((nodes[second_node].pos - nodes[first_node].pos).normalized, new Vector3(0, 0, 0), 90) * 1 / 4 + (nodes[second_node].pos - nodes[first_node].pos).normalized * 0.4f;
                    TmpObjNod.transform.position = pos - offset;
                    
                }
                if (Input.GetMouseButtonDown(0) && (Vector3.Distance(nodes[first_node].pos, pos)<=2 || Vector3.Distance(nodes[second_node].pos, pos) <= 2f)) 
                    SpawnTrafficLight(first_node, second_node);
               
            }

            
            
        }
        else if(temp.GetComponent<events>().FlagHouses)
        {
            
            FindNearestEdge();
            if(AdjacencyList.Count != 0 && Geometry.IsOnLane(nodes[first_node].pos, nodes[second_node].pos, pos) && Vector3.Distance(Geometry.GetPerpendicularIntersection(nodes[first_node].pos, nodes[second_node].pos, pos), pos) <= 3.5f)
            {
                ChangeCurs(house);
                Vector3 Res = Geometry.GetPerpendicularIntersection(nodes[first_node].pos, nodes[second_node].pos, pos);
                Road tmp = GetRoad(first_node, second_node);
                TmpObjNod.transform.rotation = Geometry.CalculateRotation(nodes[second_node].pos, nodes[first_node].pos, 0f);       
                TmpObjNod.transform.position = Geometry.GetPerpendicularPoint(Res, pos,0.25f);                     
                if (Geometry.IsRightDirection(nodes[first_node].pos, nodes[second_node].pos))
                {
                    if (Geometry.Vector_Multi(nodes[second_node].pos - nodes[first_node].pos, TmpObjNod.transform.position - nodes[first_node].pos) > 0 && TmpObjNod.transform.rotation.z > 180)
                        TmpObjNod.transform.Rotate(0, 0, -180, Space.Self);
                    else if (Geometry.Vector_Multi(nodes[second_node].pos - nodes[first_node].pos, TmpObjNod.transform.position - nodes[first_node].pos) <= 0 && TmpObjNod.transform.rotation.z < 180)
                        TmpObjNod.transform.Rotate(0, 0, 180, Space.Self);
                }
                else
                {
                    if (Geometry.Vector_Multi(nodes[first_node].pos - nodes[second_node].pos, TmpObjNod.transform.position - nodes[second_node].pos) > 0 && TmpObjNod.transform.rotation.z > 180)
                        TmpObjNod.transform.Rotate(0, 0, -180, Space.Self);
                    else if (Geometry.Vector_Multi(nodes[first_node].pos - nodes[second_node].pos, TmpObjNod.transform.position - nodes[second_node].pos) <= 0 && TmpObjNod.transform.rotation.z < 180)
                        TmpObjNod.transform.Rotate(0, 0, 180, Space.Self);
                }
                
                if (Input.GetMouseButtonDown(0))
                {
                    if (Geometry.IsRightDirection(nodes[first_node].pos, nodes[second_node].pos) && Geometry.Vector_Multi(nodes[second_node].pos - nodes[first_node].pos, TmpObjNod.transform.position - nodes[first_node].pos) > 0)
                    {
                        Swap<int>(ref second_node, ref first_node);
                        SpawnHouse(Res,false);
                    }
                    else if (Geometry.IsRightDirection(nodes[second_node].pos, nodes[first_node].pos) && Geometry.Vector_Multi(nodes[first_node].pos - nodes[second_node].pos, TmpObjNod.transform.position - nodes[second_node].pos) > 0)
                    {
                        Swap<int>(ref second_node, ref first_node);
                        SpawnHouse(Res, false);
                    }
                    else
                        SpawnHouse(Res,true);
                }
                    
            }
            else
                ChangeCurs(node);
        }
        else if (temp.GetComponent<events>().FlagSyncTL)
        {
            ChangeCurs(null);
            if (Input.GetMouseButtonDown(0))
            {
                if (FlagClickSyncTL == 0)
                {
                    TLTemp1 = CatchTL(pos);
                    if (TLTemp1 != -1)
                        FlagClickSyncTL = 1;
                }
                else if (FlagClickSyncTL == 1)
                {
                    TLTemp2 = CatchTL(pos);
                    if (TLTemp2 != -1)
                    {
                        TrafficLightList[TLTemp2].time = TrafficLightList[TLTemp1].time;
                        TrafficLightList[TLTemp2].red_period = TrafficLightList[TLTemp1].red_period;
                        TrafficLightList[TLTemp2].green_period = TrafficLightList[TLTemp1].green_period;
                        TrafficLightList[TLTemp2].curr_period = TrafficLightList[TLTemp1].curr_period;
                        TrafficLightList[TLTemp2].flag = TrafficLightList[TLTemp1].flag;
                        TrafficLightList[TLTemp2].obj.GetComponent<Renderer>().material.color = TrafficLightList[TLTemp1].obj.GetComponent<Renderer>().material.color;
                        FlagClickSyncTL = 0;
                    }
                }
            }
        }
        else if (temp.GetComponent<events>().FlagDesyncTL)
        {
            ChangeCurs(null);
            if(Input.GetMouseButtonDown(0))
            {
                if (FlagClickDesyncTL == 0)
                {
                    TLTemp1 = CatchTL(pos);
                    if (TLTemp1 != -1)
                        FlagClickDesyncTL = 1;
                }
                else if (FlagClickDesyncTL == 1)
                {
                    TLTemp2 = CatchTL(pos);
                    if (TLTemp2 != -1)
                    {
                        TrafficLightList[TLTemp2].time = TrafficLightList[TLTemp1].time;
                        TrafficLightList[TLTemp2].flag = !TrafficLightList[TLTemp1].flag;
                        if (TrafficLightList[TLTemp2].flag)
                        {
                            TrafficLightList[TLTemp2].obj.GetComponent<Renderer>().material.color = Color.green;
                        }
                        else
                            TrafficLightList[TLTemp2].obj.GetComponent<Renderer>().material.color = Color.red;
                        FlagClickDesyncTL = 0;
                    }
                }
            }
            
        }
        else if (temp.GetComponent<events>().DelFlag)
        {

            ChangeCurs(null);
            if(Input.GetMouseButtonDown(0))
            {
                nsecond_node = CatchNode(pos);
                
                if (nsecond_node!=-1)
                {
                    if (FlagDelClicks % 2==1)
                    {
                        if (nfirst_node == nsecond_node)                                                   
                            for (int i = 0; i< AdjacencyList[nfirst_node].Count;)                            
                                DeleteEdge(nfirst_node, AdjacencyList[nfirst_node][i].Item1);                                                    
                        else                        
                            DeleteEdge(nfirst_node, nsecond_node);                        
                    }
                    else
                        nfirst_node = nsecond_node;
                    FlagDelClicks++;
                }
            }
        }
        if (!temp.GetComponent<events>().DelFlag)
        {
            FlagDelClicks = 0;          
        }
    }
}
