using UnityEngine;

public class textManager : MonoBehaviour
{
    public TextAsset txt;
    string[,] Sentence;
    int lineSize;
    int rowSize;

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

        for(int i = 0; i < lineSize; i++)
        {
            string[] row = line[i].Split("\t");
            for(int j = 0; j < rowSize; j++)
            {
                Sentence[i,j]  = row[j];
                print(i + "," + j + "," + Sentence[i,j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
