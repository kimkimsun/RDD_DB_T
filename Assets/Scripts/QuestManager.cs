using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public DialogueDB_Manager characterDB;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        if(characterDB == null)
        {
            characterDB = GetComponent<DialogueDB_Manager>();
        }

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            characterDB.csv_FileName = "Test_Dialogue";
        }
        else if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            characterDB.csv_FileName = "Test_Dialogue2";
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
}
