using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using SmartDLL;

public class LoadData1 : MonoBehaviour
{
    GameObject temp;
    public SmartFileExplorer fileExplorer = new SmartFileExplorer();
    void Start()
    {
        temp = GameObject.Find("path");
    }    
    public void LoadData()
    {
        Destroy(temp);
        temp = Instantiate(Resources.Load<GameObject>("path"), Vector3.zero, Quaternion.identity);
        temp.name = "path";
        string initialDir = Application.dataPath + "/Saves";
        bool restoreDir = true;
        string title = "Choose file";
        string defExt = "gra";
        string filter = "gra files (*.gra)|*.gra";
        fileExplorer.OpenExplorer(initialDir,restoreDir,title,defExt,filter);
        string path = fileExplorer.fileName;
        if (path != null)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                line = reader.ReadLine();
                int n = Int32.Parse(line);
                for(int i = 0; i< n; i++)
                {
                    line = reader.ReadLine();
                    temp.GetComponent<create>().SpawnNode(Vector3.zero, line);
                }
                line = reader.ReadLine();
                n = Int32.Parse(line);
                for (int i = 0; i < n; i++)
                {
                    line = reader.ReadLine();
                    temp.GetComponent<create>().SpawnEdge(0,0,null, line);
                }
                line = reader.ReadLine();
                n = Int32.Parse(line);
                for (int i = 0; i < n; i++)
                {
                    line = reader.ReadLine();
                    temp.GetComponent<create>().SpawnHouse(line);
                }
                line = reader.ReadLine();
                n = Int32.Parse(line);
                for (int i = 0; i < n; i++)
                {
                    line = reader.ReadLine();
                    temp.GetComponent<create>().SpawnTrafficLight(line);
                }
                line = reader.ReadLine();
                temp.GetComponent<create>().NumberIfInstance = Int32.Parse(line);

            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (temp == null)
            temp = GameObject.Find("path");
    }
}
