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
    
    private void Start()
    {
        ws = new WebSocket("ws://localhost:7777");
        ws.Connect();
        ws.OnMessage += Call;
    }

    private void Call(object sender, MessageEventArgs e)
    {
        serverData = JsonUtility.FromJson<QuestResponse>(e.Data);
        Debug.Log(serverData.data[1].quest_code);
    }

    public static void SelectFromTable(int quest_code,int selectData)
    {
        foreach(QuestData data in serverData.data)
        {
            if (data.quest_code == quest_code)
                Debug.Log(data.quest_code);
        }
    }

    public static void SendUpdateNpcCode(int questCode, int newNpcCode)
    {
        var message = new UpdateDatabase
        {
            action = "updateNpcCode",
            questCode = questCode,
            newNpcCode = newNpcCode
        };

        var jsonMessage = JsonUtility.ToJson(message);
        Debug.Log("보낼 메시지: " + jsonMessage); // 메시지가 제대로 생성되는지 확인
        ws.Send(jsonMessage);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SendUpdateNpcCode(1, 3);
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