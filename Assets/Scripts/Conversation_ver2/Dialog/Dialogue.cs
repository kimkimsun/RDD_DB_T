using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Dialogue
{
    [Tooltip("��� ĳ���� �̸�")]
    public string name;

    [Tooltip("��� ����")]
    public string[] contexts;
}

[System.Serializable]
public class DialogueEvent
{
    public string name;         //�̺�Ʈ �̸�


    public Vector2 line;        //����� ���۰� ��
    public Dialogue[] dialogues;

}
