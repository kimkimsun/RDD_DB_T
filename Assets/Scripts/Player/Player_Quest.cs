using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_Quest : MonoBehaviour
{
    public Image questPanel;
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
}
