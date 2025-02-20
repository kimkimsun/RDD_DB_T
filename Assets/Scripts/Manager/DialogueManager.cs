using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using NUnit.Framework;



public class DialogueManager : MonoBehaviour
{
    public TextAsset dialogueCSV;
    public TextAsset processingCSV;
    public static DialogueManager instance;

    private string dialogueCSV_name;
    private string precessingCSV_name;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        precessingCSV_name = "processing_dialogue";
        //tableCSV값 어떻게 정해줄 지 작성 필요 + Dont destroy On Load 제작 필요

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            dialogueCSV_name = "island_Dialogue_Table";
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            dialogueCSV_name = "island_Dialogue_Table";
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            dialogueCSV_name = "island_Dialogue_Table";
        }
        //Debug.Log("첫번째");

        dialogueCSV = Resources.Load<TextAsset>(dialogueCSV_name);
        processingCSV = Resources.Load<TextAsset>(precessingCSV_name);
    }
    void Start()
    {
        DialogueGetter();
    }

    [System.Serializable]
    public class Dialogue
    {
        public int quest_code;
        public string NPC_name;
        public string dialogues;
        public int dialogueType;
    }

    List<int> questCode = new List<int>();
    List<string> nameList = new List<string>();
    List<string> dialogues = new List<string>();
    List<int> dialogueType = new List<int>();

    [Header("Area Dialogue List")]
    public List<Dialogue> dialogueList = new List<Dialogue>();

    string[,] tables;
    int lineSize;
    int rowSize;


    public void DialogueGetter(TextAsset a)
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

        for(int i = 1; i  < lineSize; i++)
        {
            string[] data = tables[i, 0].Split(',');
            questCode.Add(int.Parse(data[0]));
            nameList.Add(data[1]);
            dialogues.Add(data[2]);
            dialogueType.Add(int.Parse(data[3]));
        }


        for (int i = 0; i < lineSize-1; i++)
        {
            Dialogue tempDialogue = new Dialogue();
            tempDialogue.quest_code = questCode[i];
            tempDialogue.NPC_name = nameList[i];
            tempDialogue.dialogues = dialogues[i];
            tempDialogue.dialogueType = dialogueType[i];
            dialogueList.Add(tempDialogue);
        }
    }
}
