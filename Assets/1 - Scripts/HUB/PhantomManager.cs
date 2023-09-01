using System.Collections;
using UnityEngine;

public class PhantomManager : MonoBehaviour
{
    public Transform startPoint;
    public Transform middlePoint;
    public Transform endPoint;

    public float movementSpeed = 2.0f;
    public float delayAtEnd = 1.0f;

    private Vector3 targetPosition;
    private bool movingToEnd = false;

    private void Start()
    {
        targetPosition = middlePoint.position;
    }

    private void Update()
    {
        if (movingToEnd)
        {
            targetPosition = endPoint.position;

            transform.position = Vector2.MoveTowards(transform.position, endPoint.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, middlePoint.position, movementSpeed * Time.deltaTime);
        }

        if(transform.position == middlePoint.position)
        {
            movingToEnd = true;
        }

        if (transform.position == endPoint.position)
        {
            transform.position = startPoint.position;
            targetPosition = middlePoint.position;
            movingToEnd = false;
        }
    }

        
}