using UnityEngine.UI;
using UnityEngine;

public class KillFeedItem : MonoBehaviour
{
    [SerializeField]
    Text text;

    public void Setup(string mort, string tueur)
    {
        text.text = tueur + " killed " + mort;
    }
}
