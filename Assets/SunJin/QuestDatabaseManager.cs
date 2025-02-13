using UnityEngine;
using WebSocketSharp;
[System.Serializable]
public class QuestData
{
    public int quest_code;
    public bool quest_get_ballon_appears;
    public bool quest_get_condition;
    public bool quest_get;
    public bool quest_completion_ballon_appears;
    public bool quest_completion_condition;
    public bool quest_completion;
    public string quest_progress;
    public string[] quest_details;
}

[System.Serializable]
public class QuestResponse
{
    public QuestData[] data;
}
[System.Serializable]
public struct UpdateDatabase
{
    public string function;
    public int questCode;
    public bool newBool;
    public string newString;
    public string[] newStringArray;
}
public class QuestDatabaseManager : MonoBehaviour
{
    public QuestResponse serverData;
    private string[] functionNameStorage = { "UpdateQuestGetBallonAppears"
            ,"UpdateQuestGetCondition" ,"UpdateQuestGet" ,
        "UpdateQuestCompletionBallonAppears" ,"UpdateQuestCompletionCondition" ,
        "UpdateQuestCompletion","UpdateQuestProgress" ,"UpdateQuestDetails"};
    string[] test = {"테스트","되는지","확인용" };
    private WebSocket ws;
    #region UpdateDB
    public void SendUpdateBool(int function, int questCode, bool newBool)
    {
        var message = new UpdateDatabase
        {
            function = functionNameStorage[function],
            questCode = questCode,
            newBool = newBool
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
    }
    public void SendUpdateString(int function, int questCode, string newString)
    {
        var message = new UpdateDatabase
        {
            function = functionNameStorage[function],
            questCode = questCode,
            newString = newString
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
    }
    public void SendUpdateStringArray(int function, int questCode, string[] newStringArray)
    {
        var message = new UpdateDatabase
        {
            function = functionNameStorage[function],
            questCode = questCode,
            newStringArray = newStringArray
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
    }
    #endregion UpdateDB
    #region LoadDB

    private void Awake()
    {
        ws = new WebSocket("ws://localhost:7777");
        ws.Connect();
        ws.OnMessage += Call;
    }
    //private void Start()
    //{
    //    ws = new WebSocket("ws://localhost:7777");
    //    ws.Connect();
    //    ws.OnMessage += Call;
    //}

    private void Call(object sender, MessageEventArgs e)
    {
        serverData = JsonUtility.FromJson<QuestResponse>(e.Data);
    }
    #endregion

    #region SearchDB
    public void SelectQuestCode(int quest_code)
    {
        foreach(QuestData data in serverData.data)
        {
            if (data.quest_code == quest_code) { }
        }
    }

    public void SelectQuestGetBallonAppears(int questCode, bool quest_get_ballon_appears)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_get_ballon_appears == quest_get_ballon_appears) 
            {
                //if임시DB 퀘스트 코드 == questcode 일 때
                //   { 임시 DB 퀘스트 코드.quest_get_ballon_appears = quest_get_ballon_appears; }

                //여기서 DB데이터를 업데이트 해줍니다.
            }
        }
    }

    public void SelectQuestGetCondition(bool quest_get_condition)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_get_condition == quest_get_condition) { }
        }
    }

    public void SelectQuestGet(bool quest_get)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_get == quest_get) { }
        }
    }

    public void SelectQuestCompletionBallonAppears(bool quest_completion_ballon_appears)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_completion_ballon_appears == quest_completion_ballon_appears) { }
        }
    }

    public void SelectQuestCompletionCondition(bool quest_completion_condition)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_completion_condition == quest_completion_condition) { }
        }
    }

    public void SelectQuestCompletion(bool quest_completion)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_completion == quest_completion) { }
        }
    }

    public void SelectQuestProgress(string quest_progress)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_progress == quest_progress) { }
        }
    }

    public void SelectQuestDetails(string[] quest_details)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_details == quest_details) { }
        }
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendUpdateBool(0, 1, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SendUpdateBool(1, 1, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SendUpdateBool(2, 1, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SendUpdateBool(3, 1, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SendUpdateBool(4, 1, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SendUpdateBool(5, 1, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SendUpdateString(6, 1, "은근 많이함");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SendUpdateStringArray(7, 1, test);
        }
    }
}