using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    GameObject playerInfo;

    [SerializeField]
    Transform playerList;

    private void OnEnable()
    {
        Player[] players = GameManager.GetAllPlayer();

        foreach (Player player in players)
        {
            GameObject itemGO = Instantiate(playerInfo, playerList);
            PlayerInfoItem item = itemGO.GetComponent<PlayerInfoItem>();

            if(item != null)
            {
                item.SetupScoreBoard(player);
            }
        }
    } 

    private void OnDisable()
    {
        foreach (Transform child in playerList)
        {
            Destroy(child.gameObject);
        }
    }
}
