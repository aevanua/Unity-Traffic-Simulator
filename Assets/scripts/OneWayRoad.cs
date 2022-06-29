using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayRoad : MonoBehaviour
{
    GameObject temp;
    public int Calc_Cars;
    public int StartNode, FinishNode;
    public List<OneDirRoad> OneDirList = new List<OneDirRoad>();
    bool Oneway;
    Vector3 StartNodeVec, FinishNodeVec;
    public GameObject obj;
    Geometry Geometry = new Geometry();
    OneDirRoad OneDirRoad = new OneDirRoad();
    public float Road_coefficient;

    public void Calculate_cars()
    {
        Calc_Cars = 0;
        for (int i = 0; i < OneDirList.Count; i++)
            Calc_Cars += OneDirList[i].OneDirCars;
    }

    public OneWayRoad SpawnOneWayRoad(int StartNode, int FinishNode, Vector3 StartNodeVec, Vector3 FinishNodeVec, int LanesNum, GameObject parent, float Road_coefficient, bool OneWay , bool flag_traffic)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("OneWayRoad"), Vector3.zero, Quaternion.Euler(0, 0, 0), parent.transform);
        obj.GetComponent<OneWayRoad>().Road_coefficient = Road_coefficient;
        obj.GetComponent<OneWayRoad>().StartNode = StartNode;
        obj.GetComponent<OneWayRoad>().FinishNode = FinishNode;
        obj.GetComponent<OneWayRoad>().FinishNodeVec = StartNodeVec;
        obj.GetComponent<OneWayRoad>().FinishNodeVec = FinishNodeVec;
        obj.GetComponent<OneWayRoad>().obj = obj;
        if (!OneWay)
        {
            this.StartNodeVec = Geometry.GetPerpendicularPointTemp(FinishNodeVec - StartNodeVec, StartNodeVec, -LanesNum / 8f);
            this.FinishNodeVec = Geometry.GetPerpendicularPointTemp(FinishNodeVec - StartNodeVec, FinishNodeVec, -LanesNum / 8f);
        }
        else
        {
            this.StartNodeVec = StartNodeVec;
            this.FinishNodeVec = FinishNodeVec;
        }
        obj.transform.GetChild(0).transform.position = (this.StartNodeVec + this.FinishNodeVec) / 2;        
        obj.transform.GetChild(0).transform.rotation = Geometry.CalculateRotation(this.StartNodeVec, this.FinishNodeVec, 0f);
        if (!flag_traffic)
        {
            if (Road_coefficient == 1)
            {
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Asphalt");
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "Asphalt";
            }
            if (Road_coefficient == 0.3f)
            {
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("OffRoad");
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "OffRoad";
            }
            if (Road_coefficient == 0.6f)
            {
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Stone");
                obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "Stone";
            }
        }
        
        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().size = new Vector2(Vector3.Distance(StartNodeVec, FinishNodeVec), 0.25f*LanesNum);
        for (int i = 1; i <= LanesNum; i++)
        {
            float offset = 0;
            if (OneWay)
                offset = -0.25f * LanesNum / 2 + 0.25f * (i - 1) + 0.125f;
            else
                offset = -0.25f * LanesNum + 0.25f * (i - 1) + 0.125f;
            if (LanesNum == 1 && OneWay)
                offset = 0;
            Vector3 TempStart = Geometry.GetPerpendicularPointTemp(FinishNodeVec - StartNodeVec, StartNodeVec, offset);
            Vector3 TempFinish = Geometry.GetPerpendicularPointTemp(FinishNodeVec - StartNodeVec, FinishNodeVec, offset);
            if (!(OneWay && i == LanesNum))
            {
                GameObject obj2 = Instantiate(Resources.Load<GameObject>("LaneTexture"), (TempStart + TempFinish) / 2, Geometry.CalculateRotation(TempStart, TempFinish, 0f), obj.transform);
                if (!Geometry.IsRightDirection(TempStart, TempFinish)) obj2.transform.rotation *= Quaternion.Euler(0, 0, 180);
                if (Road_coefficient != 0.3f)
                {
                    if (i ==LanesNum && !OneWay) obj2.transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("LaneSprite2");
                    else if (i != LanesNum) obj2.transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("LaneSprite1");
                    obj2.transform.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
                    if (i == LanesNum && !OneWay) obj2.transform.GetComponent<SpriteRenderer>().size = new Vector2(Vector3.Distance(TempStart, TempFinish), 0.3f);
                    else if (i != LanesNum) obj2.transform.GetComponent<SpriteRenderer>().size = new Vector2(Vector3.Distance(TempStart, TempFinish) - 1.5f, 0.3f);
                }
                obj2.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        obj.GetComponent<OneWayRoad>().OneDirList.Add(OneDirRoad.SpawnOneRoad(StartNode, FinishNode, LanesNum, StartNodeVec, FinishNodeVec, obj, OneWay));
        return obj.GetComponent<OneWayRoad>();
    }

    void Set_color()
    {
        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(Calc_Cars / Vector3.Distance(StartNodeVec , FinishNodeVec) * 65, Vector3.Distance(StartNodeVec , FinishNodeVec) / (Calc_Cars * 65 + 0.00001f), 0);
    }

    void Start()
    {
        temp = GameObject.Find("EventSystem");
    }

    void Update()
    {
        Calculate_cars();
        if (temp.GetComponent<events>().FlagTrafficColor)
            Set_color();
    }
}
