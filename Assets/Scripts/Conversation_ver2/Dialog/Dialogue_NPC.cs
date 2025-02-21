using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Dialogue_NPC : MonoBehaviour
{
    [Header("NPC code")]
    public int npcCode;
    public int currentQuestCode;
    public QuestManager.TableData curQuestData;

    [Space(10f)]

    public GameObject player;
    public Player_Quest p_Quest;

    public List<QuestManager.TableData> questData = new List<QuestManager.TableData>();
    public List<DialogueManager.Dialogue> dialogueData = new List<DialogueManager.Dialogue>();
    public List<DialogueManager.Processing> processingData = new List<DialogueManager.Processing>();
    
    [Tooltip ("valueStorage")]
    private List<DialogueManager.Dialogue> tempDialogueData = new List<DialogueManager.Dialogue>();
    private int tempQuestIndex;

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
                // 만약 내가 가지고 있어야 할 퀘스트가 있다면 다 가져온다.
                questData.Add(qmInstance.questdataList[i]);
            }
        }
        BallonCheck();
        //UI어떤 놈이 나와야되는지 
        CommunicationCheck();
    }
    float Dist = 0f;

    void UISet(bool appear, int uiIndex = -1)
    {
        questIndex = -1;
        questBaloonCanvas.gameObject.SetActive(appear);
        if (appear)
        {
            questBaloonUI.GetComponent<Image>().sprite = questData[uiIndex].questBaloon_UI;
        }
            if(uiIndex != -1)
                questIndex = uiIndex;
    }
    void BallonCheck()
    {
        currentQuestCode = -1;
        isprocessing = false;
        for (int i = 0; i < questData.Count; i++)
        {
            if (questData[i].quest_get)
            {
                currentQuestCode = questData[i].quest_code;
                UISet(false);
                isprocessing = true;
                ProcessingSet();
                break;
            }
            else if(questData[i].quest_get_condition)
            {
                currentQuestCode = questData[i].quest_code;
                if (questData[i].ballon_appears)
                    UISet(true, i);
                else
                    UISet(false, i);
                break;
            }
        }
        if (currentQuestCode == -1)
            UISet(false);
    }
    void ProcessingSet()
    {
        for (int i = 0; i < dmInstance.processingList.Count; i++) 
        {
            if (dmInstance.processingList[i].processing_quest_code == currentQuestCode)
                processingData.Add(dmInstance.processingList[i]);
        }
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
                            questUICanvas.gameObject.SetActive(true);
                            processingDialogue();
                        }
                        else 
                        {
                            if (questData[questIndex].quest_get_condition)
                            {
                                questUICanvas.gameObject.SetActive(true);
                                tempDialogueData = dialogueData;
                                tempQuestIndex = questIndex;
                                NextDialogue();
                            }
                            else
                            {
                                // processing과 로직이 같게 움직이면 됨
                            }
                        }
                    }
                }
            }
        }
    }


    private int dialogueStartIndex = -1;

    public void NextDialogue()
    {
        if (isprocessing)
        {
            processingDialogue();
            return;
        }
        dialogueStartIndex++;
        if (dialogueStartIndex == questData[tempQuestIndex].questGet_index)
        {
            qmInstance.QDBM.SendUpdateBallonAppears(questData[tempQuestIndex].quest_code, false);
            BallonCheck();
            CommunicationCheck();
            qmInstance.QDBM.SendUpdateQuestGet(questData[tempQuestIndex].quest_code, true);
            BallonCheck();
            CommunicationCheck();
            if (questData[tempQuestIndex].chain_quest_get_code > 0)
            {
                qmInstance.QDBM.SendUpdateBallonAppears(questData[tempQuestIndex].chain_quest_get_code, true);
                BallonCheck();
                CommunicationCheck();
                qmInstance.QDBM.SendUpdateQuestGetCondition(questData[tempQuestIndex].chain_quest_get_code, true);
                BallonCheck();
                CommunicationCheck();
            }
            p_Quest.QuestDataInIt(questData[tempQuestIndex].questIcon_UI, 
                                  questData[tempQuestIndex].quest_title, 
                                  questData[tempQuestIndex].quest_describe, 
                                  questData[tempQuestIndex].quest_details);

        }
        if (tempDialogueData.Count - 1 == dialogueStartIndex)
        {
            dialogueContents.text = tempDialogueData[dialogueStartIndex].dialogues;
            BtnText.text = "닫기";
        }
        else if (tempDialogueData.Count == dialogueStartIndex)
        {
            questUICanvas.gameObject.SetActive(false);
            dialogueStartIndex = -1;
        }
        else
            dialogueContents.text = tempDialogueData[dialogueStartIndex].dialogues;
    }
    public void processingDialogue()
    {
        dialogueStartIndex++;
        if (processingData.Count - 1 == dialogueStartIndex)
        {
            dialogueContents.text = processingData[dialogueStartIndex].processing_dialogues;
            BtnText.text = "닫기";
        }
        else if (processingData.Count <= dialogueStartIndex)
        {
            questUICanvas.gameObject.SetActive(false);
            dialogueStartIndex = -1;
        }
        else
            dialogueContents.text = processingData[dialogueStartIndex].processing_dialogues;
    }
}