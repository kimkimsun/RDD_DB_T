using System.Collections.Generic;
using UnityEngine;

public class DialogueDB_Manager : MonoBehaviour
{
    public static DialogueDB_Manager instance;

    [SerializeField] string csv_FileName;

    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();

    public static bool isFinish = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Dialogue_Parser theParser= GetComponent<Dialogue_Parser>();
            Dialogue[] dialogues = theParser.Parse(csv_FileName);

            for(int i = 0; i < dialogues.Length; i++)
            {
                dialogueDic.Add(i+1, dialogues[i]);
            }

            isFinish = true;
        }
    }

    public Dialogue[] GetDialogue(int _StartNum, int _EndNum)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        for(int i = 0; i <= _EndNum - _StartNum; i++)
        {
            dialogueList.Add(dialogueDic[_StartNum + i]);
        }

        return dialogueList.ToArray();
    }
}
