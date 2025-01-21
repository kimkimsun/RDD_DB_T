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
public class WsClient : MonoBehaviour
{
    WebSocket ws;
    private void Start()
    {
        ws = new WebSocket("ws://localhost:7777");
        //서버에서 설정한 포트를 넣어줍니다.

        ws.Connect();

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
    private void Update()
    {
        if (ws == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("abcd");
            //데이터를 보냅니다 예제이기 때문에 "abcd" 를 보냅니다
        }
    }
}