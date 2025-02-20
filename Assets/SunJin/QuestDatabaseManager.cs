using UnityEngine;
using WebSocketSharp;
[System.Serializable]
public class QuestData
{
    public int quest_code;
    public int npc_code;
    public bool ballon_appears;
    public bool quest_get_condition;
    public bool quest_get;
    public int chain_quest_get_code;
    public bool quest_completion_condition;
    public bool quest_completion;
    public int chain_quest_completion_code;
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
    private string[] functionNameStorage = { "UpdateBallonAppears"
            ,"UpdateQuestGetCondition" ,"UpdateQuestGet" ,
        "UpdateQuestCompletionCondition" ,"UpdateQuestCompletion" ,
        "UpdateQuestProgress","UpdateQuestDetails" ,"UpdateQuestDetails"};
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
        Debug.Log("¶Ç¶ß´Ï?");
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

}