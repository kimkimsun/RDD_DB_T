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

        //본인 NPC코드에 맞는 퀘스트 가져오기
        for (int i = 0; i < QuestManager.instance.TQuestdataList.Count; i++)
        {
            if (QuestManager.instance.TQuestdataList[i].TGivenpc_code == npcCode)
            {
                questData.Add(QuestManager.instance.TQuestdataList[i]);
            }
        }
        //여기에 맞춰서 대화도 가져오면 좋을텐데
        //quest code에 대응하는 대화들
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
            //Debug.Log("일정 조건 되면 마크 띄움");

            //마크가 뜬 상태에서 클릭 했을 시 조건 추가
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
            BtnText.text = "다음";
        }
        else if(dialogue.dialogues.Length - 1 == cur_Dialogue_Index)
        {
            dialogueCharacter.text = dialogue.dialogues[cur_Dialogue_Index].name;
            dialogueContents.text = dialogue.dialogues[cur_Dialogue_Index].contexts[0];
            cur_Dialogue_Index++;
            BtnText.text = "닫기";
        }
        else
        {
            if(dialoguePanel.gameObject.activeSelf)
            {
                //나머지 초기화
                cur_Dialogue_Index = 0;
                Debug.Log("닫기");
                dialoguePanel.gameObject.SetActive(false);
            }
        }

        if (cur_Dialogue_Index == event_Dialogue_Index)
        {
            Debug.Log("DB 전달");
           // QuestDatabaseManager.SendUpdateNpcCode(1, 4);
        }
    }
}
