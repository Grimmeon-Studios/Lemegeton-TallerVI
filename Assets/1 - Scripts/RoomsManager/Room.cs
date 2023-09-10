using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    public UnityEvent NoRemainingEnemies;

    private EdgeCollider2D edgeCollider;
    private GameObject dungeonManager;
    private DungeonManager _dungeonManager;
    private HashSet<GameObject> enemiesHashSet = new HashSet<GameObject>();

    // The first Collider is the one who detects the player and trapts it in the Room
    [SerializeField] private BoxCollider2D mainBoxCollider;
    [SerializeField] private GameObject combatOverlay;

    [Header("Enemies Config")]
    // pls especify how many enemies prefabs there are
    [SerializeField] private int numberOfEnemiesPrefabs;
    [SerializeField] private GameObject enemyPrefab1;
    [SerializeField] private GameObject enemyPrefab2;

    void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        mainBoxCollider = GetComponent<BoxCollider2D>();
        mainBoxCollider.enabled = true;
        edgeCollider.enabled = false;
    }

    private void Start()
    {
        dungeonManager = GameObject.Find("Dungeon Manager");
        _dungeonManager = dungeonManager.GetComponent<DungeonManager>();
        combatOverlay.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var @object = other.gameObject;
        if (@object.CompareTag("Player"))
        {
            Debug.Log("Player Entered");
            edgeCollider.enabled = true;
            StartCoroutine(WaitAndTrapPlayer(2));            
        }
        else if(@object.CompareTag("Enemy") || @object.CompareTag("EnemySoul"))
        {
            if (!enemiesHashSet.Contains(@object))
            {
                enemiesHashSet.Add(@object);
            }
        }
        else if(!@object.CompareTag("Dungeon Manager"))
        {
            Debug.Log("Entity: "+ @object.name + " Not Recognized");
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        
        var @object = other.gameObject;
        if (@object.CompareTag("Player"))
        {
            Debug.Log("Player Exited the Room");
        }
        else if (@object.CompareTag("Enemy") || @object.CompareTag("EnemySoul"))
        {
            if (enemiesHashSet.Contains(@object))
            {
                enemiesHashSet.Remove(@object);
                Destroy(@object);
                if(enemiesHashSet.Count == 0)
                {
                    NoRemainingEnemies.Invoke();
                }
            }
        }
        else
        {
            Debug.Log("Entity: " + @object.name + " Not Recognized");
        }

    }

    private Vector2 RandomEnemySpawnPos()
    {

        float newBoundary_x = UnityEngine.Random.Range(mainBoxCollider.bounds.min.x, mainBoxCollider.bounds.max.x);
        float newBoundary_y = UnityEngine.Random.Range(mainBoxCollider.bounds.min.y, mainBoxCollider.bounds.max.y);

        Vector2 enemyPos = new Vector2(newBoundary_x, newBoundary_y);
        return enemyPos;
    }

    private void SpawnEnemies(int addEnemyCount, Vector3 enemyStatsMult)
    {

        for (int i = 0; i < 4 + addEnemyCount; i++)
        {
            int enemyToSpawn = UnityEngine.Random.Range(0, (numberOfEnemiesPrefabs));
            if(enemyToSpawn == 0)
            {
                Instantiate(enemyPrefab1, RandomEnemySpawnPos(), Quaternion.identity);
            }
            else if(enemyToSpawn == 1){
                Instantiate(enemyPrefab2, RandomEnemySpawnPos(), Quaternion.identity);
            }
        }

    }

    IEnumerator WaitAndTrapPlayer(float waitTime)
    {
        combatOverlay.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        //if (_dungeonManager._difficultylvl % 3 == 0)
        //{
        //    // aumenta número de spawneo
        //}
        //else
        //{
        //    // aumenta las estadísticas de los enemigos spawneados
        //}

        _dungeonManager.EnemyManagement();

        IncubusScript incubusScript = enemyPrefab1.GetComponent<IncubusScript>();
        LostSoulScript lostSoulScript = enemyPrefab2.GetComponent<LostSoulScript>();

        incubusScript.Health = incubusScript.Health + _dungeonManager.EnemyStatsMultiplier.x;
        incubusScript.ContactDamage = incubusScript.ContactDamage + _dungeonManager.EnemyStatsMultiplier.y;
        incubusScript.MoveSpeed = incubusScript.MoveSpeed + _dungeonManager.EnemyStatsMultiplier.z;

        lostSoulScript.health = lostSoulScript.health + _dungeonManager.EnemyStatsMultiplier.x;
        lostSoulScript.shotDamage = lostSoulScript.shotDamage + _dungeonManager.EnemyStatsMultiplier.y;
        lostSoulScript.moveSpeed = lostSoulScript.moveSpeed + _dungeonManager.EnemyStatsMultiplier.z;

        SpawnEnemies(_dungeonManager.AdditionalEnemyCount, _dungeonManager.EnemyStatsMultiplier);



        //for (int i = 0; i < _dungeonManager._difficultylvl; i++)
        //{
        //    int enemyToSpawn = UnityEngine.Random.Range(0, (numberOfEnemiesPrefabs));
        //    Debug.Log("Rand int: " + enemyToSpawn);

        //    switch (enemyToSpawn)
        //    {
        //        case 0:
        //            SpawnEnemies(enemyPrefab1); 
        //            break;

        //        case 1:
        //            SpawnEnemies(enemyPrefab2);
        //            break;

        //        //Type More cases if there are more enemies added
        //    }
        //}
    }
}
