using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Quest_Manager : MonoBehaviour
{
    public QuestManager QM;

    [Header("NPC Code")]
    public int NPC_Code;

    [Space(20f)]

    [Header("Quest Mark")]
    public Image questIndicator;                                                //����Ʈ ��ũ

    [Header("Dialog Panel")]
    public Image dialogPanel;                                                   //dialog Panel
    public Text dialogText;                                                     //dialog Text
    public Text dialogTextPopup;                                                //dialog Popup

    [Header("Quest Lists")]
    public List<QuestManager.Quest> quests = new List<QuestManager.Quest>();    //����Ʈ ��� from DB
    [Space(20f)]

    public int processingQuestCode;                                            //��������� ����Ʈ�� index
    public int processingQuestindex;                                            //��������� ����Ʈ�� index

    [System.Serializable]
    public class Dialog
    {
        public bool dialogType = false;     //false =  ��ǳ�� ��ȭ, true = TextBox ��ȭ
        public string dialog = null;        //����Ʈ ���� ����
    }

    [System.Serializable]
    public class QuestValue
    {
        public bool questGet = false;           //����Ʈ ȹ�� ���� -> DB�� ����
        public int questGetIndex = 0;       //����Ʈ ȹ�� ����
        [Header("Quest Get Conditions")]
        public bool requireLevel;               //����Ʈ ȹ�� ���� : ���������� �ִ���
        public int condition_Level;             //����Ʈ ȹ�� ���� : ����

        public bool connectQuest;               //����Ʈ ȹ�� ���� : ���� ����Ʈ
        public int preQuestCode;                //����Ʈ ȹ�� ���� : ���� ����Ʈ�� ���� ����Ʈ �ڵ�

        public bool itemQuest;                  //����Ʈ ȹ�� ���� : �䱸 ������
        public string[] requireItemLis;         //����Ʈ ȹ�� ���� : �䱸 �����۸���Ʈ

        [Space(10f)]
        [Header("Quest Processing Check")]
        public bool questProcessing = false;    //����Ʈ ���� ���� -> DB�� ����

        [Space(10f)]
        [Header("Quest Finish Conditions")]
        public bool questFinish = false;        //����Ʈ �Ϸ� ���� -> DB�� ����
        public int questFinishIndex = 0;    //����Ʈ �Ϸ� ����

        [Space(10f)]
        [Header("Quest Details")]
        public Text[] questDetail = null;       //����Ʈ ���� ���� -> DB�� ����

        [Space(10f)]
        [Header("Quest Information")]
        public int QuestCode;   //��ȭ�� ���� �ɶ�, NPC_Quest_Manager.processingQuestIndex�� ���� ã�ƿ;���

        public bool choiceDialogStart;      //N�� ���� ��ȭ ����
        public int[] choiceDialogIndex;     //N�� ���� ��ȭ ���� ������

        public List<Dialog> dialogs = new List<Dialog>();       //��ȭ ���

        [Space(10f)]
        [Header("Quest Info Images")]
        public Image descriptImg = null;    //���� �̹��� => imgList���⿡�� �ٲ�ġ�� ��
        public int[] imageShowIndex;        //imageShowIndex.count > 0 �� ��, ���� ������ ������ ����
        public Image[] imgList = null;      //������ image index�� ���� �� �ش� �̹��� ����Ʈ���� �ε����� �´� �̹��� ������
    }

    //����Ʈ ȹ������ ���� ����
    //����Ʈ ȹ�� ���� ����
    //����Ʈ �Ϸ� ����
    //����Ʈ �Ϸ� ���� ����

    [Space(20f)]
    [Header("Quest Dialog Lists")]
    public List<QuestValue> questValues = new List<QuestValue>();

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
            Debug.Log("����Ʈ ��ȭ ����");

            for(int i = 0; i < questValues.Count; i++)
            {
                if(questValues[i].condition_Level <= playerController.Lv)
                {
                    processingQuestindex = i;
                    processingQuestCode = questValues[i].QuestCode;
                    //DB�� ���� �������� ����Ʈ �ֱ�
                }
            }

            int dialogIndex = 0;

            //����Ʈ dialogs�� ����ִ� dialog ���� ��ŭ ��ȭ ����
            while (dialogIndex < questValues[processingQuestindex].dialogs.Count)
            {
                //����Ʈ �߱� ������ �Ǹ� ����Ʈ �߱�
                if (dialogIndex == questValues[processingQuestindex].questGetIndex && !questValues[processingQuestindex].questGet)
                {
                    questValues[processingQuestindex].questGet = true;
                    Debug.Log("����Ʈ �߱�!!!");
                }

                //��ȭ type�� ���� true�� textBox��ȭ false�� ��ǳ�� ��ȭ
                if (questValues[processingQuestindex].dialogs[dialogIndex].dialogType)
                {
                    dialogText.text = questValues[processingQuestindex].dialogs[dialogIndex].dialog;
                }
                else
                {
                    dialogText.text = "��ǳ�� ����" + questValues[processingQuestindex].dialogs[dialogIndex].dialog;
                }

                //GŰ�� ������ ���� ��ȭ�� ����
                if (Input.GetKeyDown(KeyCode.G))
                {
                    dialogIndex++;
                }

                yield return null;
            }
            dialogPanel.gameObject.SetActive(false);
        }
    }
}
