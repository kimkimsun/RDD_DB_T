using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Parser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); //��� ����Ʈ ����.
        TextAsset csvDt = Resources.Load<TextAsset>(_CSVFileName);    //csv���� ������.
        string[] data = csvDt.text.Split(new char[] {'\n'});

        for(int i = 1;  i < data.Length;)
        {
            string[] row = data[i].Split(new char[] {','});

            Dialogue dialogue  = new Dialogue();    //��� ����Ʈ ����
            dialogue.name = row[0];
            List<string> contextList = new List<string>();

            do
            {
                if (++i < data.Length)
                {
                    contextList.Add(row[1]);
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
