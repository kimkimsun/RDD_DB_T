using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using Unity.VisualScripting;

public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class Quest
    {
        public int QuestCode;       // 퀘스트 코드
        public int NpcCode;         // NPC 코드
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
        public int quest_code;  // 퀘스트 코드
        public int npc_code;    // NPC 코드
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
        ws.OnMessage += OnMessageReceived; // 서버 응답 처리

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

        // 응답이 JSON 형식이 아니라면 그냥 출력하고 종료
        if (e.Data == "연결이 성공적으로 이루어졌습니다.")
        {
            Debug.Log("서버와의 연결이 성공적으로 이루어졌습니다.");
            return;
        }

        // 응답 데이터를 분할하고 파싱하여 필요한 값 저장
        try
        {
            // 서버 응답을 파싱 (예: { "type": "questData", "data": [...] })
            var response = JsonUtility.FromJson<ServerResponse>(e.Data);

            // 응답 타입이 "questData"인 경우
            if (response.data != null && response.data.Count > 0)
            {
                // 퀘스트 데이터를 리스트로 업데이트
                quests.Clear();  // 기존 데이터 클리어

                foreach (var quest in response.data)
                {
                    // quest_code와 npc_code를 추출하여 로그로 출력
                    //Debug.Log("퀘스트 코드: " + quest.quest_code);
                    //Debug.Log("NPC 코드: " + quest.npc_code);

                    // 각 퀘스트를 리스트에 추가
                    quests.Add(new Quest
                    {
                        QuestCode = quest.quest_code,
                        NpcCode = quest.npc_code,
                    });
                }

                //Debug.Log("퀘스트 데이터 업데이트 완료");
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

}
