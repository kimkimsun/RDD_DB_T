using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_Quest : MonoBehaviour
{
    public bool panelToggle;
    public bool detailToggle;
    [Header("Quest Panel")]
    public Image questPanel;
    public Image questIcon;

    public Text questTitle;
    public Text questSubTitle;

    public Button questBtn;
    [Space(10f)]

    [Header("Quest Detail Panel")]
    public Image questdetailPanel;
    public Text[] detailGroup = new Text[4];

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            panelToggle = !panelToggle;
            questPanel.gameObject.SetActive(panelToggle);
        }
    }

    public void QuestDetiailOn()
    {
        detailToggle = !detailToggle;
        questdetailPanel.gameObject.SetActive(detailToggle);
    }

    public void QuestDataInIt(Sprite Icon, string title, string describe, string[] details)
    {
        questIcon.sprite = Icon;
        questTitle.text = title;
        questSubTitle.text = describe;
        for (int i = 0; i < details.Length; i++)
        {
            detailGroup[i].text = details[i];
        }
    }
}