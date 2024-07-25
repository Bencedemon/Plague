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
    [SerializeField] private Ability ability1;
    [SerializeField] private Ability ability2;
    [SerializeField] private Ability[] abilities;

    public void SelectAbility(int _id, float _strength, float _range, float _duration, float _cooldown){
        bool exists=false;
        if(ability1 != null){
            if(ability1.cardProperty.cardId == _id){
                exists=true;
                ability1.strength += _strength;
                ability1.range += _range;
                ability1.duration += _duration;
                ability1.cooldown += _cooldown;
            }
        }
        if(ability2 != null){
            if(ability2.cardProperty.cardId == _id){
                exists=true;
                ability2.strength += _strength;
                ability2.range += _range;
                ability2.duration += _duration;
                ability2.cooldown += _cooldown;
            }
        }
        if(exists) return;
        foreach (var _ability in abilities)
        {
            if(_ability.cardProperty.cardId == _id){
                if(ability1==null){
                    ability1=_ability;
                    ability1.gameObject.SetActive(true);
                }else{
                    ability2=_ability;
                    ability2.gameObject.SetActive(true);
                }
            }
        }
    }
    public void UseAbility1(InputAction.CallbackContext context){
        if(ability1==null) return;
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed){
            if(!playerMovement.canMove) return;
            ability1.UseAbility();
        }
    }
    public void UseAbility2(InputAction.CallbackContext context){
        if(ability2==null) return;
        if(playerStats._currentHealth.Value<=0) return;
        if(context.performed){
            if(!playerMovement.canMove) return;
            ability2.UseAbility();
        }
    }

}
