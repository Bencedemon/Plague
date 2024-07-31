using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Ammo : Item
{
    public override void PickUp(Collider player){
        if (player.transform.TryGetComponent<PlayerWeapon>(out PlayerWeapon playerWeapon))
        {
            if(playerWeapon.CanGetAmmo()){
                audioSource.Play();
                playerWeapon.GetAmmo();
                DespawnObject(parent);
            }
        }
    }
}
