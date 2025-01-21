using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quest
{
    public int QuestCode;        //����Ʈ �ڵ�
    public int requireLV;       //����Ʈ �䱸 ����
    public bool processing;     //����Ʈ ���������� üũ
    public string[] texts;      //����Ʈ ��ȭ��


    public int startIndex;      //����Ʈ �߱� ���� index    
    public bool finishCondition()
    {
        //DB����?
        //�̰� true�� return�ϰ� �Ǹ� �Ϸ�Ȱ���
        return false;
    }

    public void QuestStart()    //����Ʈ �߱� �Լ�
    {
        //DB�� ����Ʈ �߱� �Ǿ��ٰ� �˸���
    }
}


public class NPC_Quest_Manager : MonoBehaviour
{
    [Header("NPC Code")]
    public int NPC_Code;

    [Space(10f)]

    public Image questIndicator;                //����Ʈ ��ũ
    //public Quest[] quests;                      //����Ʈ�� ����Ʈ
    public List<QuestManager.Quest> quests = new List<QuestManager.Quest>();

    public Player_Controller playerController;  //�÷��̾�

    public int processingQuestIndex;            //��������� ����Ʈ�� index

    public QuestManager QM;

    private void Awake()
    {
        playerController = GameObject.FindAnyObjectByType<Player_Controller>();
        QM = FindFirstObjectByType<QuestManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //QM.RequestQuestData(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < QM.quests.Count; i++)
            {
                if (QM.quests[i].NpcCode == NPC_Code)
                {
                    quests.Add(QM.quests[i]);
                }
            }
        }
    }

    public bool pEnter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            pEnter = true;

            questIndicator.enabled = true;
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
    public IEnumerator QuestDialog()    //����Ʈ ��ȭ ���� coroutine
    {
        Debug.Log("����Ʈ ��ȭ ����");
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
                //Default ��ȭ
            }
        }
    }
}
