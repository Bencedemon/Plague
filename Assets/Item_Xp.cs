using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Xp : Item
{
    [SerializeField] private GameObject parent;
    public override void PickUp(Collider player){

        LevelManager levelManager=FindObjectOfType<LevelManager>();
        if(levelManager!=null){
            levelManager.AddExperiance(50f);
        }
        DespawnObject(parent);
    }
}
