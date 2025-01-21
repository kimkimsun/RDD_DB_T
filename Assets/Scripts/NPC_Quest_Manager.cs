using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quest
{
    public int requireLV;       //퀘스트 요구 레벨
    public bool processing;     //퀘스트 진행중인지 체크
    public string[] texts;      //퀘스트 대화들
    public int startIndex;      //퀘스트 발급 시점 index

    
    public bool finishCondition()
    {
        //DB연동?
        return false;
    }

    public void QuestStart()    //퀘스트 발급 함수
    {
        //DB에 퀘스트 발급 되었다고 알리기
    }
}


public class NPC_Quest_Manager : MonoBehaviour
{
    public Image questIndicator;
    public Quest[] quests;

    public Player_Controller playerController;

    private void Awake()
    {
        playerController = GameObject.FindAnyObjectByType<Player_Controller>();

        Init();
    }

    void Init()
    {
        for(int i = 0; i < quests.Length; i++)
        {
            //DB에서 해당 퀘스트 별 정보 다시 가져오기 로딩
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            for(int i = 0; i < quests.Length; i++)
            {
                if (quests[i].requireLV <= playerController.Lv)
                {
                    questIndicator.enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(questIndicator.enabled)
            {
                questIndicator.enabled = false;
            }
        }
    }
}
