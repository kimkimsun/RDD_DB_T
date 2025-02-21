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
    private QuestManager qmInstance;
    private WebSocket ws;
    #region UpdateDB
    public void SendUpdateBallonAppears(int questCode, bool newBool)
    {
        var message = new UpdateDatabase
        {
            function = "UpdateBallonAppears",
            questCode = questCode,
            newBool = newBool
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
        serverData.data[questCode].ballon_appears = newBool;
        int qc = FindListSameQC(questCode);
        if(qc != -1)
            qmInstance.questdataList[qc].ballon_appears = newBool;
    }
    public void SendUpdateQuestGetCondition(int questCode, bool newBool)
    {
        var message = new UpdateDatabase
        {
            function = "UpdateQuestGetCondition",
            questCode = questCode,
            newBool = newBool
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
        serverData.data[questCode].quest_get_condition = newBool;
        int qc = FindListSameQC(questCode);
        if (qc != -1)
            qmInstance.questdataList[qc].quest_get_condition = newBool;
    }
    public void SendUpdateQuestGet(int questCode, bool newBool)
    {
        var message = new UpdateDatabase
        {
            function = "UpdateQuestGet",
            questCode = questCode,
            newBool = newBool
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
        serverData.data[questCode].quest_get = newBool;
        int qc = FindListSameQC(questCode);
        if (qc != -1)
            qmInstance.questdataList[qc].quest_get = newBool;
    }
    public void SendUpdatQuestCompletionCondition(int questCode, bool newBool)
    {
        var message = new UpdateDatabase
        {
            function = "UpdateQuestCompletionCondition",
            questCode = questCode,
            newBool = newBool
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
        serverData.data[questCode].quest_completion_condition = newBool;
        int qc = FindListSameQC(questCode);
        if (qc != -1)
            qmInstance.questdataList[qc].quest_complete_condition = newBool;
    }
    public void SendUpdatQuestCompletion(int questCode, bool newBool)
    {
        var message = new UpdateDatabase
        {
            function = "UpdateQuestCompletion",
            questCode = questCode,
            newBool = newBool
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
        serverData.data[questCode].quest_completion = newBool;
        int qc = FindListSameQC(questCode);
        if (qc != -1)
            qmInstance.questdataList[qc].quest_completion = newBool;
    }
    public void SendUpdatQuestProgress(int questCode, string newString)
    {
        var message = new UpdateDatabase
        {
            function = "UpdateQuestProgress",
            questCode = questCode,
            newString = newString
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
        serverData.data[questCode].quest_progress= newString;
        int qc = FindListSameQC(questCode);
        if (qc != -1)
            qmInstance.questdataList[qc].quest_progress = newString;
    }
    public void SendUpdateQuestDetails(int questCode, string[] newStringArray)
    {
        var message = new UpdateDatabase
        {
            function = "UpdateQuestDetails",
            questCode = questCode,
            newStringArray = newStringArray
        };
        var jsonMessage = JsonUtility.ToJson(message);
        ws.Send(jsonMessage);
        serverData.data[questCode].quest_details = newStringArray;
        int qc = FindListSameQC(questCode);
        if (qc != -1)
            qmInstance.questdataList[qc].quest_details = newStringArray;
    }

    public int FindListSameQC(int questCode)
    {
        for(int i = 0; i < qmInstance.questdataList.Count; i++)
        {
            if (qmInstance.questdataList[i].quest_code == questCode)
                return i;
        }
        return -1;
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
    private void Call(object sender, MessageEventArgs e)
    {
        serverData = JsonUtility.FromJson<QuestResponse>(e.Data);
    }
    private void Start()
    {
        qmInstance = QuestManager.instance;
    }
    #endregion

}