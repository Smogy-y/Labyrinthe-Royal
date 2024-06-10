using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{
    [SerializeField]
    private Text usernameText;

    [SerializeField]
    private Player player;

    [SerializeField]
    private RectTransform hpBar;

    void Update()
    {
        usernameText.text = player.username;
        hpBar.localScale = new Vector3(player.GetCurrentHP(), 1f, 1f);
    }
}
