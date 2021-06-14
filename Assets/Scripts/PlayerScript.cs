using Mirror;
using UnityEngine;

namespace QuickStart
{
    
    public class PlayerScript : NetworkBehaviour
    {
        public float xOffset, yOffset, zOffset;
        public float xSpeed = 180f;
        public float zSpeed = 5f;
        public TextMesh playerNameText;
        public GameObject floatingInfo;

        private Material playerMaterialClone;

        [SyncVar(hook = nameof(OnNameChanged))]
        public string playerName;

        [SyncVar(hook = nameof(OnColorChanged))]
        public Color playerColor = Color.white;

private SceneScript sceneScript;

void Awake()
{
    //allow all players to run this
    sceneScript = GameObject.FindObjectOfType<SceneScript>();
}
[Command]
public void CmdSendPlayerMessage()
{
    if (sceneScript) 
    { 
        sceneScript.statusText = $"{playerName} says hello {Random.Range(10, 99)}";
    }
}
[Command]
public void CmdSetupPlayer(string _name, Color _col)
{
    //player info sent to server, then server updates sync vars which handles it on all clients
    playerName = _name;
    playerColor = _col;
    sceneScript.statusText = $"{playerName} joined.";
}

        void OnNameChanged(string _Old, string _New)
        {
            playerNameText.text = playerName;
        }

        void OnColorChanged(Color _Old, Color _New)
        {
            playerNameText.color = _New;
            playerMaterialClone = new Material(GetComponent<Renderer>().material);
            playerMaterialClone.color = _New;
            GetComponent<Renderer>().material = playerMaterialClone;
        }

        public override void OnStartLocalPlayer()
        {
            sceneScript.playerScript = this;

            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(xOffset, yOffset, -zOffset);
            
            floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
            floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            string name = "Player" + Random.Range(100, 999);
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            CmdSetupPlayer(name, color);
        }
        void Update()
        {
            if (!isLocalPlayer)
            {
                // make non-local players run this
                floatingInfo.transform.LookAt(Camera.main.transform);
                return;
            }

            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * xSpeed;
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * zSpeed;

            transform.Rotate(0, moveX, 0);
            transform.Translate(0, 0, moveZ);
        }
    }
}