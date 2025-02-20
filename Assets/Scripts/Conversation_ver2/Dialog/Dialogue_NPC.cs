using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;


public class Dialogue_NPC : MonoBehaviour
{
    [Header("NPC code")]
    public int npcCode;
    public int currentQuestCode;
    public QuestManager.TableData curQuestData;

    [Space(10f)]

    public int cur_Dialogue_Index = 0;
    //public int quest_get_Index;

    [Space(10f)]

    public GameObject player;
    public Player_Quest p_Quest;

    public List<QuestManager.TableData> questData = new List<QuestManager.TableData>();
    public List<DialogueManager.Dialogue> dialogueData = new List<DialogueManager.Dialogue>();

    [Header("Dialogue Show")]
    public Canvas questUICanvas;

    public Canvas questBaloonCanvas;
    //bool baloonOn;

    public Text baloonText;
    public Image questBaloonUI;
    public Text dialogueContents;
    public Text BtnText;

    private QuestManager qmInstance;
    private DialogueManager dmInstance;

    private bool isprocessing;
    private int questIndex;

    [Tooltip("Database Number")]
    private int ballon = 0;
    private int questgetcondition = 1;
    private int questget = 2;
    private int questcompletioncondition = 3;
    private int questcompletion = 4;
    private int questprogress = 5;
    private int questdetails = 6;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        p_Quest = player.GetComponent<Player_Quest>();
        qmInstance = QuestManager.instance;
        dmInstance = DialogueManager.instance;
        questData.Clear();
        //본인 NPC코드에 맞는 퀘스트 가져오기
        for (int i = 0; i < qmInstance.questdataList.Count; i++)
        {
            if (qmInstance.questdataList[i].npc_code == npcCode)
            {
                Debug.Log(i);
                // 만약 내가 가지고 있어야 할 퀘스트가 있다면 다 가져온다.
                questData.Add(qmInstance.questdataList[i]);
            }
        }
        BallonCheck();
        //UI어떤 놈이 나와야되는지 
        CommunicationCheck();
    }
    float Dist = 0f;

    void UISet(bool appear, int uiIndex = 0)
    {
        questBaloonCanvas.gameObject.SetActive(appear);
        if (appear)
        {
            questBaloonUI.GetComponent<Image>().sprite = questData[uiIndex].questBaloon_UI;
            questIndex = uiIndex;
        }
    }
    void BallonCheck()
    {
        currentQuestCode = -1;
        questIndex = -1;
        isprocessing = false;
        for (int i = 0; i < questData.Count; i++)
        {
            if (questData[i].quest_get)
            {
                currentQuestCode = questData[i].quest_code;
                UISet(false);
                isprocessing = true;
                break;
            }
            else if(questData[i].ballon_appears)
            {
                currentQuestCode = questData[i].quest_code;
                UISet(true , i);
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
                        if (isprocessing)
                        {
                            //questData[currentQuestCode].
                        }
                        else 
                        {
                            if (questData[questIndex].quest_get_condition)
                            {
                                questUICanvas.gameObject.SetActive(true);
                                NextDialogue();
                            }
                            else
                            {
                                // processing과 로직이 같게 움직이면 됨
                            }
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

    public void NextDialogue()
    {
        dialogueContents.text = dialogueData[dialogueStartIndex].dialogues;
        dialogueStartIndex++;
        Debug.Log("nextDialogue start : " + dialogueStartIndex);
        if (dialogueStartIndex == questData[questIndex].questGet_index)
        {
            qmInstance.QDBM.SendUpdateBool(ballon, questData[questIndex].quest_code, false);
            qmInstance.QDBM.SendUpdateBool(questget, questData[questIndex].quest_code, true);
            qmInstance.QDBM.SendUpdateBool(ballon, questData[questIndex].chain_quest_get_code, true);
            //p_Quest.
        }
        if (dialogueData.Count - 1 == dialogueStartIndex)
        {
            Debug.Log("닫기 뜰 때 : " + dialogueStartIndex);
            dialogueContents.text = dialogueData[dialogueStartIndex].dialogues;
            BtnText.text = "닫기";
        }
        else if (dialogueData.Count == dialogueStartIndex)
        {
            Debug.Log("없어 질 때 : " + dialogueStartIndex);
            questUICanvas.gameObject.SetActive(false);
            dialogueStartIndex = 0;
        }
        //더이상 받을 퀘스트 없을 때, 종료 Case
    }
}