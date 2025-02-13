using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public DialogueDB_Manager characterDB;

    public QuestDatabaseManager QDBM;

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
        QDBM = GetComponent<QuestDatabaseManager>();
        TableSetter();
    }

    //4. 주는 NPC의 줄때 대화
    //5. 완료 NPC의 완료 대화
    //6. 주는 NPC의 퀘스트 진행 중 대화
    //7. 완료 NPC의 퀘스트 진행 중 대화

    [System.Serializable]
    public class TableData
    {
        [Header("CSV Part")]
        public int Tquest_code;                     //1. 퀘스트 코드

        public string Tquest_name;                  //2. 퀘스트 이름

        public string Tquest_contents;              //3. 퀘스트 내용

        public int TGivenpc_code;                   //3. 주는NPC 코드

        public int TFinishnpc_code;                 //4. 완료NPC 코드

        public int TquestGet_index;                 //8. 퀘스트 받는 시점

        public int TquestChoice_index;              //9. 다지선다 시점

        public int TquestFinish_index;              //10. 퀘스트 완료 시점

        public int TquestTyping_index;              //11. 퀘스트 따라 쓰기 시점

        public Sprite TquestBaloon_UI;              //13. 퀘스트 획득 말풍선 UI Type

        public string TqusetGet_Condition;          //퀘스트 획득 조건(일정 거리 내에 들어오면 검사)

        //---------------------------------------------------------------------------------------
        [Header("DataBase Part")]
        public bool quest_get;
    }

    List<int> quest_Code = new List<int>();
    List<string> quest_Name = new List<string>();
    List<string> quest_contents = new List<string>();
    List<int> npcGive_Code = new List<int>();
    List<int> npcFinish_Code = new List<int>();
    List<int> questGet_index = new List<int>();
    List<int> questChoice_index = new List<int>();
    List<int> questFinish_index = new List<int>();
    List<int> questTyping_index = new List<int>();

    List<int> quest_GetbaloonUI = new List<int>();
    public List<Sprite> questUI = new List<Sprite>();

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

        //bool on;
        for (int i = 1; i < lineSize; i++)
        {
            string[] data = tables[i, 0].Split(',');

            quest_Code.Add(int.Parse(data[0]));

            quest_Name.Add(data[1]);

            quest_contents.Add(data[2]);

            npcGive_Code.Add(int.Parse(data[3]));

            npcFinish_Code.Add(int.Parse(data[4]));

            questGet_index.Add(int.Parse(data[5]));

            questChoice_index.Add(int.Parse(data[6]));

            questFinish_index.Add(int.Parse(data[7]));

            questTyping_index.Add(int.Parse(data[8]));

            //if (Boolean.TryParse(data[2], out on))
            //{
            //    quest_baloonOn.Add(Boolean.Parse(data[2]));
            //}
            //else
            //{
            //    Debug.Log("Boolean 형이 아닙니다.");
            //}

            quest_GetbaloonUI.Add(int.Parse(data[9]));

            questGet_Condition.Add(data[10]);
        }

        for (int i = 0; i < quest_Code.Count; i++)
        {
            TableData tempData = new TableData();
            tempData.Tquest_code = quest_Code[i];
            tempData.Tquest_name = quest_Name[i];
            tempData.Tquest_contents = quest_contents[i];
            tempData.TGivenpc_code = npcGive_Code[i];
            tempData.TFinishnpc_code = npcFinish_Code[i];
            tempData.TquestGet_index = questGet_index[i];
            tempData.TquestChoice_index = questChoice_index[i];
            tempData.TquestFinish_index = questFinish_index[i];
            tempData.TquestTyping_index = questTyping_index[i];
            tempData.TquestBaloon_UI = questUI[quest_GetbaloonUI[i]];
            tempData.TqusetGet_Condition = questGet_Condition[i];

            //tempData.quest_get = QDBM.serverData.data[i].quest_get;

            // QDBM.serverData.data[i].quest_name = quest_Name[i];

            TdataList.Add(tempData);
        }
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
