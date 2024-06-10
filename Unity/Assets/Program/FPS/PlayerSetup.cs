using UnityEngine;
using Mirror;


[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentToDisable;

    [SerializeField]
    private string remoteLayer = "RemotePlayer";

    [SerializeField]
    private string dontDrawLayerName = "dontDraw";

    [SerializeField]
    private GameObject playerGraphics;

    [SerializeField]
    private GameObject playerNameGraphics;

    [SerializeField]
    private GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;

    private void AssigneRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayer);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netID, player);
    }

    private void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentToDisable.Length; i++)
            {
                componentToDisable[i].enabled = false;
            }
            AssigneRemoteLayer();
        }
        else
        {
            Utile.SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
            Utile.SetLayerRecursively(playerNameGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            playerUIInstance = Instantiate(playerUIPrefab);

            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.Log("pas de Composant PlayUI sur playerUiInstance");
            }
            else
            {
                ui.SetPlayer(GetComponent<Player>());
            }


            GetComponent<Player>().Setup();


            CmdSetUsername(transform.name, UserAccountManager.logUserName);
        }      
    }

    [Command]
    void CmdSetUsername(string playerId, string username)
    {
        Player player = GameManager.GetPlayer(playerId);
        if(player != null)
        {
            player.username = username;
        }
    }

    private void OnDisable()
    {
        Destroy(playerUIInstance);

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCam(true);
        }

        GameManager.UnregisterPlayer(transform.name);
    }
}
