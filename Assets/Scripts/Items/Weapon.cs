using UnityEngine;

public enum FireMode { Auto, SemiAuto }

[CreateAssetMenu(fileName = "New Weapon", menuName = "FPS/Weapon")]

public class Weapon : Item
{
    public string weaponName;
    public float damage;
    public float range;

    public FireMode fireMode;
    public float fireRate;

    public int magazineSize;

    public float reloadTime;

    public GameObject prefab;

    public AudioClip shootSound;
    public AudioClip reloadSound;
}
