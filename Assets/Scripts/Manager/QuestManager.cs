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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QDBM = GetComponent<QuestDatabaseManager>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        TableCSVSetter();
        TableSetter();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TableCSVSetter();
        TableSetter();
    }

    public void TableCSVSetter()
    {
        //tableCSV�� ��� ������ �� �ۼ� �ʿ� + Dont destroy On Load ���� �ʿ�

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
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            csvName = "izlu_Quest_Table";
        }
        Debug.Log("ù��°");
    }

    //4. �ִ� NPC�� �ٶ� ��ȭ
    //5. �Ϸ� NPC�� �Ϸ� ��ȭ
    //6. �ִ� NPC�� ����Ʈ ���� �� ��ȭ
    //7. �Ϸ� NPC�� ����Ʈ ���� �� ��ȭ

    [System.Serializable]
    public class TableData
    {
        [Header("CSV Part")]
        public int Tquest_code;                     //1. ����Ʈ �ڵ�

        public string Tquest_name;                  //2. ����Ʈ �̸�

        public int TGivenpc_code;                   //3. �ִ�NPC �ڵ�

        public int TquestGet_index;                 //8. ����Ʈ �޴� ����

        public int TquestChoice_index;              //9. �������� ����

        public int TquestFinish_index;              //10. ����Ʈ �Ϸ� ����

        public int TquestTyping_index;              //11. ����Ʈ ���� ���� ����

        public Sprite TquestBaloon_UI;              //13. ����Ʈ ȹ�� ��ǳ�� UI Type

        public QCT TquestGet_Condition_Type;

        public int TqusetGet_Condition;          //����Ʈ ȹ�� ����(���� �Ÿ� ���� ������ �˻�)

        public int TquestReward_index;

        public string Tquest_Reward;

        //---------------------------------------------------------------------------------------
        [Header("DataBase Part")]
        public bool ballon_appears;

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
    List<int> npcGive_Code = new List<int>();
    List<int> questGet_index = new List<int>();
    List<int> questChoice_index = new List<int>();
    List<int> questFinish_index = new List<int>();
    List<int> questTyping_index = new List<int>();

    List<int> quest_GetbaloonUI = new List<int>();
    public List<Sprite> questUI = new List<Sprite>();
    
    List<int> questGet_Condition_Type = new List<int>();
    List<int> questGet_Condition = new List<int>();

    List<int> quest_Reward_index = new List<int>();
    List<string> quest_Reward = new List<string>();

    public List<TableData> TQuestdataList = new List<TableData>();

    string[,] tables;
    public int lineSize;
    public int rowSize;

    public void TableClear()
    {
        if (TQuestdataList.Count > 0)
        {
            TQuestdataList.Clear();
        }
        quest_Code.Clear();
        quest_Name.Clear();
        npcGive_Code.Clear();
        questGet_index.Clear();
        questChoice_index.Clear();
        questFinish_index.Clear();
        questTyping_index.Clear();
        quest_GetbaloonUI.Clear();
        questGet_Condition_Type.Clear();
        questGet_Condition.Clear();
        quest_Reward_index.Clear();
        quest_Reward.Clear();
        Debug.Log("Ŭ����");
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

        //Table�ʱ�ȭ �ϰ� ����
        TableClear();

        //bool on;
        for (int i = 1; i < lineSize; i++)
        {
            Debug.Log("�ٽ� �ֱ�");
            string[] data = tables[i, 0].Split(',');
            
            quest_Code.Add(int.Parse(data[0]));

            quest_Name.Add(data[1]);

            npcGive_Code.Add(int.Parse(data[2]));

            questGet_index.Add(int.Parse(data[3]));

            questChoice_index.Add(int.Parse(data[4]));

            questFinish_index.Add(int.Parse(data[5]));

            questTyping_index.Add(int.Parse(data[6]));

            quest_GetbaloonUI.Add(int.Parse(data[7]));

            questGet_Condition_Type.Add(int.Parse(data[8]));

            questGet_Condition.Add(int.Parse(data[9]));

            quest_Reward_index.Add(int.Parse(data[10]));
         
            quest_Reward.Add(data[11]);
        }

        //CSV�� �� Tdata_List�� �ֱ�
        for (int i = 0; i < QDBM.serverData.data.Length; i++)
        {
            TableData tempData = new TableData();
            tempData.Tquest_code = quest_Code[i];
            tempData.Tquest_name = quest_Name[i];
            tempData.TGivenpc_code = npcGive_Code[i];
            tempData.TquestGet_index = questGet_index[i];
            tempData.TquestChoice_index = questChoice_index[i];
            tempData.TquestFinish_index = questFinish_index[i];
            tempData.TquestTyping_index = questTyping_index[i];
            tempData.TquestBaloon_UI = questUI[quest_GetbaloonUI[i]];
            switch(questGet_Condition_Type[i])
            {
                case 1:
                    tempData.TquestGet_Condition_Type = QCT.LEVEL;
                    break;
                case 2:
                    tempData.TquestGet_Condition_Type = QCT.PREQUEST;
                    break;
            }
            tempData.TqusetGet_Condition = questGet_Condition[i];
            tempData.TquestReward_index = quest_Reward_index[i];
            tempData.Tquest_Reward = quest_Reward[i];
            TQuestdataList.Add(tempData);
        }

        //DB�� �� Tdata_List�� �߰��� �ֱ�
        for (int i = 0; i < QDBM.serverData.data.Length; i++)
        {
            TQuestdataList[i].ballon_appears = QDBM.serverData.data[i].ballon_appears;
            TQuestdataList[i].quest_get_condition = QDBM.serverData.data[i].quest_get_condition;
            TQuestdataList[i].quest_get = QDBM.serverData.data[i].quest_get;
            TQuestdataList[i].chain_quest_get_code = QDBM.serverData.data[i].chain_quest_get_code;
            TQuestdataList[i].chain_quest_completion_code = QDBM.serverData.data[i].chain_quest_completion_code;
            TQuestdataList[i].quest_complete_condition = QDBM.serverData.data[i].quest_completion_condition;
            TQuestdataList[i].quest_completion = QDBM.serverData.data[i].quest_completion;
            TQuestdataList[i].quest_progress = QDBM.serverData.data[i].quest_progress;
            TQuestdataList[i].quest_details = QDBM.serverData.data[i].quest_details;
        }
        Debug.Log("���̺�");
    }

   // Update is called once per frame
    //void Update()
    //{

    //}
}
