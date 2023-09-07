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
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject combatOverlay;

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
            StartCoroutine(WaitAndTrapPlayer(2));            
        }
        else if(@object.CompareTag("Enemy"))
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
        else if (@object.CompareTag("Enemy"))
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

    private void SpawnEnemies(int dificulty)
    {
        int i;
        for (i = 0; i < dificulty; i++)
        {
            Instantiate(enemyPrefab, RandomEnemySpawnPos(), Quaternion.identity);
        }
    }

    IEnumerator WaitAndTrapPlayer(float waitTime)
    {
        combatOverlay.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        edgeCollider.enabled = true;
        SpawnEnemies(_dungeonManager._difficultylvl);
    }
}
