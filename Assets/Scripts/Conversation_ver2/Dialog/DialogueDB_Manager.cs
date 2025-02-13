using System.Collections.Generic;
using UnityEngine;



// 캐릭터 별 Dialogue DB

public class DialogueDB_Manager : MonoBehaviour
{
    //public static DialogueDB_Manager instance;

    public string csv_FileName;

    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();

    public static bool isFinish = false;

    //private void Awake()
    //{
    //    Dialogue_Parser theParser = GetComponent<Dialogue_Parser>();
    //    Dialogue[] dialogues = theParser.Parse(csv_FileName);

    //    for (int i = 0; i < dialogues.Length; i++)
    //    {
    //        dialogueDic.Add(i + 1, dialogues[i]);
    //    }

    //    isFinish = true;
    //}

    private void Start()
    {
        //Dialogue_Parser theParser = GetComponent<Dialogue_Parser>();
        //Dialogue[] dialogues = theParser.Parse(csv_FileName);

        //for (int i = 0; i < dialogues.Length; i++)
        //{
        //    dialogueDic.Add(i + 1, dialogues[i]);
        //}

        //isFinish = true;
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
