using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class LayerManager : NetworkBehaviour
{
    [Header("Layers")]

    [Tooltip("playerSelfLayer = 6, weaponSelfLayer = 8")]
    [SerializeField] private int selfLayer = 6;

    [Tooltip("playerOtherLayer = 7, weaponOtherLayer = 9")]
    [SerializeField] private int otherLayer = 7;

    [Space]
    [Header("Objects")]
    [SerializeField] private GameObject[] objects;

    [Space]
    [Header("Audio")]
    [SerializeField] private AudioSource[] audioSources;

    public override void OnStartClient(){
        base.OnStartClient();

        if(base.IsOwner){
            foreach (var item in objects)
            {
                item.layer = selfLayer;
            }
            if(selfLayer==6){
                foreach (var item in audioSources)
                {
                    item.mute=true;
                }
            }else{
                foreach (var item in audioSources)
                {
                    item.mute=false;
                }
            }
        }else{
            foreach (var item in objects)
            {
                item.layer = otherLayer;
            }
            if(selfLayer==6){
                foreach (var item in audioSources)
                {
                    item.mute=false;
                }
            }else{
                foreach (var item in audioSources)
                {
                    item.mute=true;
                }
            }
        }
    }
}
