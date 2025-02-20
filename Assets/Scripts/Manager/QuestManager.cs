using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public enum QCT
{
    LEVEL,
    PREQUEST,
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    //public DialogueDB_Manager characterDB;

    public QuestDatabaseManager QDBM;

    public TextAsset tableCSV;
    public string csvName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        var obj = FindObjectsOfType<QuestManager>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public bool firstLoad;

    void Start()
    {
        QDBM = GetComponent<QuestDatabaseManager>();
        
        if(!firstLoad)
        {
            firstLoad = true;
            TableCSVSetter();
            TableSetter();
        }
        else
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TableCSVSetter();
        TableSetter();
    }

    public void TableCSVSetter()
    {
        //tableCSV값 어떻게 정해줄 지 작성 필요 + Dont destroy On Load 제작 필요

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            csvName = "island_Quest_Table";
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            csvName = "island_Quest_Table";
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            csvName = "island_Quest_Table";
        }
        //Debug.Log("첫번째");
    }

    //4. 주는 NPC의 줄때 대화
    //5. 완료 NPC의 완료 대화
    //6. 주는 NPC의 퀘스트 진행 중 대화
    //7. 완료 NPC의 퀘스트 진행 중 대화

    [System.Serializable]
    public class TableData
    {
        [Header("CSV Part")]

        public int quest_code;                     //1. 퀘스트 코드

        public string quest_name;                  //2. 퀘스트 이름

        public int questGet_index;                 //8. 퀘스트 받는 시점

        public int questChoice_index;              //9. 다지선다 시점

        public int questFinish_index;              //10. 퀘스트 완료 시점

        public int questTyping_index;              //11. 퀘스트 따라 쓰기 시점

        public Sprite questBaloon_UI;              //13. 퀘스트 획득 말풍선 UI Type

        public Sprite questIcon_UI;

        public QCT questGet_Condition_Type;

        public int qusetGet_Condition;          //퀘스트 획득 조건(일정 거리 내에 들어오면 검사)

        public int questReward_index;

        public string quest_Reward;

        //---------------------------------------------------------------------------------------
        [Header("DataBase Part")]
        public bool ballon_appears;
        
        public int npc_code;                   //3. 주는NPC 코드

        public bool quest_get_condition;

        public bool quest_get;

        //public int NPC_Code;
        //public bool DBquest_complete_baloon;
        public int chain_quest_get_code;

        public bool quest_complete_condition;

        public bool quest_completion;

        public int chain_quest_completion_code;

        public string quest_progress = null;

        public string[] quest_details = null;

    }

    List<int> quest_Code = new List<int>();
    List<string> quest_Name = new List<string>();
    List<int> questGet_index = new List<int>();
    List<int> questChoice_index = new List<int>();
    List<int> questFinish_index = new List<int>();
    List<int> questTyping_index = new List<int>();

    List<int> quest_GetbaloonUI = new List<int>();
    public List<Sprite> questUI = new List<Sprite>();
    List<int> quest_icon_UI = new List<int>();
    public List<Sprite> questIconUI = new List<Sprite>();
    
    List<int> questGet_Condition_Type = new List<int>();
    List<int> questGet_Condition = new List<int>();

    List<int> quest_Reward_index = new List<int>();
    List<string> quest_Reward = new List<string>();

    public List<TableData> questdataList = new List<TableData>();

    string[,] tables;
    public int lineSize;
    public int rowSize;

    public void TableClear()
    {
        if (questdataList.Count > 0)
        {
            questdataList.Clear();
        }
        quest_Code.Clear();
        quest_Name.Clear();
        questGet_index.Clear();
        questChoice_index.Clear();
        questFinish_index.Clear();
        questTyping_index.Clear();
        quest_GetbaloonUI.Clear();
        questGet_Condition_Type.Clear();
        questGet_Condition.Clear();
        quest_Reward_index.Clear();
        quest_Reward.Clear();
        //Debug.Log("클리어");
    }

    public void TableSetter()
    {
        tableCSV = Resources.Load<TextAsset>(csvName);
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

        //Table초기화 하고 시작
        TableClear();

        //bool on;
        for (int i = 1; i < lineSize; i++)
        {
            //Debug.Log("다시 넣기");
            string[] data = tables[i, 0].Split(',');
            
            quest_Code.Add(int.Parse(data[0]));

            quest_Name.Add(data[1]);

            questGet_index.Add(int.Parse(data[2]));

            questChoice_index.Add(int.Parse(data[3]));

            questFinish_index.Add(int.Parse(data[4]));

            questTyping_index.Add(int.Parse(data[5]));

            quest_GetbaloonUI.Add(int.Parse(data[6]));

            quest_icon_UI.Add(int.Parse(data[7]));

            questGet_Condition_Type.Add(int.Parse(data[8]));

            questGet_Condition.Add(int.Parse(data[9]));

            quest_Reward_index.Add(int.Parse(data[10]));
         
            quest_Reward.Add(data[11]);
        }
        //CSV의 값 Tdata_List에 넣기
        for (int i = 0; i < lineSize - 1; i++)
        {
            TableData tempData = new TableData();
            tempData.quest_code = quest_Code[i];
            tempData.quest_name = quest_Name[i];
            tempData.questGet_index = questGet_index[i];
            tempData.questChoice_index = questChoice_index[i];
            tempData.questFinish_index = questFinish_index[i];
            tempData.questTyping_index = questTyping_index[i];
            tempData.questBaloon_UI = questUI[quest_GetbaloonUI[i]];
            tempData.questIcon_UI = questIconUI[quest_icon_UI[i]];
            switch (questGet_Condition_Type[i])
            {
                case 1:
                    tempData.questGet_Condition_Type = QCT.LEVEL;
                    break;
                case 2:
                    tempData.questGet_Condition_Type = QCT.PREQUEST;
                    break;
            }
            tempData.qusetGet_Condition = questGet_Condition[i];
            tempData.questReward_index = quest_Reward_index[i];
            tempData.quest_Reward = quest_Reward[i];
            questdataList.Add(tempData);
        }

        //DB의 값 Tdata_List에 추가로 넣기
        for (int i = 0; i < questdataList.Count; i++)
        {
            for (int j = 0; j < QDBM.serverData.data.Length; j++) 
            {
                if(questdataList[i].quest_code == QDBM.serverData.data[j].quest_code)
                {
                    questdataList[i].npc_code = QDBM.serverData.data[j].npc_code;
                    questdataList[i].ballon_appears = QDBM.serverData.data[j].ballon_appears;
                    questdataList[i].quest_get_condition = QDBM.serverData.data[j].quest_get_condition;
                    questdataList[i].quest_get = QDBM.serverData.data[j].quest_get;
                    questdataList[i].chain_quest_get_code = QDBM.serverData.data[j].chain_quest_get_code;
                    questdataList[i].chain_quest_completion_code = QDBM.serverData.data[j].chain_quest_completion_code;
                    questdataList[i].quest_complete_condition = QDBM.serverData.data[j].quest_completion_condition;
                    questdataList[i].quest_completion = QDBM.serverData.data[j].quest_completion;
                    questdataList[i].quest_progress = QDBM.serverData.data[j].quest_progress;
                    questdataList[i].quest_details = QDBM.serverData.data[j].quest_details;
                }
            }
        }
    }
}