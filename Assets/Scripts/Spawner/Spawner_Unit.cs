using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

[System.Serializable]
public class Spawner_Unit
{
    public NetworkObject enemyPrefab;

    public float weight;
    
    public Spawner_Unit(NetworkObject _enemyPrefab,float _weight){
        enemyPrefab = _enemyPrefab;
        weight = _weight;
    }
}
