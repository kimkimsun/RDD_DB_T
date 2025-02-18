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
        //본인 NPC코드에 맞는 퀘스트 가져오기
        for (int i = 0; i < qmInstance.TQuestdataList.Count; i++)
        {
            if (qmInstance.TQuestdataList[i].TGivenpc_code == npcCode)
            {
                if (qmInstance.TQuestdataList[i].quest_get_condition &&
                !(qmInstance.TQuestdataList[i].quest_completion))
                {
                    questData.Add(qmInstance.TQuestdataList[i]);
                }
            }
        }

        //quest code에 대응하는 대화들
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

    int dialogueIndex = 0;
    void Update()
    {
        Dist = Vector3.Distance(this.transform.position, player.transform.position);
        if (Dist <= 13f)
        {
            #region 퀘스트 말풍선 띄우기 조건
            if (dialogueData.Count > 0)    //SendMessage(QuestGEt)
            {
                questBaloonCanvas.gameObject.SetActive(true);
                if (questData[0].Tquest_code == dialogueData[0].quest_code)
                    questBaloonUI.GetComponent<Image>().sprite = questData[0].TquestBaloon_UI;
            }
            #endregion


            //마크가 뜬 상태에서 클릭 했을 시 조건 추가
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
            if (questBaloonCanvas.gameObject.activeSelf)
            {
                questBaloonCanvas.gameObject.SetActive(false);
            }
        }
    }

    public void NextDialogue()
    {
        if (dialogueData.Count - 1 > cur_Dialogue_Index)
        {
            dialogueCharacter.text = dialogueData[cur_Dialogue_Index].NPC_name;
            dialogueContents.text = dialogueData[cur_Dialogue_Index].dialogues;
            BtnText.text = "다음";
            cur_Dialogue_Index++;
        }
        else if (dialogueData.Count - 1 == cur_Dialogue_Index)
        {
            dialogueCharacter.text = dialogueData[cur_Dialogue_Index].NPC_name;
            dialogueContents.text = dialogueData[cur_Dialogue_Index].dialogues;
            BtnText.text = "닫기";
            cur_Dialogue_Index++;
        }
        else
        {
            if (questUICanvas.gameObject.activeSelf)
            {
                //나머지 초기화
                cur_Dialogue_Index = 0;
                questUICanvas.gameObject.SetActive(false);
                Debug.Log("닫기");
            }
        }

        if (cur_Dialogue_Index == questData[0].TquestGet_index)
        {
            Debug.Log("퀘스트 획득");
            // QuestDatabaseManager.SendUpdateNpcCode(1, 4);
        }

    }
}
