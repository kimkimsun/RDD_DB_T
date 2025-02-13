using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class DialogueManager : MonoBehaviour
{
    public string dialogueCSV_name;
    public TextAsset dialogueCSV;
    public static DialogueManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            dialogueCSV_name = "izlu_Dialogue_Table";

        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            dialogueCSV_name = "Test_Dialogue2";
        }

        dialogueCSV = Resources.Load<TextAsset>(dialogueCSV_name);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueGetter();
    }

    string[,] tables;
    public int lineSize;
    public int rowSize;

    [System.Serializable]
    public class Dialogue
    {
        public string NPC_name;
        public string[] dialogues;
    }
    public Dialogue[] dialogue;
    public void DialogueGetter()
    {
        string currentText = dialogueCSV.text.Substring(0, dialogueCSV.text.Length - 1);
        string[] line = currentText.Split(new char[] { '\n' });
        lineSize = line.Length;
        rowSize = line[1].Split(new char[] { ',' }).Length;
        tables = new string[lineSize, rowSize];

        for(int i = 0; i <  lineSize; i++)
        {
            dialogue[i].NPC_name = line[i];
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
