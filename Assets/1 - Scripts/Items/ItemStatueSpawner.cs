using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatueSpawner : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefabTier1;
    [SerializeField] private GameObject itemPrefabTier2;
    [SerializeField] private int radius = 2;
    [SerializeField] private int degrees;
    [SerializeField] private int itemsQuantityToSpawn = 3;

    private bool isStatueUsed;
    List<GameObject> itemList = new List<GameObject>();

    void Start()
    {
        isStatueUsed = false;
        itemList.Add(itemPrefabTier1);
    }

    public void DropItems()
    {
        if (!isStatueUsed)
        {
            Debug.Log("Droping Items");
            for (int i = 0; i < itemsQuantityToSpawn; i++)
            {
                degrees = degrees + 45;
                int itemTier = UnityEngine.Random.Range(0, itemList.Count);
                switch (itemTier)
                {
                    case 0:
                        InstantiateItems(degrees, itemPrefabTier1);
                        break;

                    case 1:
                        //Temporaly assigned Item Prefab Tier1 due to laking of tier 2 items
                        InstantiateItems(degrees, itemPrefabTier1);
                        break;
                }
            }
            isStatueUsed=true;
        }
    }

    private void InstantiateItems(int degrees, GameObject itemPrefab)
    {
        Instantiate(itemPrefab, (PositionInCircle(degrees)), Quaternion.identity);
        isStatueUsed = true;
    }

    public Vector2 PositionInCircle(float angleDegrees)
    {
        // Convert the angle from degrees to radians
        float angleRadians = angleDegrees * Mathf.Deg2Rad;

        // Calculate the new position using vector addition
        float x = gameObject.transform.position.x + radius * Mathf.Cos(angleRadians);
        float y = gameObject.transform.position.y + radius * Mathf.Sin(angleRadians);

        return new Vector2(x, y);
    }
}
