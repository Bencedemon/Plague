using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

[System.Serializable]
public class Spawner_Wave
{
    public GameObject[] spawners;

    public float delay;

    public Spawner_Wave(GameObject[] _spawners,float _delay){
        spawners = _spawners;
        delay = _delay;
    }

}
