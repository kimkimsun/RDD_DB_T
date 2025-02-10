using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Dialogue
{
    [Tooltip("대사 캐릭터 이름")]
    public string name;

    [Tooltip("대사 내용")]
    public string[] contexts;
}

[System.Serializable]
public class DialogueEvent
{
    public string name;         //이벤트 이름


    public Vector2 line;        //대사의 시작과 끝
    public Dialogue[] dialogues;

}
