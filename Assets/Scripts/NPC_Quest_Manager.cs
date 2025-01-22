using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Quest_Manager : MonoBehaviour
{
    public QuestManager QM;

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

    [Header("NPC Code")]
    public int NPC_Code;

    [Space(20f)]

    [Header("Quest Mark")]
    public Image questIndicator;                                                //����Ʈ ��ũ

    [Header("Dialog Panel")]
    public Image dialogPanel;                                                   //dialog Panel

    [Header("Quest Lists")]
    public List<QuestManager.Quest> quests = new List<QuestManager.Quest>();    //����Ʈ ��� from DB
    //public QuestConditions getQuestconditions;                                  //����Ʈ ȹ�� ���� ����
    [Space(20f)]

    public int processingQuestIndex;                                            //��������� ����Ʈ�� index

    public class Dialog
    {
        public bool dialogType = false;     //false =  ��ǳ�� ��ȭ, true = TextBox ��ȭ
        public string dialog = null;      //����Ʈ ���� ����
    }

    [System.Serializable]
    public class QuestValue
    {
        public int QuestCode;   //��ȭ�� ���� �ɶ�, NPC_Quest_Manager.processingQuestIndex�� ���� ã�ƿ;���
        public List<Dialog> dialogs = new List<Dialog>();       //��ȭ ���

        public bool questGet = false;           //����Ʈ ȹ�� ���� -> DB�� ����
        public bool questProcessing = false;    //����Ʈ ���� ���� -> DB�� ����
        public bool questFinish = false;        //����Ʈ �Ϸ� ���� -> DB�� ����
        public Text[] questDetail = null;       //����Ʈ ���� ���� -> DB�� ����

        public int questGetIndex = 0;       //����Ʈ ȹ�� ����
        public int questFinishIndex = 0;    //����Ʈ �Ϸ� ����

        public Image descriptImg = null;    //���� �̹��� => imgList���⿡�� �ٲ�ġ�� ��
        public int imageShowIndex = 0;      //������ != null�� ��, ���� ������ ����
        public int choiceDialogIndex = 0;   //N�� ���� ��ȭ ���� ����
    }

    //����Ʈ ȹ������ ���� ����
    //����Ʈ ȹ�� ���� ����
    //����Ʈ �Ϸ� ����
    //����Ʈ �Ϸ� ���� ����

    [Space(20f)]
    [Header("Quest Dialog Lists")]
    public List<QuestValue> questValues = new List<QuestValue>();
    public Image[] imgList = null;

    [Space(10f)]
    public Player_Controller playerController;  //�÷��̾�
    public bool pEnter;
    public BoxCollider detectCol;


    private void Awake()
    {
        playerController = GameObject.FindAnyObjectByType<Player_Controller>();
        QM = FindFirstObjectByType<QuestManager>();
        detectCol = GetComponent<BoxCollider>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Quest Manager���� ������ NPC_Code�� �´� ����Ʈ�� ã�ƿ���
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

        if(Vector3.Distance(this.transform.position, playerController.gameObject.transform.position) < 10f)
        {
            pEnter = true;
            if (!questIndicator.enabled)
            {
                questIndicator.enabled = true;
            }
        }
        else
        {
            pEnter = false;
            if (questIndicator.enabled)
            {
                questIndicator.enabled = false;
            }
        }
    }

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(pEnter)
        {
            if (questIndicator.enabled)
            {
                // processingQuestIndex�� ���� ���� ��� ������ ���ΰ�.

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("NPC") && !hit.collider.isTrigger)
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

    Coroutine dialog;
    public IEnumerator QuestDialog()    //����Ʈ ��ȭ ���� coroutine
    {
        if(dialogPanel != null && !dialogPanel.gameObject.activeSelf)
        {
            dialogPanel.gameObject.SetActive(true);
        }
        Debug.Log("����Ʈ ��ȭ ����");
        yield return null;
    }
}
