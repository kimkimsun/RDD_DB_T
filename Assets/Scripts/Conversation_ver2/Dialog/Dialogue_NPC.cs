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

    //public DialogueDB_Manager characterDB;
    public int cur_Dialogue_Index = 0;
    public int event_Dialogue_Index;
    [Space(10f)]

    public GameObject player;

    //public QuestManager.TableData[] questData;

    public List<QuestManager.TableData> questData = new List<QuestManager.TableData>();
    public List<DialogueManager.Dialogue> dialogueData = new List<DialogueManager.Dialogue>();

    [Header("Dialogue Show")]
    public Canvas questUI;
    public Image questBaloonUI;
    //public Image dialoguePanel;
    public Text dialogueCharacter;
    public Text dialogueContents;
    public Text BtnText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //본인 NPC코드에 맞는 퀘스트 가져오기
        for (int i = 0; i < QuestManager.instance.TQuestdataList.Count; i++)
        {
            if (QuestManager.instance.TQuestdataList[i].TGivenpc_code == npcCode)
            {
                questData.Add(QuestManager.instance.TQuestdataList[i]);
            }
        }

        //Debug.Log(DialogueManager.instance.dialogueList.Count);
        //여기에 맞춰서 대화도 가져오면 좋을텐데
        for (int i = 0; i < DialogueManager.instance.dialogueList.Count; i++)
        {
            if (DialogueManager.instance.dialogueList[i].quest_code == cur_QuestCode)
            {
                dialogueData.Add(DialogueManager.instance.dialogueList[i]);
            }
        }
        //quest code에 대응하는 대화들
    }

    float Dist = 0f;
    // Update is called once per frame
    void Update()
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
                        Debug.Log(hit.transform.gameObject.name);

                        //TEST
                        if (!questUI.gameObject.activeSelf)
                        {
                            questUI.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void NextDialogue()
    {
        if(dialogueData.Count -1 > cur_Dialogue_Index)
        {
            dialogueCharacter.text = dialogueData[cur_Dialogue_Index].NPC_name;
            dialogueContents.text = dialogueData[cur_Dialogue_Index].dialogues;
            BtnText.text = "다음";
            cur_Dialogue_Index++;
        }
        else if(dialogueData.Count - 1 == cur_Dialogue_Index)
        {
            dialogueCharacter.text = dialogueData[cur_Dialogue_Index].NPC_name;
            dialogueContents.text = dialogueData[cur_Dialogue_Index].dialogues;
            BtnText.text = "닫기";
            cur_Dialogue_Index++;
        }
        else
        {
            if (questUI.gameObject.activeSelf)
            {
                //나머지 초기화
                cur_Dialogue_Index = 0;
                questUI.gameObject.SetActive(false);
                Debug.Log("닫기");
            }
        }

        if (cur_Dialogue_Index == event_Dialogue_Index)
        {
            Debug.Log("DB 전달");
            // QuestDatabaseManager.SendUpdateNpcCode(1, 4);
        }

        //if (dialogueData..Length - 1 > cur_Dialogue_Index)
        //{
        //    dialogueCharacter.text = dialogue.dialogues[cur_Dialogue_Index].name;
        //    dialogueContents.text = dialogue.dialogues[cur_Dialogue_Index].contexts[0];
        //    cur_Dialogue_Index++;
        //    BtnText.text = "다음";
        //}
        //else if (dialogue.dialogues.Length - 1 == cur_Dialogue_Index)
        //{
        //    dialogueCharacter.text = dialogue.dialogues[cur_Dialogue_Index].name;
        //    dialogueContents.text = dialogue.dialogues[cur_Dialogue_Index].contexts[0];
        //    cur_Dialogue_Index++;
        //    BtnText.text = "닫기";
        //}
        //else
        //{
        //    if (dialoguePanel.gameObject.activeSelf)
        //    {
        //        //나머지 초기화
        //        cur_Dialogue_Index = 0;
        //        Debug.Log("닫기");
        //        dialoguePanel.gameObject.SetActive(false);
        //    }
        //}

        //if (cur_Dialogue_Index == event_Dialogue_Index)
        //{
        //    Debug.Log("DB 전달");
        //    // QuestDatabaseManager.SendUpdateNpcCode(1, 4);
        //}
    }
}
