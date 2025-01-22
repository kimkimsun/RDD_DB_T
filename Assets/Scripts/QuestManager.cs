using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using Unity.VisualScripting;

public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class Quest
    {
        public int QuestCode;       // ����Ʈ �ڵ�
        public int NpcCode;         // NPC �ڵ�
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
        public int quest_code;  // ����Ʈ �ڵ�
        public int npc_code;    // NPC �ڵ�
    }

    public class QuestRequestMessage
    {
        public string type = "";
        public int questCode = 0;
    }

    private WebSocket ws;
    public List<Quest> quests = new List<Quest>();

    private void Awake()
    {
        ws = new WebSocket("ws://localhost:7777");
    }

    private void Start()
    {
        ws.Connect();
        ws.OnMessage += OnMessageReceived; // ���� ���� ó��

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

        // ������ JSON ������ �ƴ϶�� �׳� ����ϰ� ����
        if (e.Data == "������ ���������� �̷�������ϴ�.")
        {
            Debug.Log("�������� ������ ���������� �̷�������ϴ�.");
            return;
        }

        // ���� �����͸� �����ϰ� �Ľ��Ͽ� �ʿ��� �� ����
        try
        {
            // ���� ������ �Ľ� (��: { "type": "questData", "data": [...] })
            var response = JsonUtility.FromJson<ServerResponse>(e.Data);

            // ���� Ÿ���� "questData"�� ���
            if (response.data != null && response.data.Count > 0)
            {
                // ����Ʈ �����͸� ����Ʈ�� ������Ʈ
                quests.Clear();  // ���� ������ Ŭ����

                foreach (var quest in response.data)
                {
                    // quest_code�� npc_code�� �����Ͽ� �α׷� ���
                    //Debug.Log("����Ʈ �ڵ�: " + quest.quest_code);
                    //Debug.Log("NPC �ڵ�: " + quest.npc_code);

                    // �� ����Ʈ�� ����Ʈ�� �߰�
                    quests.Add(new Quest
                    {
                        QuestCode = quest.quest_code,
                        NpcCode = quest.npc_code,
                    });
                }

                //Debug.Log("����Ʈ ������ ������Ʈ �Ϸ�");
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

}
