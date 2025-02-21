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
        //���� NPC�ڵ忡 �´� ����Ʈ ��������
        for (int i = 0; i < qmInstance.questdataList.Count; i++)
        {
            if (qmInstance.questdataList[i].npc_code == npcCode)
            {
                // ���� ���� ������ �־�� �� ����Ʈ�� �ִٸ� �� �����´�.
                questData.Add(qmInstance.questdataList[i]);
            }
        }
        BallonCheck();
        //UI� ���� ���;ߵǴ��� 
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
                        Debug.Log("�ٴҶ� ���"); // ���Ⱑ �ٴҶ��Դϴ�.
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
                                // processing�� ������ ���� �����̸� ��
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
            BtnText.text = "�ݱ�";
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
            BtnText.text = "�ݱ�";
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