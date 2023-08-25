using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomMove : MonoBehaviour
{
    public Transform initialPosition;
    public Transform finalPosition;

    private Transform currentTarget;
    public float speed = 3.0f;

    private void Start()
    {
        // Set the target to the portal
        currentTarget = finalPosition;
    }

    private void Update()
    {
        // Calculate the distance between itself and the target
        float distance = Vector2.Distance(transform.position, currentTarget.position);
        
        if (distance < 0.1f)
        {
            if (currentTarget == finalPosition)
            {
                transform.position = initialPosition.position;
            }
            else
            {
                currentTarget = finalPosition;
            }
                
        }

        // Moving the phantom to the portal
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
    }
}
