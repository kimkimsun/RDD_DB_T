using UnityEngine;
using WebSocketSharp;
[System.Serializable]
public class QuestData
{
    public int quest_code;
    public int npc_code;
}

[System.Serializable]
public class QuestResponse
{
    public QuestData[] data;
}
[System.Serializable]
public class UpdateNpcCodeMessage
{
    public string action;
    public int questCode;
    public int newNpcCode;
}
public class QuestDatabaseManager : MonoBehaviour
{
    WebSocket ws;
    private void Start()
    {
        ws = new WebSocket("ws://localhost:7777");
        //서버에서 설정한 포트를 넣어줍니다.
        Debug.Log("여긴가?");
        ws.Connect();
        Debug.Log("여긴가222?");
        //연결합니다.
        ws.OnMessage += Call;
    }

    void Call(object sender, MessageEventArgs e)
    {
        var response = JsonUtility.FromJson<QuestResponse>(e.Data);

        if (response.data.Length > 0)
        {
            int questCode = response.data[0].quest_code;
            int npcCode = response.data[0].npc_code;
            Debug.Log("quest_code: " + questCode);
            Debug.Log("npc_code: " + npcCode);
        }
        else
        {
            Debug.Log("결과가 없습니다.");
        }
    }
    public void SendUpdateNpcCode(int questCode, int newNpcCode)
    {
        var message = new UpdateNpcCodeMessage
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            SendUpdateNpcCode(1, 3);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ws.Send("제이쓴아님");
        }
    }
}