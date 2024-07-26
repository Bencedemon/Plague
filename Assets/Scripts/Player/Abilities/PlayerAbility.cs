using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerAbility : NetworkBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerMovement playerMovement;
    [Header("Abilities")]
    [SerializeField] public Ability ability;
    [SerializeField] private Ability[] abilities;

    public void SelectAbility(int _id){
        foreach (var _ability in abilities)
        {
            if(_ability.cardProperty.cardId == _id){
                ability=_ability;
                ability.gameObject.SetActive(true);
            }
        }
    }
    public void UseAbility(InputAction.CallbackContext context){
        if(ability==null) return;
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed){
            if(!playerMovement.canMove) return;
            ability.UseAbility();
        }
    }

}
