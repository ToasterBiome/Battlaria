using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    string textInput = "";

    public GameObject bubble;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.height - 30, 10, 300, 300));
        textInput = GUILayout.TextField(textInput, 16);
        if (GUILayout.Button("Send"))
        {
            SendChatText();
        }
        if (Event.current.keyCode == KeyCode.Return)
        {
            SendChatText();
        }

        GUILayout.EndArea();
    }

    public void SendChatText()
    {
        if (textInput != "")
        {
            //send something
            if (NetworkManager.Singleton.IsServer)
            {
                //we do the spawning

                SendChat(textInput, NetworkManager.Singleton.LocalClientId);


            }
            else
            {
                //or else ask the server to spawn.. or something?
                SendChatServerRpc(textInput, NetworkManager.Singleton.LocalClientId);
            }
        }

        textInput = "";
    }

    [ServerRpc(RequireOwnership=false)]
    public void SendChatServerRpc(string text, ulong client)
    {
        if(NetworkManager.Singleton.IsServer)
        {
            SendChat(text, client);
        }
    }

    public void SendChat(string text, ulong client)
    {
        Debug.Log(NetworkManager.Singleton.IsServer);
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(client,
                        out var networkedClient))
        {
            var player = networkedClient.PlayerObject;
            GameObject go = Instantiate(bubble, player.transform.position + new Vector3(0, 1), Quaternion.identity);
            go.transform.parent = player.gameObject.transform;
            go.GetComponent<RunechatBubble>().chatText.Value = text;
            go.GetComponent<NetworkObject>().Spawn();
        }
    }
}
