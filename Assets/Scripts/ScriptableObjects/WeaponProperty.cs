using UnityEngine;
using FishNet.Object;

[CreateAssetMenu(fileName = "WeaponProperty", menuName = "ScriptableObjects/WeaponProperty", order = 1)]
public class WeaponProperty : ScriptableObject
{
    public int weaponId;
    public string weaponName;
    [Space]
    public Sprite icon;
    public Sprite iconSmall;
    [Space]
    public float damage;
    public float maxRange = 20f;
    public int punchDamage = 10;
    public float punchRange = 2f;
    [Space]
    public int maxAmmo = 9;
    public int totalAmmo = 0;
    [Space]
    public float spread = 0f;
    [Space]
    public WeaponType weaponType;
    public enum WeaponType
    {
        auomatic,
        semiautomatic
    }
    [Space]
    public bool rifleType = false;
    [Space]
    public NetworkObject hitParticle_wall;
    public NetworkObject hitParticle_enemy;
    [Space]
    public AudioClip[] shoot;
    public AudioClip reloadLast;
    public AudioClip reload;
    public AudioClip cock;
    public AudioClip[] punch;
    [Space]
    public int level;
}
