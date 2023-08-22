using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private int x_Room;
    private int y_Room;

    void Awake()
    {
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
    }


    public void UpdateEdgeCollider(int x, int y)
    {

    }
}
