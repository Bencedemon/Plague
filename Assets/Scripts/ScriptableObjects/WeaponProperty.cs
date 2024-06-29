using UnityEngine;

[CreateAssetMenu(fileName = "WeaponProperty", menuName = "ScriptableObjects/WeaponProperty", order = 1)]
public class WeaponProperty : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float maxRange = 20f;
    public int maxAmmo = 9;
    public WeaponType weaponType;
    public enum WeaponType
    {
        auomatic,
        semiautomatic
    }
    public bool rifleType = false;
}
