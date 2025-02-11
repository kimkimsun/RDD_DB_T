using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InterAction_Event : MonoBehaviour
{
    //public DialogueDB_Manager characterDB;
    public int curIndex = 0;
    public int eventIndex;
    [Space(10f)]

    public GameObject player;

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
        dialogue.dialogues = GetDialogue();
    }
    public Dialogue[] GetDialogue()
    {
        dialogue.dialogues = QuestManager.instance.characterDB.GetDialogue((int)dialogue.line.x, (int)dialogue.line.y);
        dialogue.name = QuestManager.instance.characterDB.csv_FileName;
        return dialogue.dialogues;
    }

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
        if (dialogue.dialogues.Length -1 > curIndex)
        {
            dialogueCharacter.text = dialogue.dialogues[curIndex].name;
            dialogueContents.text = dialogue.dialogues[curIndex].contexts[0];
            curIndex++;
            BtnText.text = "다음";
        }
        else if(dialogue.dialogues.Length - 1 == curIndex)
        {
            dialogueCharacter.text = dialogue.dialogues[curIndex].name;
            dialogueContents.text = dialogue.dialogues[curIndex].contexts[0];
            curIndex++;
            BtnText.text = "닫기";
        }
        else
        {
            if(dialoguePanel.gameObject.activeSelf)
            {
                //나머지 초기화
                curIndex = 0;
                Debug.Log("닫기");
                dialoguePanel.gameObject.SetActive(false);
            }
        }

        if (curIndex == eventIndex)
        {
            Debug.Log("DB 전달");
            QuestDatabaseManager.SendUpdateNpcCode(1, 4);
        }
    }
}
