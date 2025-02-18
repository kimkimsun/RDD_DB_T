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

    public Image questBaloonUI;
    public Text dialogueCharacter;
    public Text dialogueContents;
    public Text BtnText;

    private QuestManager qmInstance;
    private DialogueManager dmInstance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        qmInstance = QuestManager.instance;
        dmInstance = DialogueManager.instance;
        //���� NPC�ڵ忡 �´� ����Ʈ ��������
        for (int i = 0; i < qmInstance.TQuestdataList.Count; i++)
        {
            if (qmInstance.TQuestdataList[i].TGivenpc_code == npcCode)
            {
                if (qmInstance.TQuestdataList[i].quest_get_condition &&
                !(qmInstance.TQuestdataList[i].quest_completion))
                {
                    questData.Add(qmInstance.TQuestdataList[i]);
                    curQuestData = questData[0];
                    cur_QuestCode = curQuestData.Tquest_code;
                }
            }
        }

        //quest code�� �����ϴ� ��ȭ��
        for (int i = 0; i < questData.Count; i++)
        {
            for (int j = 0; j < dmInstance.dialogueList.Count; j++)
            {
                if (dmInstance.dialogueList[j].quest_code == questData[i].Tquest_code)
                {
                    dialogueData.Add(dmInstance.dialogueList[j]);

                }
            }
        }
    }
    float Dist = 0f;


    public int currentDialogueCount = 0;
    void Update()
    {
        Dist = Vector3.Distance(this.transform.position, player.transform.position);
        if (Dist <= 13f)
        {
            #region ����Ʈ ��ǳ�� ���� ����
            if (dialogueData.Count > 0)    //SendMessage(QuestGEt)
            {
                questBaloonCanvas.gameObject.SetActive(true);
                if (questData[0].Tquest_code == dialogueData[0].quest_code)
                    questBaloonUI.GetComponent<Image>().sprite = questData[0].TquestBaloon_UI;
            }
            #endregion


            //��ũ�� �� ���¿��� Ŭ�� ���� �� ���� �߰�
            if (questBaloonUI.gameObject.activeSelf)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.gameObject.tag == "NPC")
                        {
                            Debug.Log(hit.transform.gameObject.name);

                            //TEST
                            if (!questUICanvas.gameObject.activeSelf)
                            {
                                for (int i = 0; i < dialogueData.Count; i++)
                                {
                                    if (dialogueData[i].quest_code == cur_QuestCode)
                                    {
                                        currentDialogueCount++;
                                    }
                                }

                                questUICanvas.gameObject.SetActive(true);
                                questBaloonCanvas.gameObject.SetActive(false);
                                NextDialogue();
                            }
                        }
                    }
                }
            }

        }
        else
        {
            if (questBaloonCanvas.gameObject.activeSelf)
            {
                questBaloonCanvas.gameObject.SetActive(false);
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

        if (cur_QuestCode ==  dialogueData[dialogueStartIndex].quest_code)
        {
            if (currentDialogueCount - 1 > cur_Dialogue_Index)
            {
                dialogueCharacter.text = dialogueData[dialogueStartIndex].NPC_name;
                dialogueContents.text = dialogueData[dialogueStartIndex].dialogues;
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
