using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (EnemySpawner))]
public class EnemySpawnPoint : Editor
{
    void OnSceneGUI(){
        EnemySpawner es = (EnemySpawner)target;
        Handles.color = Color.red;
        foreach (var item in es.enemySpawnLocations)
        {
            Handles.DrawWireCube(item.position, item.size);
        }
    }
}
