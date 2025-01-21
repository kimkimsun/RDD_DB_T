using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quest
{
    public int requireLV;       //����Ʈ �䱸 ����
    public bool processing;     //����Ʈ ���������� üũ
    public string[] texts;      //����Ʈ ��ȭ��
    public int startIndex;      //����Ʈ �߱� ���� index

    
    public bool finishCondition()
    {
        //DB����?
        return false;
    }

    public void QuestStart()    //����Ʈ �߱� �Լ�
    {
        //DB�� ����Ʈ �߱� �Ǿ��ٰ� �˸���
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
            //DB���� �ش� ����Ʈ �� ���� �ٽ� �������� �ε�
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
