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

        public string Tquest_contents;              //3. ����Ʈ ����

        public int TGivenpc_code;                   //3. �ִ�NPC �ڵ�

        public int TFinishnpc_code;                 //4. �Ϸ�NPC �ڵ�

        public int TquestGet_index;                 //8. ����Ʈ �޴� ����

        public int TquestChoice_index;              //9. �������� ����

        public int TquestFinish_index;              //10. ����Ʈ �Ϸ� ����

        public int TquestTyping_index;              //11. ����Ʈ ���� ���� ����

        public Sprite TquestBaloon_UI;              //13. ����Ʈ ȹ�� ��ǳ�� UI Type

        public string TqusetGet_Condition;          //����Ʈ ȹ�� ����(���� �Ÿ� ���� ������ �˻�)

        //---------------------------------------------------------------------------------------
        [Header("DataBase Part")]
        public bool DBquest_get_baloon;

        public bool DBquest_get_condition;

        public bool DBquest_get;

        public bool DBquest_complete_baloon;

        public bool DBquest_complete_condition;

        public bool DBquest_complete;

        public string DBquest_progress;

        public string[] DBquest_detail;
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
        Debug.Log("��");
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
            //    Debug.Log("Boolean ���� �ƴմϴ�.");
            //}

            quest_GetbaloonUI.Add(int.Parse(data[9]));

            questGet_Condition.Add(data[10]);
        }

        TableData tempData = new TableData();

        //CSV�� �� Tdata_List�� �ֱ�
        for (int i = 0; i < QDBM.serverData.data.Length/*quest_Code.Count*/; i++)
        {
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

            //TdataList.Add(tempData);
        }

        //DB�� �� Tdata_List�� �ֱ�
        for (int i = 0; i < QDBM.serverData.data.Length; i++)
        {
            tempData.DBquest_get_baloon = QDBM.serverData.data[i].quest_get_ballon_appears;
            tempData.DBquest_get_condition = QDBM.serverData.data[i].quest_get_condition;
            tempData.DBquest_get = QDBM.serverData.data[i].quest_get;
            tempData.DBquest_complete_baloon = QDBM.serverData.data[i].quest_completion_ballon_appears;
            tempData.DBquest_complete_condition = QDBM.serverData.data[i].quest_completion_condition;
            tempData.DBquest_complete = QDBM.serverData.data[i].quest_completion;
            tempData.DBquest_progress = QDBM.serverData.data[i].quest_progress;
            tempData.DBquest_detail = QDBM.serverData.data[i].quest_details;
        }


        TdataList.Add(tempData);
    }

   // Update is called once per frame
    void Update()
    {

    }
}
