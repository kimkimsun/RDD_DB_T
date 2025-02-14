using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InterAction_Event : MonoBehaviour
{
    [Header("NPC code")]
    public int npcCode;
    public int cur_QuestCode;

    [Space(10f)]

    //public DialogueDB_Manager characterDB;
    public int cur_Dialogue_Index = 0;
    public int event_Dialogue_Index;
    [Space(10f)]

    public GameObject player;

    //public QuestManager.TableData[] questData;

    public List<QuestManager.TableData> questData = new List<QuestManager.TableData>();

    [SerializeField] DialogueEvent dialogue;

    [Header("Dialogue Show")]
    public Image dialoguePanel;
    public Text dialogueCharacter;
    public Text dialogueContents;
    public Text BtnText;


    public void Awake()
    {

    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //dialogue.dialogues = GetDialogue();

        //���� NPC�ڵ忡 �´� ����Ʈ ��������
        for (int i = 0; i < QuestManager.instance.TQuestdataList.Count; i++)
        {
            if (QuestManager.instance.TQuestdataList[i].TGivenpc_code == npcCode)
            {
                questData.Add(QuestManager.instance.TQuestdataList[i]);
            }
        }
        //���⿡ ���缭 ��ȭ�� �������� �����ٵ�
        //quest code�� �����ϴ� ��ȭ��
    }

    //public Dialogue[] GetDialogue()
    //{
    //    dialogue.dialogues = QuestManager.instance.characterDB.GetDialogue((int)dialogue.line.x, (int)dialogue.line.y);
    //    dialogue.name = QuestManager.instance.characterDB.csv_FileName;
    //    return dialogue.dialogues;
    //}

    float Dist = 0f;
    private void Update()
    {
        Dist = Vector3.Distance(this.transform.position, player.transform.position);
        if (Dist <= 13f)
        {
            //Debug.Log("���� ���� �Ǹ� ��ũ ���");

            //��ũ�� �� ���¿��� Ŭ�� ���� �� ���� �߰�
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "NPC")
                    {
                        Debug.Log("NPC");

                        //TEST
                        if (!dialoguePanel.gameObject.activeSelf)
                        {
                            dialoguePanel.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void NextDialogue()
    {
        if (dialogue.dialogues.Length -1 > cur_Dialogue_Index)
        {
            dialogueCharacter.text = dialogue.dialogues[cur_Dialogue_Index].name;
            dialogueContents.text = dialogue.dialogues[cur_Dialogue_Index].contexts[0];
            cur_Dialogue_Index++;
            BtnText.text = "����";
        }
        else if(dialogue.dialogues.Length - 1 == cur_Dialogue_Index)
        {
            dialogueCharacter.text = dialogue.dialogues[cur_Dialogue_Index].name;
            dialogueContents.text = dialogue.dialogues[cur_Dialogue_Index].contexts[0];
            cur_Dialogue_Index++;
            BtnText.text = "�ݱ�";
        }
        else
        {
            if(dialoguePanel.gameObject.activeSelf)
            {
                //������ �ʱ�ȭ
                cur_Dialogue_Index = 0;
                Debug.Log("�ݱ�");
                dialoguePanel.gameObject.SetActive(false);
            }
        }

        if (cur_Dialogue_Index == event_Dialogue_Index)
        {
            Debug.Log("DB ����");
           // QuestDatabaseManager.SendUpdateNpcCode(1, 4);
        }
    }
}
