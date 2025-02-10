using UnityEngine;
using UnityEngine.UI;

public class InterAction_Event : MonoBehaviour
{
    //public DialogueDB_Manager characterDB;
    public int eventIndex = 0;
    [Space(10f)]

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
        dialogue.dialogues = GetDialogue();
    }
    public Dialogue[] GetDialogue()
    {
        dialogue.dialogues = QuestManager.instance.characterDB.GetDialogue((int)dialogue.line.x, (int)dialogue.line.y);
        dialogue.name = QuestManager.instance.characterDB.csv_FileName;
        return dialogue.dialogues;
    }

    //private void Update()
    //{

    //}

    public void NextDialogue()
    {
        if (dialogue.dialogues.Length -1 > eventIndex)
        {
            dialogueCharacter.text = dialogue.dialogues[eventIndex].name;
            dialogueContents.text = dialogue.dialogues[eventIndex].contexts[0];
            eventIndex++;
            BtnText.text = "¥Ÿ¿Ω";
        }
        else if(dialogue.dialogues.Length - 1 == eventIndex)
        {
            dialogueCharacter.text = dialogue.dialogues[eventIndex].name;
            dialogueContents.text = dialogue.dialogues[eventIndex].contexts[0];
            eventIndex++;
            BtnText.text = "¥›±‚";
        }
        else
        {
            if(dialoguePanel.gameObject.activeSelf)
            {
                Debug.Log("¥›±‚");
                dialoguePanel.gameObject.SetActive(false);
            }
        }
    }
}
