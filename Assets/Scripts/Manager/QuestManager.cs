//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;


//public enum QCT
//{
//    LEVEL,
//    PREQUEST,
//}

//public class QuestManager : MonoBehaviour
//{
//    public static QuestManager instance;
//    //public DialogueDB_Manager characterDB;

//    public QuestDatabaseManager QDBM;

//    public TextAsset tableCSV;
//    public string csvName;

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }

//        var obj = FindObjectsOfType<QuestManager>();

//        if (obj.Length == 1)
//        {
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }

//    }

//    public bool firstLoad;

//    void Start()
//    {
//        QDBM = GetComponent<QuestDatabaseManager>();

//        if (!firstLoad)
//        {
//            firstLoad = true;
//            TableCSVSetter();
//            TableSetter();
//        }
//        else
//        {
//            SceneManager.sceneLoaded += OnSceneLoaded;
//        }

//    }

//    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        TableCSVSetter();
//        TableSetter();
//    }

//    public void TableCSVSetter()
//    {
//        //tableCSV값 어떻게 정해줄 지 작성 필요 + Dont destroy On Load 제작 필요

//        if (SceneManager.GetActiveScene().buildIndex == 0)
//        {
//            csvName = "island_Quest_Table";
//        }
//        else if (SceneManager.GetActiveScene().buildIndex == 1)
//        {
//            csvName = "island_Quest_Table";
//        }
//        else if (SceneManager.GetActiveScene().buildIndex == 2)
//        {
//            csvName = "island_Quest_Table";
//        }
//        //Debug.Log("첫번째");
//    }

//    //4. 주는 NPC의 줄때 대화
//    //5. 완료 NPC의 완료 대화
//    //6. 주는 NPC의 퀘스트 진행 중 대화
//    //7. 완료 NPC의 퀘스트 진행 중 대화

//    [System.Serializable]
//    public class TableData
//    {
//        //-----------------------------------CSV에서 긁어오는 데이터-----------------------------------
//        [Header("CSV Part")]

//        public int quest_code;                     //0. 퀘스트 코드

//        public string quest_name;                  //1. 퀘스트 이름

//        public int questGet_index;                 //2. 퀘스트 받는 시점

//        public int questChoice_index;              //3. 다지선다 시점

//        public int questTyping_index;              //4. 퀘스트 따라 쓰기 시점

//        public Sprite questBaloon_UI;              //5. 퀘스트 획득 말풍선 UI Type

//        public Sprite questIcon_UI;                //6. 퀘스트 받았을 때 panel에 뜨는 ICON TYPE

//        public QCT questGet_Condition_Type;        //7. 퀘스트를 받는 조건 ex) Level, Job

//        public int qusetGet_Condition;             //8. 퀘스트를 받을 때 필요한 변수 Level이면 안에 5를 표기

//        public int questReward_index;              //9. 퀘스트 보상이 발생하는 시점

//        public string quest_Reward;                //10. 퀘스트 보상

//        public string quest_title;                 //11. 퀘스트 제목 (title로 통합 할 예정)

//        public string quest_describe;              //12. 퀘스트 설명

//        //-----------------------------------DB에서 긁어오는 데이터-----------------------------------
//        [Header("DataBase Part")]
//        public bool ballon_appears;                //0. 퀘스트 말풍선 출현?

//        public int npc_code;                       //1. NPC코드 CSV와 접점을 위함

//        public bool quest_get_condition;           //2. 퀘스트 받는 조건 달성 여부 

//        public bool quest_get;                     //3. 퀘스트 받았는지에 대한 여부

//        public bool quest_complete_condition;      //4. 퀘스트 완료 조건 달성 여부

//        public bool quest_completion;              //5. 퀘스트 완료 여부

//        public string quest_progress = null;       //6. 퀘스트 진행도

//        public string[] quest_details = null;      //7. 퀘스트 Detail 열었을 때 나오는 거
//    }

//    List<int> quest_Code = new List<int>();
//    List<string> quest_Name = new List<string>();
//    List<int> questGet_index = new List<int>();
//    List<int> questChoice_index = new List<int>();
//    List<int> questTyping_index = new List<int>();

//    List<int> quest_GetbaloonUI = new List<int>();
//    public List<Sprite> questUI = new List<Sprite>();
//    List<int> quest_icon_UI = new List<int>();
//    public List<Sprite> questIconUI = new List<Sprite>();

//    List<int> questGet_Condition_Type = new List<int>();
//    List<int> questGet_Condition = new List<int>();

//    List<int> quest_Reward_index = new List<int>();
//    List<string> quest_Reward = new List<string>();
//    List<string> quest_title = new List<string>();
//    List<string> quest_describe = new List<string>();


