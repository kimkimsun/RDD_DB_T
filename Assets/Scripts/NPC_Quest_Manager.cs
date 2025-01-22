using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Quest_Manager : MonoBehaviour
{
    public QuestManager QM;

    [System.Serializable]
    public class Quest
    {
        public int QuestCode;        //퀘스트 코드
        public int requireLV;       //퀘스트 요구 레벨
        public bool processing;     //퀘스트 진행중인지 체크
        public string[] texts;      //퀘스트 대화들


        public int startIndex;      //퀘스트 발급 시점 index    
        public bool finishCondition()
        {
            //DB연동?
            //이게 true를 return하게 되면 완료된거임
            return false;
        }

        public void QuestStart()    //퀘스트 발급 함수
        {
            //DB에 퀘스트 발급 되었다고 알리기
        }
    }

    [Header("NPC Code")]
    public int NPC_Code;

    [Space(20f)]

    [Header("Quest Mark")]
    public Image questIndicator;                                                //퀘스트 마크

    [Header("Dialog Panel")]
    public Image dialogPanel;                                                   //dialog Panel

    [Header("Quest Lists")]
    public List<QuestManager.Quest> quests = new List<QuestManager.Quest>();    //퀘스트 목록 from DB
    //public QuestConditions getQuestconditions;                                  //퀘스트 획득 세부 조건
    [Space(20f)]

    public int processingQuestIndex;                                            //진행시작한 퀘스트의 index

    public class Dialog
    {
        public bool dialogType = false;     //false =  말풍선 대화, true = TextBox 대화
        public string dialog = null;      //퀘스트 대사들 모음
    }

    [System.Serializable]
    public class QuestValue
    {
        public int QuestCode;   //대화가 시작 될때, NPC_Quest_Manager.processingQuestIndex의 값을 찾아와야함
        public List<Dialog> dialogs = new List<Dialog>();       //대화 목록

        public bool questGet = false;           //퀘스트 획득 여부 -> DB로 보냄
        public bool questProcessing = false;    //퀘스트 진행 여부 -> DB로 보냄
        public bool questFinish = false;        //퀘스트 완료 여부 -> DB로 보냄
        public Text[] questDetail = null;       //퀘스트 세부 사항 -> DB로 보냄

        public int questGetIndex = 0;       //퀘스트 획득 시점
        public int questFinishIndex = 0;    //퀘스트 완료 시점

        public Image descriptImg = null;    //사진 이미지 => imgList여기에서 바꿔치기 함
        public int imageShowIndex = 0;      //사진이 != null일 때, 사진 보여줄 시점
        public int choiceDialogIndex = 0;   //N지 선다 대화 선택 시점
    }

    //퀘스트 획득조건 별도 제작
    //퀘스트 획득 가능 여부
    //퀘스트 완료 조건
    //퀘스트 완료 가능 여부

    [Space(20f)]
    [Header("Quest Dialog Lists")]
    public List<QuestValue> questValues = new List<QuestValue>();
    public Image[] imgList = null;

    [Space(10f)]
    public Player_Controller playerController;  //플레이어
    public bool pEnter;
    public BoxCollider detectCol;


    private void Awake()
    {
        playerController = GameObject.FindAnyObjectByType<Player_Controller>();
        QM = FindFirstObjectByType<QuestManager>();
        detectCol = GetComponent<BoxCollider>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Quest Manager에서 본인의 NPC_Code에 맞는 퀘스트들 찾아오기
        if(Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < QM.quests.Count; i++)
            {
                if (QM.quests[i].NpcCode == NPC_Code)
                {
                    quests.Add(QM.quests[i]);
                }
            }
        }

        if(Vector3.Distance(this.transform.position, playerController.gameObject.transform.position) < 10f)
        {
            pEnter = true;
            if (!questIndicator.enabled)
            {
                questIndicator.enabled = true;
            }
        }
        else
        {
            pEnter = false;
            if (questIndicator.enabled)
            {
                questIndicator.enabled = false;
            }
        }
    }

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(pEnter)
        {
            if (questIndicator.enabled)
            {
                // processingQuestIndex에 대한 값을 어떻게 가져올 것인가.

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("NPC") && !hit.collider.isTrigger)
                {
                    if (dialog == null)
                    {
                        dialog = StartCoroutine(QuestDialog());
                    }
                    else
                    {
                        StopCoroutine(dialog);
                        dialog = StartCoroutine(QuestDialog());
                    }
                }
            }
            else
            {
                //Default 대화
            }
        }
    }

    Coroutine dialog;
    public IEnumerator QuestDialog()    //퀘스트 대화 진행 coroutine
    {
        if(dialogPanel != null && !dialogPanel.gameObject.activeSelf)
        {
            dialogPanel.gameObject.SetActive(true);
        }
        Debug.Log("퀘스트 대화 시작");
        yield return null;
    }
}
