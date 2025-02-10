using UnityEngine;

public class InterAction_Event : MonoBehaviour
{
    //public DialogueDB_Manager characterDB;
    [Space(10f)]

    [SerializeField] DialogueEvent dialogue;


    public int eventIndex;

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
}
