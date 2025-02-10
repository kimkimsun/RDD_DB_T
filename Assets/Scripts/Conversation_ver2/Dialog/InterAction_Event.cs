using UnityEngine;

public class InterAction_Event : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogue;

    public Dialogue[] GetDialogue()
    {
        dialogue.dialogues = DialogueDB_Manager.instance.GetDialogue((int)dialogue.line.x, (int)dialogue.line.y);

        return dialogue.dialogues;
    }
}
