using UnityEngine;

public class InterAction_Event : MonoBehaviour
{
    public DialogueDB_Manager characterDB;
    [Space(10f)]

    [SerializeField] DialogueEvent dialogue;


    public int eventIndex;

    public Dialogue[] GetDialogue()
    {
        dialogue.dialogues = characterDB.GetDialogue((int)dialogue.line.x, (int)dialogue.line.y);
        //dialogue.name = 
        return dialogue.dialogues;
    }

    public void Awake()
    {
        dialogue.dialogues = GetDialogue();
    }
}
