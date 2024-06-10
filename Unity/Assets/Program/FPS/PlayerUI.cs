using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPause;

    [SerializeField]
    private GameObject scoreBoard;

    [SerializeField]
    private RectTransform hPBarre;

    private Player player;

    private void Start()
    {
        MenuPause.isOn = false;
    }

    private void Update()
    {
        SetHPAmount(player.GetCurrentHP());

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenuPause();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreBoard.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreBoard.SetActive(false);
        }
    }

    public void ToggleMenuPause()
    {
        menuPause.SetActive(!menuPause.activeSelf);
        MenuPause.isOn = menuPause.activeSelf;
    }

    void SetHPAmount(float _amount)
    {
        hPBarre.localScale = new Vector3(_amount, 1f, 1f);
    }

    public void SetPlayer(Player _player)
    {
        player = _player;
    }
}
