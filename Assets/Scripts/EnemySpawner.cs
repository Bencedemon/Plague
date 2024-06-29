using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private NetworkObject enemyPrefab;

    public override void OnStartClient(){
        base.OnStartClient();
        if(!IsServerInitialized){
            enabled = false;
            return;
        }
    }

    void Start(){
        StartCoroutine("WaitAndSpawn");
    }

    private IEnumerator WaitAndSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f,5f));
            SpawnEnemy();
        }
    }

    private void SpawnEnemy(){
        NetworkObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        ServerManager.Spawn(enemy);
    }
}
