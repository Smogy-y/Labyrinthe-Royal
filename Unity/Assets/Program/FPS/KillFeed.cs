using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFeed : MonoBehaviour
{
    [SerializeField]
    GameObject killFeedItem;

    void Start()
    {
        GameManager.instance.onPlayerKilledCallBack += OnKill;
    }

    public void OnKill(string mort, string tueur)
    {
        GameObject go = Instantiate(killFeedItem, transform);
        go.GetComponent<KillFeedItem>().Setup(mort, tueur);
        Destroy(go, 3f);
    }
}
