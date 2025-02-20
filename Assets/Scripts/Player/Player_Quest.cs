using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_Quest : MonoBehaviour
{
    [Header("Quest Panel")]
    public Image questPanel;

    public Text questTitle;
    public Text questSubTitle;

    [Space(10f)]

    [Header("Quest Detail Panel")]
    public Image questDetails;
    public List<Image> questList = new List<Image>();
    public bool toggle;

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
        questDetails.gameObject.SetActive(true);
    }
}
