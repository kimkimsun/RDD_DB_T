using UnityEngine;
using UnityEngine.UI;

public class InterAction_Event : MonoBehaviour
{
    //public DialogueDB_Manager characterDB;
    [Space(10f)]

    [SerializeField] DialogueEvent dialogue;


    public int eventIndex = 0;

    [Header("Dialogue Show")]
    public Text dialogueCharacter;
    public Text dialogueContents;
    public Text BtnText;

    public Dialogue[] GetDialogue()
    {
        dialogue.dialogues = QuestManager.instance.characterDB.GetDialogue((int)dialogue.line.x, (int)dialogue.line.y);
        dialogue.name = QuestManager.instance.characterDB.csv_FileName;
        return dialogue.dialogues;
    }

    public void Awake()
    {
    }

    private void Start()
    {
        //dialogue.dialogues = QuestManager.instance.characterDB.GetDialogue((int)dialogue.line.x, (int)dialogue.line.y);
        //dialogue.name = QuestManager.instance.characterDB.csv_FileName;
        dialogue.dialogues = GetDialogue();
    }

    private void Update()
    {

    }

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
            Debug.Log("¥›±‚");
        }
    }
}
