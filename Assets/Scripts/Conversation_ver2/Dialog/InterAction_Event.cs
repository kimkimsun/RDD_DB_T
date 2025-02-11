using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InterAction_Event : MonoBehaviour
{
    //public DialogueDB_Manager characterDB;
    public int eventIndex = 0;
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

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "NPC") // 악마만두 클릭 시
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
        if (dialogue.dialogues.Length -1 > eventIndex)
        {
            dialogueCharacter.text = dialogue.dialogues[eventIndex].name;
            dialogueContents.text = dialogue.dialogues[eventIndex].contexts[0];
            eventIndex++;
            BtnText.text = "다음";
        }
        else if(dialogue.dialogues.Length - 1 == eventIndex)
        {
            dialogueCharacter.text = dialogue.dialogues[eventIndex].name;
            dialogueContents.text = dialogue.dialogues[eventIndex].contexts[0];
            eventIndex++;
            BtnText.text = "닫기";
        }
        else
        {
            if(dialoguePanel.gameObject.activeSelf)
            {
                Debug.Log("닫기");
                dialoguePanel.gameObject.SetActive(false);
            }
        }
    }
}
