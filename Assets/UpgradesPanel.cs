using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesPanel : MonoBehaviour
{
    
    [SerializeField] private PlayerUpgrades playerUpgrades;

    public void Select(){
        playerUpgrades.PlayerLevelingUpEnd();
    }
}
