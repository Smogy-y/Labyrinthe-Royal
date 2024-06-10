using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "My Game/ WeaponDate")]
public class WeaponData : ScriptableObject
{
    public string name;
    public float damage;
    public float range;

    public float fireRate;

    public int magazineSize;
    public float timeToReload;

    public AudioClip shootSound;

    public GameObject graphics;
}
