using UnityEngine;
using WebSocketSharp;

[System.Serializable]
public class QuestObtainStateDataTable
{
    public int questId;
    public int npcId;
    public bool questObtainConditionAchieveIs;
}

[System.Serializable]
public class QuestObtainDataTable
{
    public int questId;
    public bool questCompletionConditionAchieveIs;
    public int relatedQuestId;
    public string quest_progress;
}

[System.Serializable]
public class QuestScriptTable
{
    public int questScriptId;
    public string[] questScript;
    public int[] questScriptCode;
}

[System.Serializable]
public class QuestProgressScriptTable
{
    public int questProgressScriptId;
    public string[] questProgressScript;
    public int[] questProgressScriptCode;
}

[System.Serializable]
public class QuestStaticInfoTable
{
    public int quest_id;
    public int quest_code;
    public int npc_id;
    public string quest_name;
    public int quest_obtain_base_level;
    public bool quest_obtain_base_level_condition;
    public int quest_obtain_job_level;
    public bool quest_obtain_job_level_condition;
    public int quest_obtain_job_code;
    public int quest_obtain_gender_code;
    public bool quest_obtain_pre_quest_condition;
    public int quest_script_id;
    public int quest_progress_code;
    public int quest_bubble_ui_code;
    public int quest_obtain_index;
    public int quest_obtain_script_id;
    public int quest_completed_index;
    public int quest_completed_script_id;
    public int related_quest_id;
    public bool quest_select_has;
    public int[] quest_select_index;
    public int[] quest_select_script_id;
    public bool quest_textfield_has;
    public int[] quest_textfield_index;
    public int[] quest_textfield_correct;
    public int[] quest_textfield_script_id;
    public bool quest_production_has;
    public int[] quest_production_index;
    public int[] quest_production_code;
    public bool quest_ui_has;
    public int[] quest_ui_appear_index;
    public int[] quest_ui_id;
    public int[] quest_ui_disappear_index;
    public bool quest_event_has;
    public int[] quest_event_index;
    public int[] quest_event_code;
    public bool quest_reward_related_has;
    public int quest_reward_related_index;
    public int quest_reward_related_quest_id;
    public string quest_explanation;
    public string[] quest_detail_explanation;
    public int quest_icon_code;
    public int quest_progress_script_id;
    public bool quest_progress_select_has;
    public int[] quest_progress_select_index;
    public int[] quest_progress_select_script_id;
    public bool quest_progress_textfield_has;
    public int[] quest_progress_textfield_index;
    public int[] quest_progress_textfield_correct;
    public int[] quest_progress_textfield_script_id;
    public bool quest_progress_production_has;
    public int[] quest_progress_production_index;
    public int[] quest_progress_production_code;
    public bool quest_progress_ui_has;
    public int[] quest_progress_ui_appear_index;
    public int[] quest_progress_ui_code;
    public int[] quest_progress_ui_disappear_index;
    public bool quest_progress_event_has;
    public int[] quest_progress_event_index;
    public int[] quest_progress_event_code;
    public int[] quest_completion_condition_monster_id;
    public bool quest_completion_monster_special_item_has;
    public int quest_completion_special_item_id;
    public int quest_completion_special_item_drop_code;
    public int[] quest_completion_monster_count;
    public int quest_completion_zeny_count;
    public int[] quest_completion_item_id;
    public int[] quest_completion_item_count;
    public int[] quest_completion_quest_id;
    public int quest_completion_time;
    public int quest_reward_base_exp;
    public int quest_reward_job_exp;
    public int quest_reward_zeny;
    public int[] quest_reward_item;
    public int[] quest_reward_item_count;
    public int[] quest_reward_quest_id;
    public int quest_reward_guild_point;
    public int quest_reward_guild_coin;
    public int[] quest_reward_skill_id;
    public int[] quest_reward_skill_level;
}

[System.Serializable]
public class QuestCompletionData
{
    public int[] questCompletionCodeData;
}

[System.Serializable]
public class QuestObtainStateData
{
    public QuestObtainStateDataTable[] questObtainStateData;
}

