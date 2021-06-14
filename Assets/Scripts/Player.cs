using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHolaCountChanged))]
    int holaCount = 0;
    void HandleMovement()
    {
        if (isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveForward = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal * Time.deltaTime * 10, 0, moveForward * Time.deltaTime  * 10);
            transform.position = transform.position + movement;
        }
    }

    void HandleCamera()
    {
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

        transform.Rotate(0, moveX, 0);
        transform.Translate(0, 0, moveZ);
    }

    void Update()
    {
        HandleMovement();
        HandleCamera();

        if (isLocalPlayer && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Sending Hola to Server");
            Hola();
        }
    }

    public override void OnStartServer()
    {
        Debug.Log("Player has been spawned on the server!");
    }

    [Command]
    void Hola()
    {
        Debug.Log("Received Hola from Client!");
        holaCount += 1;
        ReplyHola();
    }

    [TargetRpc]
    void ReplyHola()
    {
        Debug.Log("Received Hola fromServer!");
    }

    [ClientRpc]
    void TooFar()
    {
        Debug.Log("Too far!");
    }

    void OnHolaCountChanged(int oldCount, int newCount)
    {
        Debug.Log($"We had {oldCount} holas but now we have {newCount} holas!");
    }


    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);
    }

}
