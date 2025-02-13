using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Dialogue_NPC : MonoBehaviour
{
    [Header("NPC code")]
    public int npcCode;
    public int cur_QuestCode;

    [Space(10f)]

    //public DialogueDB_Manager characterDB;
    public int cur_Dialogue_Index = 0;
    public int event_Dialogue_Index;
    [Space(10f)]

    public GameObject player;

    //public QuestManager.TableData[] questData;

    public List<QuestManager.TableData> questData = new List<QuestManager.TableData>();

    [Header("Dialogue Show")]
    public Image dialoguePanel;
    public Text dialogueCharacter;
    public Text dialogueContents;
    public Text BtnText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //본인 NPC코드에 맞는 퀘스트 가져오기
        for (int i = 0; i < QuestManager.instance.TdataList.Count; i++)
        {
            if (QuestManager.instance.TdataList[i].TGivenpc_code == npcCode)
            {
                questData.Add(QuestManager.instance.TdataList[i]);
            }
        }
        //여기에 맞춰서 대화도 가져오면 좋을텐데
        //quest code에 대응하는 대화들
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
