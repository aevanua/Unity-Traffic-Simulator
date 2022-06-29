 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class events : MonoBehaviour
{
    public bool FlagEdit = true;
    public bool FlagCars = false;
    public bool FlagEdges = false;
    public bool FlagTrafficColor = false, FlagTrafficSleep = true, TrafficRepeat = false;
    public bool FlagAsphalt = true, FlagStone = false, FlagOffRoad = false;
    public bool FlagTrafficLights = false;
    public bool FlagHouses = false;
    public bool ShowMenu = false;
    public bool OneWay = false;
    public bool pause_flag = false;
    public bool FlagSpawn = false;
    public bool DelFlag = false;
    public float worldspeed = 1.0f;
    public float prev_worldspeed = 1.0f;
    public GameObject CarOb;
    public GameObject EdgOb;
    public GameObject TrLOb;
    public GameObject HOb;
    public GameObject TrDOb;
    public GameObject Asphalt;
    public GameObject Stone;
    public GameObject OffRoad;
    public GameObject Sync;
    public GameObject Desync;
    public GameObject Inc;
    public GameObject Dec;
    public GameObject One;
    public GameObject Two;
    public GameObject Increase;
    public GameObject Decrease;
    public GameObject temp;
    public GameObject RoadCanvas;
    public GameObject TLCanvas;
    public bool FPS = false;
    public bool TPS = true;
    public bool FlagSyncTL = false;
    public bool FlagDesyncTL = false;

    void Start()
    {
        temp = GameObject.Find("path");
        CarOb = GameObject.Find("CarButton");
        EdgOb = GameObject.Find("EdgeButton");
        TrLOb = GameObject.Find("TrafficLightButton");
        HOb = GameObject.Find("HouseButton");
        TrDOb = GameObject.Find("TrafficDisplayButton");
        Asphalt = GameObject.Find("Asphalt");
        Stone = GameObject.Find("Stone");
        OffRoad = GameObject.Find("OffRoad");
        Sync = GameObject.Find("SyncTLButton");
        Desync = GameObject.Find("DesyncTLButton");
        One = GameObject.Find("OneWay");
        Two = GameObject.Find("TwoWay");
        Increase = GameObject.Find("Increase");
        Decrease = GameObject.Find("Decrease");
        RoadCanvas = GameObject.Find("RoadMenu");
        TLCanvas = GameObject.Find("TLMenu");
        FlagAsphalt = true;
        RoadCanvas.SetActive(false);
        TLCanvas.SetActive(false);
        OneWay = false;
        CarOb.GetComponent<Button>().interactable = false;
    }

    public void MenuButton()
    {
        if (!ShowMenu)
        {
            CarOb.SetActive(false);
            EdgOb.SetActive(false);
            TrLOb.SetActive(false);
            One.SetActive(false);
            Two.SetActive(false);
            HOb.SetActive(false);
            TrDOb.SetActive(false);
            Asphalt.SetActive(false);
            Stone.SetActive(false);
            OffRoad.SetActive(false);
            Sync.SetActive(false);
            Desync.SetActive(false);
        }
        else
        {
            HOb.SetActive(true);
            CarOb.SetActive(true);
            EdgOb.SetActive(true);
            TrLOb.SetActive(true);
            TrDOb.SetActive(true);
            Sync.SetActive(true);
            Desync.SetActive(true);
            if (FlagEdges)
            {
                Asphalt.SetActive(true);
                Stone.SetActive(true);
                OffRoad.SetActive(true);
                One.SetActive(false);
                Two.SetActive(false);
            }

            
        }
        ShowMenu = !ShowMenu;
    }
    public void PPButton()
    {
        if (pause_flag)
        {
            Time.timeScale = 1;
            worldspeed = prev_worldspeed;
        }
        else
        {
            prev_worldspeed = worldspeed;
            Time.timeScale = 0;
            worldspeed = 0;
        }
        FlagHouses = false;
        FlagCars = false;
        FlagEdges = false;
        FlagTrafficLights = false;
        FlagSpawn = false;
        pause_flag = !pause_flag;
        DelFlag = false;
    }
    public void Car()
    {
        FlagHouses = false;
        FlagCars = true;
        FlagEdges = false;
        FlagTrafficLights = false;
        TLCanvas.SetActive(false);
        RoadCanvas.SetActive(false);
        DelFlag = false;
    }
    public void Edge()
    {
        FlagHouses = false;
        FlagCars = false;
        FlagEdges = true;
        FlagTrafficLights = false;
        RoadCanvas.SetActive(true);
        TLCanvas.SetActive(false);
        DelFlag = false;
    }
    public void TrafficLight()
    {
        FlagHouses = false;
        FlagCars = false;
        FlagEdges = false;
        FlagTrafficLights = true;
        TLCanvas.SetActive(true);
        RoadCanvas.SetActive(false);
        DelFlag = false;
    }
    public void House()
    {
        FlagHouses = true;
        FlagCars = false;
        FlagEdges = false;
        FlagTrafficLights = false;
        RoadCanvas.SetActive(false);
        TLCanvas.SetActive(false);
        DelFlag = false;
    }

    public void SetAsphalt()
    {
        FlagAsphalt = true;
        FlagOffRoad = false;
        FlagStone = false;
    }

    public void SetStone()
    {
        FlagStone = true;
        FlagAsphalt = false;
        FlagOffRoad = false;
    }

    public void SetOffRoad()
    {
        FlagOffRoad = true;
        FlagStone = false;
        FlagAsphalt = false;
    }

    public void TrafficDisplay()
    {
        TLCanvas.SetActive(false);
        RoadCanvas.SetActive(false);
        if (!TrafficRepeat)
        {
            FlagTrafficColor = true;
            FlagTrafficSleep = false;
        }
        else
        {
            FlagTrafficColor = false;
            FlagTrafficSleep = true;
        }
        TrafficRepeat = !TrafficRepeat;
    }

    public void EditButton()
    {
        FlagHouses = false;
        FlagCars = false;
        FlagEdges = false;
        FlagTrafficLights = false;
        FlagSyncTL = false;
        FlagDesyncTL = false;
        DelFlag = false;
        if (FlagEdit)
        {
            CarOb.GetComponent<Button>().interactable = true;
            EdgOb.GetComponent<Button>().interactable = false;
            HOb.GetComponent<Button>().interactable = false;
            Sync.GetComponent<Button>().interactable = false;
            Desync.GetComponent<Button>().interactable = false;
            TrLOb.GetComponent<Button>().interactable = false;
            RoadCanvas.SetActive(false);
            TLCanvas.SetActive(false);
        }
        else
        {
            temp.GetComponent<create>().cars.Clear();
            temp.GetComponent<create>().cars_amount = 1;
            for (var i = GameObject.Find("cars").transform.childCount - 1; i >= 0; i--)
                Destroy(GameObject.Find("cars").transform.GetChild(i).gameObject);
            CarOb.GetComponent<Button>().interactable = false;
            EdgOb.GetComponent<Button>().interactable = true;
            HOb.GetComponent<Button>().interactable = true;
            TrLOb.GetComponent<Button>().interactable = true;
            Sync.GetComponent<Button>().interactable = true;
            Desync.GetComponent<Button>().interactable = true;
        }
        FlagEdit = !FlagEdit;
    }

    public void SyncTLButton()
    {
        FlagHouses = false;
        FlagCars = false;
        FlagEdges = false;
        FlagTrafficLights = false;
        FlagSyncTL = true;
        FlagDesyncTL = false;
        DelFlag = false;
    }
    public void DesyncTLButton()
    {
        FlagHouses = false;
        FlagCars = false;
        FlagEdges = false;
        FlagTrafficLights = false;
        FlagSyncTL = false;
        FlagDesyncTL = true;
        DelFlag = false;
    }
    public void IncreaseSpeed()
    {
        worldspeed += 0.1f;
        Decrease.GetComponent<Button>().interactable = true;
    }

    public void DecreaseSpeed()
    {
        if (worldspeed <= 0.1f)
        {
            worldspeed -= 0.1f;
            Decrease.GetComponent<Button>().interactable = false;
        }
        else
            worldspeed -= 0.1f;
    }

    public void Set_OneWay()
    {
        OneWay = true;
    }

    public void Set_TwoWay()
    {
        OneWay = false;
    }

    public void CarSpawn()
    {
        FlagHouses = false;
        FlagCars = false;
        FlagEdges = false;
        FlagTrafficLights = false;
        FlagSyncTL = false;
        FlagDesyncTL = false;
        FlagSpawn = true;
        DelFlag = false;
        TLCanvas.SetActive(false);
        RoadCanvas.SetActive(false);

    }
    public void DelButActive()
    {
        FlagHouses = false;
        FlagCars = false;
        FlagEdges = false;
        FlagTrafficLights = false;
        FlagSyncTL = false;
        FlagDesyncTL = false;
        FlagSpawn = false ;
        DelFlag = true;
        TLCanvas.SetActive(false);
        RoadCanvas.SetActive(false);
        
    }
    public void SetOnTPS()
    {
        TPS = true;
        FPS = false;
    }
    public void SetOnFPS()
    {
        FPS = true;
        TPS = false;
    }
    public void TryShit()
    {
        //GetComponent<Animation>().Play("HB"); //������������ �������� shit:1.
    }

}