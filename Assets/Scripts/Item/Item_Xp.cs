using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Xp : Item
{
    public override void PickUp(Collider player){

        LevelManager levelManager=FindObjectOfType<LevelManager>();
        if(levelManager!=null){
            levelManager.AddExperiance(Random.Range(5f,20f));
        }
        DespawnObject(parent);
    }
}
