using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DialogueManager;


public class Dialogue_NPC : MonoBehaviour
{
    [Header("NPC code")]
    public int npcCode;
    public int cur_QuestCode;

    [Space(10f)]

    public int cur_Dialogue_Index = 0;
    public int event_Dialogue_Index;
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //���� NPC�ڵ忡 �´� ����Ʈ ��������
        for (int i = 0; i < QuestManager.instance.TQuestdataList.Count; i++)
        {
            if (QuestManager.instance.TQuestdataList[i].TGivenpc_code == npcCode)
            {
                questData.Add(QuestManager.instance.TQuestdataList[i]);
            }
        }

        //quest code�� �����ϴ� ��ȭ��
        for (int i = 0; i < DialogueManager.instance.dialogueList.Count; i++)
        {
            if (DialogueManager.instance.dialogueList[i].quest_code == cur_QuestCode)
            {
                dialogueData.Add(DialogueManager.instance.dialogueList[i]);
            }
        }

    }

    float Dist = 0f;
    // Update is called once per frame
    void Update()
    {
        Dist = Vector3.Distance(this.transform.position, player.transform.position);
        if (Dist <= 13f)
        {
#region ����Ʈ ��ǳ�� ���� ����
            //Debug.Log("���� ���� �Ǹ� ��ũ ���");
            //if("�������� == true")    //SendMessage(QuestGEt)
            //{
            //    //if("����Ʈ ���� ������ UI �ѵα�")
            //    {
                    //if(!baloonOn)
                    //{
                    //    baloonOn = true;
                    //    questBaloonCanvas.gameObject.SetActive(true);
                    //}
                    questBaloonCanvas.gameObject.SetActive(true);
                    questBaloonUI.GetComponent<Image>().sprite = questData[0].TquestBaloon_UI;
            //    }
            //}
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
            if(questBaloonCanvas.gameObject.activeSelf)
            {
                questBaloonCanvas.gameObject.SetActive(false);
            }
        }
    }

    public void NextDialogue()
    {
        if(dialogueData.Count -1 > cur_Dialogue_Index)
        {
            dialogueCharacter.text = dialogueData[cur_Dialogue_Index].NPC_name;
            dialogueContents.text = dialogueData[cur_Dialogue_Index].dialogues;
            BtnText.text = "����";
            cur_Dialogue_Index++;
        }
        else if(dialogueData.Count - 1 == cur_Dialogue_Index)
        {
            dialogueCharacter.text = dialogueData[cur_Dialogue_Index].NPC_name;
            dialogueContents.text = dialogueData[cur_Dialogue_Index].dialogues;
            BtnText.text = "�ݱ�";
            cur_Dialogue_Index++;
        }
        else
        {
            if (questUICanvas.gameObject.activeSelf)
            {
                //������ �ʱ�ȭ
                cur_Dialogue_Index = 0;
                questUICanvas.gameObject.SetActive(false);
                Debug.Log("�ݱ�");
            }
        }

        if (cur_Dialogue_Index == event_Dialogue_Index)
        {
            Debug.Log("DB ����");
            // QuestDatabaseManager.SendUpdateNpcCode(1, 4);
        }

    }
}
