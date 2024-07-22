using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class Enemy_Hitbox : NetworkBehaviour
{   

    public enum HitboxType{head, body, limb}
    public HitboxType hitboxType;


    public Rigidbody rigidbody;
    public Enemy enemy;


    public void HitboxTakeDamage(float damage,Vector3 direction,PlayerStats _playerStats){
        switch (hitboxType)
        {
            case HitboxType.head:
                enemy.TakeDamage(damage*3,direction,this,_playerStats);
            break;
            case HitboxType.body:
                enemy.TakeDamage(damage,direction,this,_playerStats);
            break;
            case HitboxType.limb:
                enemy.TakeDamage(damage,direction,this,_playerStats);
            break;
            default:
                Debug.LogError(""+hitboxType+" not excists");
            break;
        }
    }


}
