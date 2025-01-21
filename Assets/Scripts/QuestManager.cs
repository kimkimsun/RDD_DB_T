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
        ws.OnMessage += OnMessageReceived; // 서버 응답 처리
        ws.Connect();
    }

    public void RequestQuestData(int questCode)
    {
        if (ws == null || !ws.IsAlive)
            return;


        // QuestRequestMessage 클래스 사용
        QuestRequestMessage message = new QuestRequestMessage
        {
            type = "getQuest",
            questCode = questCode
        };

        string jsonMessage = JsonUtility.ToJson(message);  // 메시지 직렬화
        Debug.Log("보내는 메시지: " + jsonMessage);  // 전송하는 메시지 로그
        ws.Send(jsonMessage);
    }

    private void OnMessageReceived(object sender, MessageEventArgs e)
    {
        Debug.Log("서버 응답: " + e.Data);

        // 서버 응답을 처리하여 퀘스트 데이터를 업데이트
        try
        {
            var response = JsonUtility.FromJson<ServerResponse>(e.Data);
            if (response.type == "questData")
            {
                // 퀘스트 데이터를 리스트로 업데이트
                quests.Clear(); // 기존 데이터 클리어
                foreach (var quest in response.data)
                {
                    quests.Add(new Quest
                    {
                        QuestCode = quest.quest_code,   // JSON 필드 이름에 맞게 수정
                        requireLV = quest.require_lv,  // JSON 필드 이름에 맞게 수정
                        texts = quest.texts
                    });
                }
                Debug.Log("퀘스트 데이터 업데이트 완료");
            }
            else
            {
                Debug.LogError("서버 오류: " + response.message);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("응답 처리 오류: " + ex.Message);
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