[System.Serializable]
public class QuestObtainData
{
    public QuestObtainDataTable[] questObtainData;
}

[System.Serializable]
public class QuestScriptData
{
    public QuestScriptTable[] questScriptTableData;
}

[System.Serializable]
public class QuestProgressScriptData
{
    public QuestProgressScriptTable[] questProgressScriptTableData;
}

[System.Serializable]
public class QuestStaticInfoData
{
    public QuestStaticInfoTable[] questStaticInfoTableData;
}

[System.Serializable]
public class UpdateDatabase
{
    public string function;
    public int questCode;
    public bool newBool;
    public string newString;
    public string[] newStringArray;
}
public class QuestDatabaseManager : MonoBehaviour
{
    public QuestCompletionData completionData;
    public QuestObtainStateData obtainStateData;
    public QuestObtainData obtainData;
    public QuestScriptData scriptData;
    public QuestProgressScriptData progressScriptData;
    public QuestStaticInfoData staticInfoData;
    private int index;
    //private QuestManager qmInstance;
    private WebSocket ws;
    //#region UpdateDB
    //public void SendUpdateBallonAppears(int questCode, bool newBool)
    //{
    //    var message = new UpdateDatabase
    //    {
    //        function = "UpdateBallonAppears",
    //        questCode = questCode,
    //        newBool = newBool
    //    };
    //    var jsonMessage = JsonUtility.ToJson(message);
    //    ws.Send(jsonMessage);
    //    serverData.data[questCode].ballon_appears = newBool;
    //    int qc = FindListSameQC(questCode);
    //    if(qc != -1)
    //        qmInstance.questdataList[qc].ballon_appears = newBool;
    //}
    //public void SendUpdateQuestGetCondition(int questCode, bool newBool)
    //{
    //    var message = new UpdateDatabase
    //    {
    //        function = "UpdateQuestGetCondition",
    //        questCode = questCode,
    //        newBool = newBool
    //    };
    //    var jsonMessage = JsonUtility.ToJson(message);
    //    ws.Send(jsonMessage);
    //    serverData.data[questCode].quest_get_condition = newBool;
    //    int qc = FindListSameQC(questCode);
    //    if (qc != -1)
    //        qmInstance.questdataList[qc].quest_get_condition = newBool;
    //}
    //public void SendUpdateQuestGet(int questCode, bool newBool)
    //{
    //    var message = new UpdateDatabase
    //    {
    //        function = "UpdateQuestGet",
    //        questCode = questCode,
    //        newBool = newBool
    //    };
    //    var jsonMessage = JsonUtility.ToJson(message);
    //    ws.Send(jsonMessage);
    //    serverData.data[questCode].quest_get = newBool;
    //    int qc = FindListSameQC(questCode);
    //    if (qc != -1)
    //        qmInstance.questdataList[qc].quest_get = newBool;
    //}
    //public void SendUpdatQuestCompletionCondition(int questCode, bool newBool)
    //{
    //    var message = new UpdateDatabase
    //    {
    //        function = "UpdateQuestCompletionCondition",
    //        questCode = questCode,
    //        newBool = newBool
    //    };
    //    var jsonMessage = JsonUtility.ToJson(message);
    //    ws.Send(jsonMessage);
    //    serverData.data[questCode].quest_completion_condition = newBool;
    //    int qc = FindListSameQC(questCode);
    //    if (qc != -1)
    //        qmInstance.questdataList[qc].quest_complete_condition = newBool;
    //}
    //public void SendUpdatQuestCompletion(int questCode, bool newBool)
    //{
    //    var message = new UpdateDatabase
    //    {
    //        function = "UpdateQuestCompletion",
    //        questCode = questCode,
    //        newBool = newBool
    //    };
    //    var jsonMessage = JsonUtility.ToJson(message);
    //    ws.Send(jsonMessage);
    //    serverData.data[questCode].quest_completion = newBool;
    //    int qc = FindListSameQC(questCode);
    //    if (qc != -1)
    //        qmInstance.questdataList[qc].quest_completion = newBool;
    //}
    //public void SendUpdatQuestProgress(int questCode, string newString)
    //{
    //    var message = new UpdateDatabase
    //    {
    //        function = "UpdateQuestProgress",
    //        questCode = questCode,
    //        newString = newString
    //    };
    //    var jsonMessage = JsonUtility.ToJson(message);
    //    ws.Send(jsonMessage);
    //    serverData.data[questCode].quest_progress= newString;
    //    int qc = FindListSameQC(questCode);
    //    if (qc != -1)
    //        qmInstance.questdataList[qc].quest_progress = newString;
    //}
    //public void SendUpdateQuestDetails(int questCode, string[] newStringArray)
    //{
    //    var message = new UpdateDatabase
    //    {
    //        function = "UpdateQuestDetails",
    //        questCode = questCode,
    //        newStringArray = newStringArray
    //    };
    //    var jsonMessage = JsonUtility.ToJson(message);
    //    ws.Send(jsonMessage);
    //    serverData.data[questCode].quest_details = newStringArray;
    //    int qc = FindListSameQC(questCode);
    //    if (qc != -1)
    //        qmInstance.questdataList[qc].quest_details = newStringArray;
    //}

