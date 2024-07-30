using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class Enemy_Hitbox : NetworkBehaviour
{   

    public enum HitboxType{head, body, limb}
    public HitboxType hitboxType;


    public Rigidbody rb;
    public Enemy enemy;


    public void HitboxTakeDamage(float damage,Vector3 direction,PlayerStats _playerStats){
        switch (hitboxType)
        {
            case HitboxType.head:
                enemy.TakeDamage(damage*3,direction,_playerStats,this);
            break;
            case HitboxType.body:
                enemy.TakeDamage(damage,direction,_playerStats,this);
            break;
            case HitboxType.limb:
                enemy.TakeDamage(damage,direction,_playerStats,this);
            break;
            default:
                Debug.LogError(""+hitboxType+" not excists");
            break;
        }
    }


}
