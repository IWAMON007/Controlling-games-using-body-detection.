using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


public class ServerExample : MonoBehaviour
{
    private GameManager gameManager;

    private UdpClient udpClient;

    void Start()
    {
        // UDPクライアントの初期化
        udpClient = new UdpClient(60000);
        //受信スタート
        udpClient.BeginReceive(OnReceived, udpClient);
    }

    private void OnReceived(System.IAsyncResult result)
    {
        Debug.Log("OnReceived: " + result.ToString());
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);

        //鼻のx,y座標を受信して変数に格納
        var receivednosedata = Encoding.UTF8.GetString(getByte);

        //カンマでデータを分割
        string[] Positiondata = receivednosedata.Split(',');

        float nose_x = float.Parse(Positiondata[0]);
        float nose_y = float.Parse(Positiondata[1]);
        float r_eye = float.Parse(Positiondata[2]);
        float l_eye = float.Parse(Positiondata[3]);

        //GameManagerの鼻の座標変数に格納する
        GameManager.Instance.nosePosition = new Vector2(nose_x, nose_y);

        //GameManagerの目尻の座標変数に格納する
        GameManager.Instance.eyePosition = new Vector2(r_eye, l_eye);

        //再び受信開始
        getUdp.BeginReceive(OnReceived, getUdp);
    }

    private void OnDestroy()
    {
        udpClient.Close();  // アプリケーション終了時にUDPクライアントを閉じる
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

}