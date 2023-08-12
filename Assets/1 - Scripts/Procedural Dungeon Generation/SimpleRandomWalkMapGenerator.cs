using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkMapGenerator : MonoBehaviour
{
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

    [SerializeField] private int iterations = 10; //number of iterations we want for RandomWalk algorithim
    [SerializeField] public int walkLenght = 10;
    [SerializeField] public bool startRandomlyEachItetarion = true; //we'll use it to tweak the ReandomWalk

    public void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        
        foreach (var position in floorPositions)
        {
            Debug.Log(position);
        }
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currenPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currenPosition, walkLenght);
            floorPositions.UnionWith(path); //UnionWith() allows us to add positions from path without duplicating them in case the random walk runs over a floor that has been set already
            if (startRandomlyEachItetarion)
                currenPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }
        return floorPositions;
    }
}
