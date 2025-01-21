using UnityEngine;
using WebSocketSharp;
public class WsClient : MonoBehaviour
{
    WebSocket ws;
    private void Start()
    {
        ws = new WebSocket("ws://localhost:7777");
        //�������� ������ ��Ʈ�� �־��ݴϴ�.


        ws.Connect();
        //�����մϴ�.
        ws.OnMessage += Call;
        //�̺�Ʈ �߰�
        /*
         ���� ���� ��
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("�ּ� :  "+((WebSocket)sender).Url+", ������ : "+e.Data);
        };
         */
    }

    void Call(object sender, MessageEventArgs e)
    {
        Debug.Log("�ּ� :  " + ((WebSocket)sender).Url + ", ������ : " + e.Data);
    }
    private void Update()
    {
        if (ws == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("abcd");
            //�����͸� �����ϴ� �����̱� ������ "abcd" �� �����ϴ�
        }
    }
}