    //public int FindListSameQC(int questCode)
    //{
    //    for(int i = 0; i < qmInstance.questdataList.Count; i++)
    //    {
    //        if (qmInstance.questdataList[i].quest_code == questCode)
    //            return i;
    //    }
    //    return -1;
    //}
    //#endregion UpdateDB
    #region LoadDB

    private void Awake()
    {
        ws = new WebSocket("ws://localhost:7777");
        ws.Connect();
        ws.OnMessage += Call;
    }
   private void Call(object sender, MessageEventArgs e)
{
    Debug.Log($"WebSocket 메시지 수신: {e.Data}");

    // 받은 데이터가 "결과가 없습니다"인 경우, 처리를 건너뜀
    if (e.Data.Contains("\"success\":false"))
    {
        Debug.Log("서버에서 데이터를 받지 못함. 처리 건너뜀.");
        return;
    }

    // 어떤 타입의 데이터인지 확인하기 위해 JSON을 검사
    if (e.Data.Contains("\"questCompletionCodeData\""))
    {
        completionData = JsonUtility.FromJson<QuestCompletionData>(e.Data);
        Debug.Log("completionData 할당 완료");
    }
    else if (e.Data.Contains("\"questObtainStateData\""))
    {
        obtainStateData = JsonUtility.FromJson<QuestObtainStateData>(e.Data);
        Debug.Log("obtainStateData 할당 완료");
    }
    else if (e.Data.Contains("\"questObtainData\""))
    {
        obtainData = JsonUtility.FromJson<QuestObtainData>(e.Data);
        Debug.Log("obtainData 할당 완료");
    }
    else if (e.Data.Contains("\"quest_script\""))
    {
        scriptData = JsonUtility.FromJson<QuestScriptData>(e.Data);
        Debug.Log($"scriptData 할당 완료: {scriptData.questScriptTableData.Length}");
    }
    else if (e.Data.Contains("\"quest_progress_script\""))
    {
        progressScriptData = JsonUtility.FromJson<QuestProgressScriptData>(e.Data);
        Debug.Log($"progressScriptData 할당 완료: {progressScriptData.questProgressScriptTableData.Length}");
    }
    else if (e.Data.Contains("\"questStaticInfoData\""))
    {
        staticInfoData = JsonUtility.FromJson<QuestStaticInfoData>(e.Data);
        Debug.Log("staticInfoData 할당 완료");
    }
    else
    {
        Debug.LogWarning("알 수 없는 데이터 구조 수신: 처리되지 않음");
    }
}
    private void Check()
    {
        Debug.Log("여기는?");
        Debug.Log(scriptData);
        Debug.Log(scriptData.questScriptTableData.Length);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            Check();
    }
    private void Start()
    {
       // qmInstance = QuestManager.instance;
    }
    #endregion

}