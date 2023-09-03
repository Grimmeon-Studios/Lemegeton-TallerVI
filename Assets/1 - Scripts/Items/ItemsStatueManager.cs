using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsStatueManager : MonoBehaviour
{
    public GameObject[] pickupPrefabs; // Array de prefabs de los objetos a generar
    public Transform[] spawnPoint; // Punto donde se generarán los objetos
    public float spawnInterval = 2.0f; // Intervalo entre generaciones de objetos
    public int numberOfPickups = 3; // Número de objetos a generar

    private List<GameObject> spawnedPickups = new List<GameObject>();

    private void Start()
    {
        GeneratePickups();
    }

    private void GeneratePickups()
    {
        for (int i = 0; i < numberOfPickups; i++)
        {
            int randomIndex = Random.Range(0, pickupPrefabs.Length); // Elegir un objeto al azar
            GameObject pickup = Instantiate(pickupPrefabs[randomIndex], spawnPoint[i].position, Quaternion.identity);
            spawnedPickups.Add(pickup);
        }
    }
    public List<GameObject> GetSpawnedPickups()
    {
        return spawnedPickups;
    }
    public void PickupSelected(GameObject selectedPickup)
    {
        // Desactivar y destruir los objetos que no fueron seleccionados
        foreach (GameObject pickup in spawnedPickups)
        {
            if (pickup != selectedPickup)
            {
                pickup.SetActive(false);
                Destroy(pickup);
            }
        }
        spawnedPickups.Clear();
    }
}
