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
        //���� NPC�ڵ忡 �´� ����Ʈ ��������
        for (int i = 0; i < qmInstance.questdataList.Count; i++)
        {
            if (qmInstance.questdataList[i].npc_code == npcCode)
            {
                Debug.Log(i);
                // ���� ���� ������ �־�� �� ����Ʈ�� �ִٸ� �� �����´�.
                questData.Add(qmInstance.questdataList[i]);
            }
        }
        BallonCheck();
        //UI� ���� ���;ߵǴ��� 
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
                        Debug.Log("�ٴҶ� ���"); // ���Ⱑ �ٴҶ��Դϴ�.
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
                                // processing�� ������ ���� �����̸� ��
                            }
                        }
                        // ����Ʈ ������ ��ȭ�� ������
                        // ����Ʈ ȹ�� ��ȭ�� ������
                        // ����Ʈ �Ϸ� ��ȭ�� ������
                        // �б� 3��
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
            Debug.Log("�ݱ� �� �� : " + dialogueStartIndex);
            dialogueContents.text = dialogueData[dialogueStartIndex].dialogues;
            BtnText.text = "�ݱ�";
        }
        else if (dialogueData.Count == dialogueStartIndex)
        {
            Debug.Log("���� �� �� : " + dialogueStartIndex);
            questUICanvas.gameObject.SetActive(false);
            dialogueStartIndex = 0;
        }
        //���̻� ���� ����Ʈ ���� ��, ���� Case
    }
}