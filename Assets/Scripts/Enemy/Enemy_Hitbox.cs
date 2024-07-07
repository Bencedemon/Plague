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


    public void HitboxTakeDamage(int damage,Vector3 direction,PlayerStats _playerStats){
        if(hitboxType==HitboxType.head){
            enemy.TakeDamage(damage*2,direction,this,_playerStats);
        }else{
            enemy.TakeDamage(damage,direction,this,_playerStats);
        }
    }


}
