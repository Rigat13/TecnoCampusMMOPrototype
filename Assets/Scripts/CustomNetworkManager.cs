using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
   public override void OnStartServer()
   {
       Debug.Log("Server started");
   }

   public override void OnStopServer()
   {
       Debug.Log("Connected to Server!");
   }

   public override void OnClientConnect(NetworkConnection connection)
   {
       Debug.Log("Client connected to server");
   }

   public override void OnClientDisconnect(NetworkConnection connection)
   {
       Debug.Log("Client disconnected to server");
   }
}
