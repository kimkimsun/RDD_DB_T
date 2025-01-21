using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quest
{

    public int QuestCode;        //퀘스트 코드
    public int requireLV;       //퀘스트 요구 레벨
    public bool processing;     //퀘스트 진행중인지 체크
    public string[] texts;      //퀘스트 대화들


    public int startIndex;      //퀘스트 발급 시점 index    
    public bool finishCondition()
    {
        //DB연동?
        //이게 true를 return하게 되면 완료된거임
        return false;
    }

    public void QuestStart()    //퀘스트 발급 함수
    {
        //DB에 퀘스트 발급 되었다고 알리기
    }
}


public class NPC_Quest_Manager : MonoBehaviour
{
    public Image questIndicator;                //퀘스트 마크
    public Quest[] quests;                      //퀘스트들 리스트

    public Player_Controller playerController;  //플레이어

    public int processingQuestIndex;            //진행시작한 퀘스트의 index

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
    //void Update()
    //{
        
    //}

    public bool pEnter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            pEnter = true;
            for (int i = 0; i < quests.Length; i++)
            {
                if (quests[i].requireLV <= playerController.Lv)
                {
                    processingQuestIndex = i;       //진행하는 퀘스트의 index 가져오기

                    questIndicator.enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pEnter = false;
            if (questIndicator.enabled)
            {
                questIndicator.enabled = false;
            }
        }
    }

    Coroutine dialog;
    public IEnumerator QuestDialog()    //퀘스트 대화 진행 coroutine
    {
        quests[processingQuestIndex].processing = true;

        Debug.Log("퀘스트 대화 시작");
        yield return null;
    }

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(pEnter)
        {
            if (questIndicator.enabled)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (dialog == null)
                    {
                        dialog = StartCoroutine(QuestDialog());
                    }
                    else
                    {
                        StopCoroutine(dialog);
                        dialog = StartCoroutine(QuestDialog());
                    }
                }
            }
            else
            {
                //Default 대화
            }
        }
    }
}
