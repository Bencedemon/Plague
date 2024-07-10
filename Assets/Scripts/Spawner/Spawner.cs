using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class Spawner : NetworkBehaviour
{

    [SerializeField] private EnemySpawnLocation[] enemySpawnLocations;
    [SerializeField] private LayerMask ground,obstacleMask;

    [Space]
    public Spawner_Groups spawner_Groups;


    private float maxWeight;
    private List<float> weights = new List<float>();
    private int enemyCount=0;

    private float randomSpawn;


    private PlayerManager playerManager;
    public EnemySpawner enemySpawner;
    public override void OnStartClient(){
        base.OnStartClient();
    }
    void Awake(){
        playerManager=FindObjectOfType<PlayerManager>();
        enemySpawner=FindObjectOfType<EnemySpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!IsServerInitialized) return;
        maxWeight=0;
        for(int i=0;i<spawner_Groups.spawner_Units.Length;i++){
            maxWeight+=spawner_Groups.spawner_Units[i].weight;
            weights.Add(maxWeight);
        }
        enemySpawner.SetEnemyCount(spawner_Groups.enemyCount);
        StartCoroutine(spawnerStart());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!base.IsServerInitialized) return;
        if(enemyCount>=spawner_Groups.enemyCount){
            Destroy(gameObject);   
        }
    }

    private IEnumerator spawnerStart(){
        yield return new WaitForSeconds(spawner_Groups.startDelay);
        StartCoroutine(WaitAndSpawn());
    }

    public IEnumerator WaitAndSpawn()
    {
        while(enemyCount<spawner_Groups.enemyCount){
            yield return new WaitForSeconds(Random.Range(spawner_Groups.frequency-spawner_Groups.frequencyDispersion,spawner_Groups.frequency+spawner_Groups.frequencyDispersion));
            SpawnEnemy();
        }
    }

    private void SpawnEnemy(){
        if(!IsServerInitialized) return;
        int spawnerId = Random.Range(0,enemySpawnLocations.Length);

        float x = Random.Range(enemySpawnLocations[spawnerId].size.x * -0.5f,enemySpawnLocations[spawnerId].size.x * 0.5f) + enemySpawnLocations[spawnerId].position.x;
        float z = Random.Range(enemySpawnLocations[spawnerId].size.z * -0.5f,enemySpawnLocations[spawnerId].size.z * 0.5f) + enemySpawnLocations[spawnerId].position.z;
        float y = enemySpawnLocations[spawnerId].size.y/2 + enemySpawnLocations[spawnerId].position.y ;

        if(Physics.Raycast(new Vector3(x,y,z), Vector3.down, out RaycastHit hit, enemySpawnLocations[spawnerId].size.y, ground)){

            bool canSpawn=true;
            for (int i = 0; i < playerManager.PlayerGameObject.Count; i++)
            {
                Transform target = playerManager.PlayerGameObject[i].transform;
                Vector3 dirToTarget = ((target.position + new Vector3(0,1.5f,0)) - (hit.point + new Vector3(0,1.5f,0))).normalized;
      
                float disToTarget = Vector3.Distance(hit.point + new Vector3(0,1.5f,0), target.position + new Vector3(0,1.5f,0));

                if(!Physics.Raycast(hit.point + new Vector3(0,1.5f,0),dirToTarget,disToTarget,obstacleMask)){
                    canSpawn=false;
                }
            }
            if(canSpawn){
                randomSpawn=Random.Range(0f,maxWeight);
                for (int j = 0; j < weights.Count; j++){
                    if(randomSpawn<=weights[j]){
                        NetworkObject enemy = Instantiate(spawner_Groups.spawner_Units[j].enemyPrefab, hit.point, Quaternion.identity);
                        enemy.GetComponent<Enemy>().enemySpawner=enemySpawner;
                        ServerManager.Spawn(enemy);
                        enemySpawner.currentEnemyCount++;
                        enemyCount++;
                        break;
                    }
                }
            }
        }
    }

}