//    public List<TableData> questdataList = new List<TableData>();

//    string[,] tables;
//    public int lineSize;
//    public int rowSize;

//    public void TableClear()
//    {
//        if (questdataList.Count > 0)
//        {
//            questdataList.Clear();
//        }
//        quest_Code.Clear();
//        quest_Name.Clear();
//        questGet_index.Clear();
//        questChoice_index.Clear();
//        questTyping_index.Clear();
//        quest_GetbaloonUI.Clear();
//        questGet_Condition_Type.Clear();
//        questGet_Condition.Clear();
//        quest_Reward_index.Clear();
//        quest_Reward.Clear();
//        quest_title.Clear();
//        quest_describe.Clear();
//    }

//    public void TableSetter()
//    {
//        tableCSV = Resources.Load<TextAsset>(csvName);
//        string currentText = tableCSV.text.Substring(0, tableCSV.text.Length - 1);
//        string[] line = currentText.Split(new char[] { '\n' });
//        lineSize = line.Length;
//        rowSize = line[0].Split(new char[] { '\t' }).Length;
//        tables = new string[lineSize, rowSize];

//        for (int i = 0; i < lineSize; i++)
//        {
//            string[] row = line[i].Split(new char[] { '\t' });
//            for (int j = 0; j < rowSize; j++)
//            {
//                tables[i, j] = row[j];
//            }
//        }

//        //Table초기화 하고 시작
//        TableClear();

//        //bool on;
//        for (int i = 1; i < lineSize; i++)
//        {
//            //Debug.Log("다시 넣기");
//            string[] data = tables[i, 0].Split(',');

//            quest_Code.Add(int.Parse(data[0]));

//            quest_Name.Add(data[1]);

//            questGet_index.Add(int.Parse(data[2]));

//            questChoice_index.Add(int.Parse(data[3]));

//            questTyping_index.Add(int.Parse(data[5]));

//            quest_GetbaloonUI.Add(int.Parse(data[6]));

//            quest_icon_UI.Add(int.Parse(data[7]));

//            questGet_Condition_Type.Add(int.Parse(data[8]));

//            questGet_Condition.Add(int.Parse(data[9]));

//            quest_Reward_index.Add(int.Parse(data[10]));

//            quest_Reward.Add(data[11]);

//            quest_title.Add(data[12]);

//            quest_describe.Add(data[13]);
//        }
//        //CSV의 값 Tdata_List에 넣기
//        for (int i = 0; i < lineSize - 1; i++)
//        {
//            TableData tempData = new TableData();
//            tempData.quest_code = quest_Code[i];
//            tempData.quest_name = quest_Name[i];
//            tempData.questGet_index = questGet_index[i];
//            tempData.questChoice_index = questChoice_index[i];
//            tempData.questTyping_index = questTyping_index[i];
//            tempData.questBaloon_UI = questUI[quest_GetbaloonUI[i]];
//            tempData.questIcon_UI = questIconUI[quest_icon_UI[i]];
//            switch (questGet_Condition_Type[i])
//            {
//                case 1:
//                    tempData.questGet_Condition_Type = QCT.LEVEL;
//                    break;
//                case 2:
//                    tempData.questGet_Condition_Type = QCT.PREQUEST;
//                    break;
//            }
//            tempData.qusetGet_Condition = questGet_Condition[i];
//            tempData.questReward_index = quest_Reward_index[i];
//            tempData.quest_Reward = quest_Reward[i];
//            tempData.quest_title = quest_title[i];
//            tempData.quest_describe = quest_describe[i];
//            questdataList.Add(tempData);
//        }

//        //DB의 값 Tdata_List에 추가로 넣기
//        for (int i = 0; i < questdataList.Count; i++)
//        {
//            for (int j = 0; j < QDBM.serverData.data.Length; j++)
//            {
//                if (questdataList[i].quest_code == QDBM.serverData.data[j].quest_code)
//                {
//                    questdataList[i].npc_code = QDBM.serverData.data[j].npc_code;
//                    questdataList[i].ballon_appears = QDBM.serverData.data[j].ballon_appears;
//                    questdataList[i].quest_get_condition = QDBM.serverData.data[j].quest_get_condition;
//                    questdataList[i].quest_get = QDBM.serverData.data[j].quest_get;
//                    questdataList[i].quest_complete_condition = QDBM.serverData.data[j].quest_completion_condition;
//                    questdataList[i].quest_completion = QDBM.serverData.data[j].quest_completion;
//                    questdataList[i].quest_progress = QDBM.serverData.data[j].quest_progress;
//                    questdataList[i].quest_details = QDBM.serverData.data[j].quest_details;
//                }
//            }
//        }
//    }
//}
