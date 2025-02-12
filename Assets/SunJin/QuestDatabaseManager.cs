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
    public string action;
    public int questCode;
    public int newNpcCode;
}
public class QuestDatabaseManager : MonoBehaviour
{
    public static QuestResponse serverData;
    private static WebSocket ws;
    public static void SendUpdateNpcCode(int questCode, int newNpcCode)
    {
        var message = new UpdateDatabase
        {
            action = "UpdateNPCCode",
            questCode = questCode,
            newNpcCode = newNpcCode
        };

        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
    }

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
    public static void SelectQuestCode(int quest_code)
    {
        foreach(QuestData data in serverData.data)
        {
            if (data.quest_code == quest_code) { }
        }
    }

    public static void SelectQuestName(string quest_name)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_name == quest_name) { }
        }
    }

    public static void SelectQuestGetBallonAppears(bool quest_get_ballon_appears)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_get_ballon_appears == quest_get_ballon_appears) { }
        }
    }

    public static void SelectQuestGetCondition(bool quest_get_condition)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_get_condition == quest_get_condition) { }
        }
    }

    public static void SelectQuestGet(bool quest_get)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_get == quest_get) { }
        }
    }

    public static void SelectQuestCompletionBallonAppears(bool quest_completion_ballon_appears)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_completion_ballon_appears == quest_completion_ballon_appears) { }
        }
    }

    public static void SelectQuestCompletionCondition(bool quest_completion_condition)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_completion_condition == quest_completion_condition) { }
        }
    }

    public static void SelectQuestCompletion(bool quest_completion)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_completion == quest_completion) { }
        }
    }

    public static void SelectQuestProgress(string quest_progress)
    {
        foreach (QuestData data in serverData.data)
        {
            if (data.quest_progress == quest_progress) { }
        }
    }

    public static void SelectQuestDetails(string[] quest_details)
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
            SendUpdateNpcCode(1, 3);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ws.Send("Á¦ÀÌ¾´¾Æ´Ô");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
        }
    }
}