using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private Spawner_Wave[] spawner_Waves;
    [SerializeField] private int currentWaveId=0;
    public int currentEnemyCount=0;
    public readonly SyncVar<int> enemyCount = new(0);
    [SerializeField] public int _enemyCount;

    [SerializeField] private Transform enemySpawners;

    [Space]
    [SerializeField] private Animator musicAnimator;
    [SerializeField] private AudioSource fight,calm;

    [Space]
    [SerializeField] private GameObject exit;

    private bool waveStart=true;
    private bool gameEnded=false;

    [Space]
    public EnemySpawnLocation[] enemySpawnLocations;


    public override void OnStartClient(){
        base.OnStartClient();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!IsServerInitialized) return;
        _enemyCount=enemyCount.Value;
        if(currentWaveId<spawner_Waves.Length+1){
            if(currentEnemyCount==0 && enemySpawners.childCount==0){
                if(!waveStart){
                    waveStart=true;
                    if(currentWaveId==spawner_Waves.Length){
                        OpenExitServer();
                    }else{
                        StartCoroutine(StartNewWave());
                    }
                }
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void OpenExitServer(){
        OpenExitObserver();
    }
    [ObserversRpc]
    private void OpenExitObserver(){
        exit.SetActive(true);
    }

    public IEnumerator StartNewWave(){
        /*if(spawner_Waves[currentWaveId].delay>0){
            musicAnimator.SetBool("fight",false);
        }*/
        yield return new WaitForSeconds(spawner_Waves[currentWaveId].delay);
        for(int i=0;i<spawner_Waves[currentWaveId].spawners.Length;i++){
            GameObject spawner = Instantiate(spawner_Waves[currentWaveId].spawners[i],transform.position, Quaternion.identity);
            spawner.GetComponent<Spawner>().enemySpawner=this;
            spawner.transform.SetParent(enemySpawners);
            ServerManager.Spawn(spawner);
            //musicAnimator.SetBool("fight",true);
        }
        currentWaveId++;
        waveStart=false;
    }

    [ServerRpc(RequireOwnership = false)] public void SetEnemyCount(int _enemyCount) => enemyCount.Value += _enemyCount;
    
}
