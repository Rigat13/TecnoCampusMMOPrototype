using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[DisallowMultipleComponent]
[AddComponentMenu("Network/NetworkManagerHUD")]
[RequireComponent(typeof(NetworkManager))]
[HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]

public class CustomNetworkManagerHUD : MonoBehaviour
{
    private const string LOCAL_ADDRESS = "localhost";
    private string SERVER_ADDRESS = "35.159.20.239";
        NetworkManager manager;
        public bool showGUI = true;

        public int offsetX;
        public int offsetY;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
            manager.networkAddress = "35.159.20.239";
        }

        void OnGUI()
        {
#pragma warning disable 618
            if (!showGUI) return;
#pragma warning restore 618

            GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            // client ready
            if (NetworkClient.isConnected && !NetworkClient.ready)
            {
                if (GUILayout.Button("Client Ready"))
                {
                    NetworkClient.Ready();
                    if (NetworkClient.localPlayer == null)
                    {
                        NetworkClient.AddPlayer();
                    }
                }
            }

            StopButtons();

            GUILayout.EndArea();
        }

        void StartButtons()
        {
            if (!NetworkClient.active)
            {
                // Client Online
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("JOIN ONLINE"))
                {
                    manager.StartClient();
                    manager.networkAddress = SERVER_ADDRESS;
                }
                GUILayout.EndHorizontal();

                // Server + Client Local
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUILayout.Button("CREATE LOCAL"))
                    {
                        manager.StartHost();
                        manager.networkAddress = LOCAL_ADDRESS;
                    }
                }

                // Client Local
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("JOIN LOCAL"))
                {
                    manager.StartClient();
                    manager.networkAddress = LOCAL_ADDRESS;
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                // Connecting
                GUILayout.Label("Connecting to " + manager.networkAddress + "..");
                if (GUILayout.Button("Cancel Connection Attempt"))
                {
                    manager.StopClient();
                }
            }
        }

        void StatusLabels()
        {
            // host mode
            // display separately because this always confused people:
            //   Server: ...
            //   Client: ...
            if (NetworkServer.active && NetworkClient.active)
            {
                GUILayout.Label($"<b>Host</b>: running via {Transport.activeTransport}");
            }
            // server only
            else if (NetworkServer.active)
            {
                GUILayout.Label($"<b>Server</b>: running via {Transport.activeTransport}");
            }
            // client only
            else if (NetworkClient.isConnected)
            {
                GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
            }
        }

        void StopButtons()
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Host"))
                {
                    manager.StopHost();
                }
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Client"))
                {
                    manager.StopClient();
                }
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Stop Server"))
                {
                    manager.StopServer();
                }
            }
        }
}
