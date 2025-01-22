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
        //�������� ������ ��Ʈ�� �־��ݴϴ�.

        ws.Connect();

        //�����մϴ�.
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
            Debug.Log("����� �����ϴ�.");
        }
    }
    public void SendUpdateNpcCode(int questCode, int newNpcCode)
    {
        var message = JsonUtility.ToJson(new { action = "updateNpcCode", questCode, newNpcCode });
        Debug.Log("���� �޽���: " + message); // �޽����� ����� �����Ǵ��� Ȯ��
        ws.Send(message);
    }
    private void Update()
    {
        if (ws == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SendUpdateNpcCode(1, 3);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ws.Send("���̾��ƴ�");
        }
    }
}