using MLAPI;
using UnityEngine;
using MLAPI.Transports.UNET;

namespace HelloWorld
{
    public class HelloWorldManager : MonoBehaviour
    {
        public static string connectIP = "127.0.0.1";
        public static int connectPort = 7777;
        public static string playerName = "Random Player";
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host"))
            {
                HostGame();
            }
            connectIP = GUILayout.TextField(connectIP, 16);
            connectPort = int.Parse(GUILayout.TextField(connectPort.ToString(), 5));
            if (GUILayout.Button("Client"))
            {
                JoinGame();
            }
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void HostGame()
        {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ServerListenPort = 9262;
            NetworkManager.Singleton.StartHost();
            Debug.Log("Attempting to start server on port: " + NetworkManager.Singleton.GetComponent<UNetTransport>().ServerListenPort);
        }

        static void JoinGame()
        {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = connectIP;
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectPort = 9262;
            Debug.Log("Attempting to join server at " + NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress + ":" + NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectPort);
            NetworkManager.Singleton.StartClient();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Listening: " +
                NetworkManager.Singleton.IsListening);
            GUILayout.Label("Port: " +
                NetworkManager.Singleton.GetComponent<UNetTransport>().ServerListenPort);
            GUILayout.Label("Mode: " + mode);


        }

        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
            {
                if (NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId,
                    out var networkedClient))
                {
                    var player = networkedClient.PlayerObject.GetComponent<HelloWorldPlayer>();
                    if (player)
                    {
                        player.Move();
                    }
                }
            }
            playerName = GUILayout.TextField(playerName, 16);
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Change Name" : "Request Change Name"))
            {
                if (NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId,
                    out var networkedClient))
                {
                    var player = networkedClient.PlayerObject.GetComponent<HelloWorldPlayer>();
                    if (player)
                    {
                        player.SubmitNewNameServerRpc(playerName);
                    }
                }
            }
        }
    }
}