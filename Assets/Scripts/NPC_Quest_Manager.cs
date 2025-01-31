using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Quest_Manager : MonoBehaviour
{
    public QuestManager QM;

    [Header("NPC Code")]
    public int NPC_Code;

    [Space(20f)]

    [Header("Quest Mark")]
    public Image questIndicator;                                                //퀘스트 마크

    [Header("Dialog Panel")]
    public Image dialogPanel;                                                   //dialog Panel
    public Text dialogText;                                                     //dialog Text
    public Text dialogTextPopup;                                                //dialog Popup

    [Header("Quest Lists")]
    public List<QuestManager.Quest> quests = new List<QuestManager.Quest>();    //퀘스트 목록 from DB
    [Space(20f)]

    public int processingQuestCode;                                             //진행시작한 퀘스트의 index
    public int processingQuestindex;                                            //진행시작한 퀘스트의 index

    [System.Serializable]
    public class Dialog
    {
        public bool dialogType = false;     //false =  말풍선 대화, true = TextBox 대화
        public string dialog = null;        //퀘스트 대사들 모음
    }

    [System.Serializable]
    public class QuestValue
    {
        public bool questGet = false;           //퀘스트 획득 여부 -> DB로 보냄
        public int questGetIndex = 0;           //퀘스트 획득 시점

        [Header("Quest Get Conditions")]
        public bool startQuest;                             //퀘스트 획득 조건 : 시작 퀘스트
        public bool requireLevel;                           //퀘스트 획득 조건 : 레벨제한이 있는지
        public int condition_Level;                         //퀘스트 획득 조건 : 레벨

        public bool connectQuest;                           //퀘스트 획득 조건 : 연계 퀘스트
        public NPC_Quest_Manager preQuestNPC_Code = null;   //퀘스트 획득 조건 : 연계 퀘스트의 이전 NPC
        public int preQuestCode;                            //퀘스트 획득 조건 : 연계 퀘스트의 이전 퀘스트 코드
        public NPC_Quest_Manager nextQuestNPC_Code = null;  //퀘스트 획득 조건 : 연계 퀘스트의 이후 NPC    
        public int nextQuestCode;                           //퀘스트 획득 조건 : 연계 퀘스트의 이후 퀘스트 코드

        public bool itemQuest;                  //퀘스트 획득 조건 : 요구 아이템
        public string[] requireItemLis;         //퀘스트 획득 조건 : 요구 아이템리스트

        [Space(10f)]
        [Header("Quest Processing Check")]
        public bool questProcessing = false;    //퀘스트 진행 여부 -> DB로 보냄

        [Space(10f)]
        [Header("Quest Finish Conditions")]
        public bool questFinish = false;        //퀘스트 완료 여부 -> DB로 보냄
        public int questFinishIndex = 0;        //퀘스트 완료 시점
        public int connectQuestFinishIndex;     //퀘스트 완료 조건 : 연계 퀘스트의 완료 시점

        [Space(10f)]
        [Header("Quest Details")]
        public Text[] questDetail = null;       //퀘스트 세부 사항 -> DB로 보냄

        [Space(10f)]
        [Header("Quest Information")]
        public int QuestCode;                                   //대화가 시작 될때, NPC_Quest_Manager.processingQuestIndex의 값을 찾아와야함

        public bool choiceDialogStart;                          //N지 선다 대화 시작
        public int[] choiceDialogIndex;                         //N지 선다 대화 선택 시점들

        public List<Dialog> dialogs = new List<Dialog>();       //대화 목록

        [Space(10f)]
        [Header("Quest Info Images")]
        public Image descriptImg = null;    //사진 이미지 => imgList여기에서 바꿔치기 함
        public int[] imageShowIndex;        //imageShowIndex.count > 0 일 때, 사진 보여줄 시점들 모음
        public Image[] imgList = null;      //보여줄 image index가 있을 때 해당 이미지 리스트에서 인덱스에 맞는 이미지 가져옴
    }

    [Space(20f)]
    [Header("Quest Dialog Lists")]
    public List<QuestValue> questValues = new List<QuestValue>();

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

        if(questValues.Count > 0)
        {
            if (!questValues[0].questGet && questValues[0].requireLevel && questValues[0].condition_Level <= playerController.Lv && !questValues[0].connectQuest && !questValues[0].itemQuest)
            {
                if (Vector3.Distance(this.transform.position, playerController.gameObject.transform.position) < 10f)
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
            else if(questValues[0].connectQuest)
            {
                if (Vector3.Distance(this.transform.position, playerController.gameObject.transform.position) < 10f)
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
            else
            {
                if (questIndicator.enabled)
                {
                    questIndicator.enabled = false;
                }
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
                processingQuestindex = 0;
                processingQuestCode = questValues[processingQuestindex].QuestCode;
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

            int dialogIndex = 0;

            //퀘스트 dialogs에 들어있는 dialog 개수 만큼 대화 진행
            while (dialogIndex < questValues[processingQuestindex].dialogs.Count)
            {
                //연계퀘스트 true면 관련 퀘스트 완료 처리
                ConnectQuestFinish(dialogIndex);

                //퀘스트 발급 시점이 되면 퀘스트 발급
                QuestPublish(dialogIndex);

                //대화 type에 따라 true면 textBox대화 false면 말풍선 대화
                if (questValues[processingQuestindex].dialogs[dialogIndex].dialogType)
                {
                    dialogText.text = questValues[processingQuestindex].dialogs[dialogIndex].dialog;
                }
                else
                {
                    dialogText.text = "말풍선 띄우기" + questValues[processingQuestindex].dialogs[dialogIndex].dialog;
                }

                //G키를 눌러서 다음 대화로 진행
                if (Input.GetKeyDown(KeyCode.G))
                {
                    dialogIndex++;
                }

                yield return null;
            }
            //대화 끝나면 판넬 창 닫기
            dialogPanel.gameObject.SetActive(false);

            //연계 퀘스트 인지 확인
            CheckConnectQuest();
        }
    }


    public void CheckConnectQuest()
    {
        if (questValues[0].nextQuestNPC_Code != null)
        {
            for (int i = 0; i < questValues[0].nextQuestNPC_Code.questValues.Count; i++)
            {
                if (questValues[0].nextQuestNPC_Code.questValues[i].QuestCode == questValues[0].nextQuestCode)
                {
                    questValues[0].nextQuestNPC_Code.questValues[i].connectQuest = true;
                }
            }
        }
    }

    public void ConnectQuestFinish(int dialogIDX)
    {
        //연계퀘스트 true면 관련 퀘스트 완료 처리
        if (questValues[processingQuestindex].connectQuest && dialogIDX == questValues[processingQuestindex].connectQuestFinishIndex)
        {
            for (int i = 0; i < questValues[processingQuestindex].preQuestNPC_Code.questValues.Count; i++)
            {
                if (questValues[processingQuestindex].preQuestNPC_Code.questValues[i].QuestCode == questValues[processingQuestindex].preQuestCode)
                {
                    questValues[processingQuestindex].preQuestNPC_Code.questValues.RemoveAt(i);
                }
            }
        }
    }
    public void QuestPublish(int dialogIDX)
    {
        //퀘스트 발급 시점이 되면 퀘스트 발급
        if (dialogIDX == questValues[processingQuestindex].questGetIndex && !questValues[processingQuestindex].questGet)
        {
            questValues[processingQuestindex].questGet = true;
            Debug.Log("퀘스트 발급!!!"); //플레이어의 퀘스트 리스트에 넣어주고 UI표시
        }
    }

}
