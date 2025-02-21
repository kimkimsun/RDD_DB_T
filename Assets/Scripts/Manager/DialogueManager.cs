using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public TextAsset dialogueCSV;
    public TextAsset processingCSV;
    public static DialogueManager instance;

    private string dialogueCSV_name;
    private string processingCSV_name;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //tableCSV값 어떻게 정해줄 지 작성 필요 + Dont destroy On Load 제작 필요

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            dialogueCSV_name = "island_Dialogue_Table";
            processingCSV_name = "island_processing_Table";
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            dialogueCSV_name = "island_Dialogue_Table";
            processingCSV_name = "island_processing_Table";
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            dialogueCSV_name = "island_Dialogue_Table";
            processingCSV_name = "island_processing_Table";
        }
        //Debug.Log("첫번째");

        dialogueCSV = Resources.Load<TextAsset>(dialogueCSV_name);
        processingCSV = Resources.Load<TextAsset>(processingCSV_name);
    }
    void Start()
    {
        DialogueGetter();
        ProcessingDialogueGetter();
    }

    [System.Serializable]
    public class Dialogue
    {
        public int quest_code;
        public string dialogues;
        public int dialogueType;
    }

    List<int> questCode = new List<int>();
    List<string> dialogues = new List<string>();
    List<int> dialogueType = new List<int>();

    [Header("Area Dialogue List")]
    public List<Dialogue> dialogueList = new List<Dialogue>();

    string[,] tables;
    int lineSize;
    int rowSize;

    [System.Serializable]
    public class Processing
    {
        public int processing_quest_code;
        public string processing_dialogues;
    }

    List<int> processingQuestCode = new List<int>();
    List<string> processingDialogues = new List<string>();

    public List<Processing> processingList = new List<Processing>();

    string[,] processingtables;
    int processinglineSize;
    int processingrowSize;

    public void DialogueGetter()
    {
        dialogueList.Clear();
        string currentText = dialogueCSV.text.Substring(0, dialogueCSV.text.Length - 1);
        string[] line = currentText.Split(new char[] { '\n' });
        lineSize = line.Length;
        rowSize = line[0].Split(new char[] { '\t' }).Length;
        tables = new string[lineSize, rowSize];

        for (int i = 0; i < lineSize; i++)
        {
            string[] row = line[i].Split(new char[] { '\t' });
            for (int j = 0; j < rowSize; j++)
            {
                tables[i, j] = row[j];
            }
        }

        for (int i = 1; i < lineSize; i++)
        {
            string[] data = tables[i, 0].Split(',');
            questCode.Add(int.Parse(data[0]));
            dialogues.Add(data[1]);
            dialogueType.Add(int.Parse(data[2]));
        }


        for (int i = 0; i < lineSize - 1; i++)
        {
            Dialogue tempDialogue = new Dialogue();
            tempDialogue.quest_code = questCode[i];
            tempDialogue.dialogues = dialogues[i];
            tempDialogue.dialogueType = dialogueType[i];
            dialogueList.Add(tempDialogue);
        }
    }
    void ProcessingDialogueGetter()
    {
        processingList.Clear();
        string currentText = processingCSV.text.Substring(0, processingCSV.text.Length - 1);
        string[] line = currentText.Split(new char[] { '\n' });
        processinglineSize = line.Length;
        processingrowSize = line[0].Split(new char[] { '\t' }).Length;
        processingtables = new string[processinglineSize, processingrowSize];

        for (int i = 0; i < processinglineSize; i++)
        {
            string[] row = line[i].Split(new char[] { '\t' });
            for (int j = 0; j < processingrowSize; j++)
            {
                processingtables[i, j] = row[j];
            }
        }

        for (int i = 1; i < processinglineSize; i++)
        {
            string[] data = processingtables[i, 0].Split(',');
            processingQuestCode.Add(int.Parse(data[0]));
            processingDialogues.Add(data[1]);
        }


        for (int i = 0; i < processinglineSize - 1; i++)
        {
            Processing tempProcessing = new Processing();
            tempProcessing.processing_quest_code = processingQuestCode[i];
            tempProcessing.processing_dialogues = processingDialogues[i];
            processingList.Add(tempProcessing);
        }
    }
}