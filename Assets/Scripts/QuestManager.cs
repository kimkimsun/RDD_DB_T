using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public class QuestRequestMessage
    {
        public string type = "";
        public int questCode = 0;
    }
    private WebSocket ws;
    private List<Quest> quests = new List<Quest>();

    private void Awake()
    {
        ws = new WebSocket("ws://localhost:7777");
        ws.OnMessage += OnMessageReceived; // ���� ���� ó��
        ws.Connect();
    }

    public void RequestQuestData(int questCode)
    {
        if (ws == null || !ws.IsAlive)
            return;


        // QuestRequestMessage Ŭ���� ���
        QuestRequestMessage message = new QuestRequestMessage
        {
            type = "getQuest",
            questCode = questCode
        };

        string jsonMessage = JsonUtility.ToJson(message);  // �޽��� ����ȭ
        Debug.Log("������ �޽���: " + jsonMessage);  // �����ϴ� �޽��� �α�
        ws.Send(jsonMessage);
    }

    private void OnMessageReceived(object sender, MessageEventArgs e)
    {
        Debug.Log("���� ����: " + e.Data);

        // ���� ������ ó���Ͽ� ����Ʈ �����͸� ������Ʈ
        try
        {
            var response = JsonUtility.FromJson<ServerResponse>(e.Data);
            if (response.type == "questData")
            {
                // ����Ʈ �����͸� ����Ʈ�� ������Ʈ
                quests.Clear(); // ���� ������ Ŭ����
                foreach (var quest in response.data)
                {
                    quests.Add(new Quest
                    {
                        QuestCode = quest.quest_code,   // JSON �ʵ� �̸��� �°� ����
                        requireLV = quest.require_lv,  // JSON �ʵ� �̸��� �°� ����
                        texts = quest.texts
                    });
                }
                Debug.Log("����Ʈ ������ ������Ʈ �Ϸ�");
            }
            else
            {
                Debug.LogError("���� ����: " + response.message);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("���� ó�� ����: " + ex.Message);
        }
    }

    [System.Serializable]
    public class Quest
    {
        public int QuestCode;
        public int requireLV;
        public string[] texts;
    }

    [System.Serializable]
    public class ServerResponse
    {
        public string type;
        public string message;
        public List<QuestData> data;
    }

    [System.Serializable]
    public class QuestData
    {
        public int quest_code;
        public int require_lv;
        public string[] texts;
    }
}
