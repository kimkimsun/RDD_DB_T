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
        //���� NPC�ڵ忡 �´� ����Ʈ ��������
        for (int i = 0; i < qmInstance.TQuestdataList.Count; i++)
        {
            if (qmInstance.TQuestdataList[i].TGivenpc_code == npcCode)
            {
                // ���� ���� ������ �־�� �� ����Ʈ�� �ִٸ� �� �����´�.
                questData.Add(qmInstance.TQuestdataList[i]);
            }
        }
        BallonCheck();
        //UI� ���� ���;ߵǴ��� 
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
                        Debug.Log("�ٴҶ� ���"); // ���Ⱑ �ٴҶ��Դϴ�.
                    }
                    else
                    {
                        if(isprocessing)
                        {
                            //questData[currentQuestCode].
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
    public int questIndex = 0;

    public void NextDialogue()
    {
        if (dialogueData.Count == 0)
        {
            Debug.LogError("dialogueData�� ��� �ֽ��ϴ�!");
            return;
        }

        if (dialogueStartIndex >= dialogueData.Count)
        {
            Debug.LogWarning("�� �̻� ��ȭ�� �����ϴ�.");
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

                BtnText.text = "����";
                dialogueStartIndex++;
                cur_Dialogue_Index++;
            }
            else if (currentDialogueCount - 1 == cur_Dialogue_Index)
            {
                dialogueCharacter.text = dialogueData[dialogueStartIndex].NPC_name;
                dialogueContents.text = dialogueData[dialogueStartIndex].dialogues;
                BtnText.text = "�ݱ�";
                cur_Dialogue_Index++;
                dialogueStartIndex++;

            }


            //���̻� ���� ����Ʈ ���� ��, ���� Case
            if (dialogueStartIndex >= dialogueData.Count)
            {
                if (questUICanvas.gameObject.activeSelf)
                {
                    //������ �ʱ�ȭ
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
                //������ �ʱ�ȭ
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
            Debug.Log("����Ʈ ȹ��");
            // QuestDatabaseManager.SendUpdateNpcCode(1, 4);
        }

    }
}
