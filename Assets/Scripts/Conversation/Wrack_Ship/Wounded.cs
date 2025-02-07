using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Wounded : MonoBehaviour
{
    public TextAsset txt;
    public string[,] Sentence;
    public int lineSize;
    public int rowSize;

    public Image textPanel;
    public Text conversation_character;
    public Text conversation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //엔터단위와 탭으로 나눠서 배열의 크기 조정
        string currentText = txt.text.Substring(0, txt.text.Length - 1);
        print(currentText);
        string[] line = currentText.Split('\n');
        lineSize = line.Length;
        rowSize = line[0].Split('\t').Length;
        Sentence = new string[lineSize, rowSize];

        for (int i = 0; i < lineSize; i++)
        {
            string[] row = line[i].Split("\t");
            for (int j = 0; j < rowSize; j++)
            {
                Sentence[i, j] = row[j];
                print(i + "," + j + "," + Sentence[i, j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    int charact = 0;
    int sen = 1;
    int check = 2;
    public Text BtnTxt;

    public void NextSentence()
    {
        conversation_character.text = "[" + Sentence[charact, 0].ToString() + "]";
        conversation.text = Sentence[charact, sen].ToString();

        //if (Sentence[charact,check].ToString() == "TRUE")
        //{
        //    Debug.Log(Sentence[charact, check]);
        //    conversation.text = Sentence[charact, sen].ToString();
        //}
        //else
        //{
        //    Debug.Log(Sentence[charact, check].ToString());
        //    conversation.text = Sentence[charact, sen].ToString();
        //}
        charact++;

        if (charact == lineSize-1)
        {
            BtnTxt.text = "닫기";
        }
        else if(charact == lineSize)
        {
            textPanel.gameObject.SetActive(false);
        }
        else
        {

        }
    }
}
