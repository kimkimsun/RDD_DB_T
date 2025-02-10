using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Parser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); //��� ����Ʈ ����.
        TextAsset csvDt = Resources.Load<TextAsset>(_CSVFileName);    //csv���� ������.
        //string csvData = csvDt.text.Substring(1, csvDt.text.Length-1);
        string[] data = csvDt.text.Split(new char[] {'\n'});


        for(int i = 1;  i < data.Length;)
        {
            string[] row = data[i].Split(new char[] {','});

            Dialogue dialogue  = new Dialogue();    //��� ����Ʈ ����
            dialogue.name = row[1];
            //Debug.Log(row[1]);
            List<string> contextList = new List<string>();

            do
            {
                if (++i < data.Length)
                {
                    contextList.Add(row[2]);
                    //Debug.Log(row[2]);
                    row = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }
            }
            while (row[0].ToString() == "");

            dialogue.contexts = contextList.ToArray();

            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }
}
