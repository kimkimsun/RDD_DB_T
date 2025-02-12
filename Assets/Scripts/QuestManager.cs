using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Analytics.IAnalytic;
using static UnityEngine.Rendering.DebugUI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public DialogueDB_Manager characterDB;


    public TextAsset tableCSV;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (characterDB == null)
        {
            characterDB = GetComponent<DialogueDB_Manager>();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            characterDB.csv_FileName = "Test_Dialogue";
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            characterDB.csv_FileName = "Test_Dialogue2";
        }
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TableSetter();
    }

    [System.Serializable]
    public class TableData
    {
        public int Tnpc_code;                       //NPC ÄÚµå

        public int Tquest_code;                     //Äù½ºÆ® ÄÚµå

        public bool TquestBaloon_On;                //Äù½ºÆ® È¹µæ ¸»Ç³¼± ÃâÇö bool

        public string TqusetGet_Condition;          //Äù½ºÆ® È¹µæ Á¶°Ç
    }

    List<int> npc_Code = new List<int>();
    List<int> quest_Code = new List<int>();
    List<bool> quest_baloonOn = new List<bool>();
    List<string> questGet_Condition = new List<string>();

    public List<TableData> TdataList = new List<TableData>();

    string[,] tables;
    public int lineSize;
    public int rowSize;

    public void TableSetter()
    {
        string currentText = tableCSV.text.Substring(0, tableCSV.text.Length - 1);
        string[] line = currentText.Split(new char[] { '\n' });
        lineSize = line.Length;
        rowSize = line[0].Split(new char[] { '\t' }).Length;
        tables = new string[lineSize, rowSize];

        for (int i = 0; i < lineSize; i++)
        {
            string[] row = line[i].Split(new char[] { '\t' });
            for (int j = 0; j < rowSize; j++)
            {
                tables[i, j] = row[j];
            }
        }

        bool on;
        for (int i = 1; i < lineSize; i++)
        {
            string[] data = tables[i, 0].Split(',');
            npc_Code.Add(int.Parse(data[0]));
            quest_Code.Add(int.Parse(data[1]));
            if (Boolean.TryParse(data[2], out on))
            {
                quest_baloonOn.Add(Boolean.Parse(data[2]));
            }
            else
            {
                Debug.Log("Boolean ÇüÀÌ ¾Æ´Õ´Ï´Ù.");
            }

            questGet_Condition.Add(data[3]);
        }

        for (int i = 0; i < npc_Code.Count; i++)
        {
            TableData tempData = new TableData();
            tempData.Tnpc_code = npc_Code[i];
            tempData.Tquest_code = quest_Code[i];
            tempData.TquestBaloon_On = quest_baloonOn[i];
            tempData.TqusetGet_Condition = questGet_Condition[i];
            TdataList.Add(tempData);
        }
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
