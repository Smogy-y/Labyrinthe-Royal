using UnityEngine;
using Mirror;
using System.Collections;


[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip dieSound;

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead;  }
        protected set { _isDead = value;  }
    }

    [SerializeField]
    private float maxHP = 100f;

    [SyncVar]
    private float currentHP;

    public float GetCurrentHP()
    {
        return (float)currentHP / maxHP;
    }

    [SyncVar]
    public string username = "Player";

    public int kills;
    public int deaths;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnableOnStart;

    [SerializeField]
    private GameObject[] disableObjectOnDeath;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSetting.respawnTimer);

        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        Setup();
    }

    private void SetDefaults()
    {
        isDead = false;
        currentHP = maxHP;

        //activation des component
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnableOnStart[i];
        }

        //activation des GameObject -> graphic
        for (int i = 0; i < disableObjectOnDeath.Length; i++)
        {
            disableObjectOnDeath[i].SetActive(true);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCam(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        //Systeme d'effet d'apparition
        GameObject gfxIns = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(gfxIns, 3f);

    }

    [ClientRpc]
    public void RpcTakeDamage(float degat, string tireurID)
    {
        if (isDead)
        {
            return;
        }


        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(hitSound);

        currentHP -= degat;
        Debug.Log(transform.name + "vie de " + currentHP);

        if (currentHP <= 0)
        {
            audioSource.PlayOneShot(dieSound);
            Die(tireurID);
        }
    }

    private void Die(string sourceID)
    {
        isDead = true;

        Player sourcePlayer = GameManager.GetPlayer(sourceID);
        if(sourcePlayer != null)
        {
            sourcePlayer.kills++;

            GameManager.instance.onPlayerKilledCallBack.Invoke(username, sourcePlayer.username);
        }

        deaths++;

        //Desactivation des component
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        //Desactivation des GameObject -> graphic
        for (int i = 0; i < disableObjectOnDeath.Length; i++)
        {
            disableObjectOnDeath[i].SetActive(false);
        }

        //Desactive le Collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        //Systeme d'explosion
        GameObject gfxIns = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gfxIns, 3f);

        StartCoroutine(Respawn());
    }

    [Command]//(ignoreAuthority = true) si jamais bug -> de manière aléatoire un joueur ne respawn pas
    private void CmdBroadCastNewplayerSetup()
    {
        RpcSetupPlayerOnAllClient();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClient()
    {
        if (firstSetup)
        {
            wasEnableOnStart = new bool[disableOnDeath.Length];
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                wasEnableOnStart[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
        }

        SetDefaults();
    }

    public void Setup()
    {
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCam(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        CmdBroadCastNewplayerSetup();
    }
}
