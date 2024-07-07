using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spawner_Groups
{
    public Spawner_Unit[] spawner_Units;
    public float startDelay;

    public int enemyCount;
    public int enemyCountDispersion;

    public float frequency;
    public float frequencyDispersion;

    public Spawner_Groups(Spawner_Unit[] _spawner_Units,float _startDelay,int _enemyCount,int _enemyCountDispersion,float _frequency,float _frequencyDispersion){
        spawner_Units= _spawner_Units;
        startDelay= _startDelay;
        enemyCount= _enemyCount;
        enemyCountDispersion=_enemyCountDispersion;
        frequency= _frequency;
        frequencyDispersion= _frequencyDispersion;
    }

    
}
