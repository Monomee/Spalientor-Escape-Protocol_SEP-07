using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/Gun", order = 1)]
public class GunConfig : ScriptableObject
{
    public GunType typeOfGun;
    public float damage;
    public float fireRate;
    public float range;
    public int magazineSize;
    public float reloadTime;
    public float recoil;
    public bool isAutomatic;
    public float spreadOffset;
    public float bulletSpeed;

    public LayerMask hitLayer;
    public AudioSource shotSFX;
    public AudioSource reloadSFX;
}
public enum GunType
{
    Rifle,
    Shotgun,
    Minigun
}