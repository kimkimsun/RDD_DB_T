using UnityEngine;
using WebSocketSharp;
[System.Serializable]
public class QuestData
{
    public int quest_code;
    public string quest_name;
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
    private WebSocket ws;
    #region UpdateDB
    public void SendUpdateBool(string function, int questCode, bool newBool)
    {
        var message = new UpdateDatabase
        {
            function = function,
            questCode = questCode,
            newBool = newBool
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
    }
    public void SendUpdateString(string function, int questCode, string newString)
    {
        var message = new UpdateDatabase
        {
            function = function,
            questCode = questCode,
            newString = newString
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
    }
    public void SendUpdateStringArray(string function, int questCode, string[] newStringArray)
    {
        var message = new UpdateDatabase
        {
            function = function,
            questCode = questCode,
            newStringArray = newStringArray
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
    }
    #endregion UpdateDB
    #region LoadDB
    private void Start()
    {
        ws = new WebSocket("ws://localhost:7777");
        ws.Connect();
        ws.OnMessage += Call;
    }

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

    public void SelectQuestName(string quest_name)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_name == quest_name) { }
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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SendUpdateBool("UpdateQuestGetBallonAppears", 1, false);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ws.Send("제이쓴아님");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
        }
    }
}