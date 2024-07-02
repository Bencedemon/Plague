using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hitbox : MonoBehaviour
{   

    public enum HitboxType{head, body, limb}
    public HitboxType hitboxType;


    public Enemy enemy;


    public void HitboxTakeDamage(int damage){
        if(hitboxType==HitboxType.head){
            enemy.TakeDamage(damage*2);
        }else{
            enemy.TakeDamage(damage);
        }
    }


}
