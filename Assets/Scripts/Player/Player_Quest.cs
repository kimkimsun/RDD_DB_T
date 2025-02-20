using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_Quest : MonoBehaviour
{
    public bool toggle;
    [Header("Quest Panel")]
    public Image questPanel;
    public Image questIcon;

    public Text questTitle;
    public Text questSubTitle;

    public List<Image> questList = new List<Image>();

    [Space(10f)]

    [Header("Quest Detail Panel")]
    public Image questdetailPanel;
    public string[] detailGroup = new string[4];

    private void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            toggle = !toggle;
            questPanel.gameObject.SetActive(toggle);
        }
    }

    public void QuestDetiailOn()
    {
        questdetailPanel.gameObject.SetActive(true);
    }

    public void QuestDataInIt(Sprite Icon, string title, string describe, string[] details)
    {

    }

}
