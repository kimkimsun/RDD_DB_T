using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DialogueManager;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;


public class Dialogue_NPC : MonoBehaviour
{
    [Header("NPC code")]
    public int npcCode;
    public int cur_QuestCode;
    public int currentQuestCode;
    public QuestManager.TableData curQuestData;

    [Space(10f)]

    public int cur_Dialogue_Index = 0;
    //public int quest_get_Index;

    [Space(10f)]

    public GameObject player;

    public List<QuestManager.TableData> questData = new List<QuestManager.TableData>();
    public List<DialogueManager.Dialogue> dialogueData = new List<DialogueManager.Dialogue>();

    [Header("Dialogue Show")]
    public Canvas questUICanvas;

    public Canvas questBaloonCanvas;
    //bool baloonOn;

    public Text baloonText;
    public Image questBaloonUI;
    public Text dialogueCharacter;
    public Text dialogueContents;
    public Text BtnText;

    private QuestManager qmInstance;
    private DialogueManager dmInstance;

    private bool isprocessing;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        qmInstance = QuestManager.instance;
        dmInstance = DialogueManager.instance;
        questData.Clear();
        //본인 NPC코드에 맞는 퀘스트 가져오기
        for (int i = 0; i < qmInstance.TQuestdataList.Count; i++)
        {
            if (qmInstance.TQuestdataList[i].TGivenpc_code == npcCode)
            {
                // 만약 내가 가지고 있어야 할 퀘스트가 있다면 다 가져온다.
                questData.Add(qmInstance.TQuestdataList[i]);
            }
        }
        BallonCheck();
        //UI어떤 놈이 나와야되는지 
        CommunicationCheck();
    }
    float Dist = 0f;

    void UISet(bool appear)
    {
        questBaloonCanvas.gameObject.SetActive(appear);
        if (appear == true)
            questBaloonUI.GetComponent<Image>().sprite = questData[currentQuestCode].TquestBaloon_UI;
    }
    void BallonCheck()
    {
        currentQuestCode = -1;
        isprocessing = false;
        for (int i = 0; i < questData.Count; i++)
        {
            if (questData[i].quest_get_condition)
            {
                currentQuestCode = questData[i].Tquest_code;
                UISet(false);
                isprocessing = true;
                break;
            }
            if (questData[i].ballon_appears)
            {
                currentQuestCode = questData[i].Tquest_code;
                UISet(true);
                break;
            }
        }
        if (currentQuestCode == -1)
            UISet(false);
    }

    void CommunicationCheck()
    {
        dialogueData.Clear();
        for (int i = 0; i < questData.Count; i++)
        {
            for (int j = 0; j < dmInstance.dialogueList.Count; j++)
            {
                if (dmInstance.dialogueList[j].quest_code == currentQuestCode)
                {
                    dialogueData.Add(dmInstance.dialogueList[j]);
                }
            }
        }
    }
    int currentDialogueCount;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "NPC")
                {
                    //Debug.Log(hit.transform.gameObject.name);

                    //TEST
                    if (currentQuestCode == -1)
                    {
                        Debug.Log("바닐라 출력"); // 여기가 바닐라입니다.
                    }
                    else
                    {
                        if(isprocessing)
                        {
                            //questData[currentQuestCode].
                        }
                        // 퀘스트 진행중 대화로 갈건지
                        // 퀘스트 획득 대화로 갈건지
                        // 퀘스트 완료 대화로 갈건지
                        // 분기 3개
                    }
                }
            }
        }
    }


    public int dialogueStartIndex = 0;
    public int questIndex = 0;

    public void NextDialogue()
    {
        if (dialogueData.Count == 0)
        {
            Debug.LogError("dialogueData가 비어 있습니다!");
            return;
        }

        if (dialogueStartIndex >= dialogueData.Count)
        {
            Debug.LogWarning("더 이상 대화가 없습니다.");
            return;
        }

        if (cur_QuestCode == dialogueData[dialogueStartIndex].quest_code)
        {
            if (currentDialogueCount - 1 > cur_Dialogue_Index)
            {
                if (dialogueData[dialogueStartIndex].dialogueType != 0)
                {
                    dialogueCharacter.text = dialogueData[dialogueStartIndex].NPC_name;
                    dialogueContents.text = dialogueData[dialogueStartIndex].dialogues;
                }

                BtnText.text = "다음";
                dialogueStartIndex++;
                cur_Dialogue_Index++;
            }
            else if (currentDialogueCount - 1 == cur_Dialogue_Index)
            {
                dialogueCharacter.text = dialogueData[dialogueStartIndex].NPC_name;
                dialogueContents.text = dialogueData[dialogueStartIndex].dialogues;
                BtnText.text = "닫기";
                cur_Dialogue_Index++;
                dialogueStartIndex++;

            }


            //더이상 받을 퀘스트 없을 때, 종료 Case
            if (dialogueStartIndex >= dialogueData.Count)
            {
                if (questUICanvas.gameObject.activeSelf)
                {
                    //나머지 초기화
                    questIndex = 0;
                    curQuestData = null;
                    cur_QuestCode = 0;
                    currentDialogueCount = 0;
                    cur_Dialogue_Index = 0;
                    questUICanvas.gameObject.SetActive(false);
                }
                return;
            }
        }
        else
        {
            if (questUICanvas.gameObject.activeSelf)
            {
                //나머지 초기화
                questIndex++;
                curQuestData = questData[questIndex];
                cur_QuestCode = curQuestData.Tquest_code;
                currentDialogueCount = 0;
                cur_Dialogue_Index = 0;
                questUICanvas.gameObject.SetActive(false);
            }
        }



        if (dialogueStartIndex == questData[0].TquestGet_index)
        {
            Debug.Log("퀘스트 획득");
            // QuestDatabaseManager.SendUpdateNpcCode(1, 4);
        }

    }
}
