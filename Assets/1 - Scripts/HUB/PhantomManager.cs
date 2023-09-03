using System;
using UnityEngine;

public class PhantomManager : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 5.0f;
    private int currentWaypointIndex;
    private void Start()
    {
        currentWaypointIndex = 0;
    }

    private void Update()
    {
        // Calculating the direction towards the current waypoint
        Vector3 targetDirection = waypoints[currentWaypointIndex].position - transform.position;

        // Moving the object towards the current waypoint
        //transform.Translate(targetDirection.normalized * moveSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed);
        // Check if the object has reached the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.001f)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

            // If we reached the end, teleport to the first waypoint
            if (currentWaypointIndex == 0)
            {
                transform.position = waypoints[0].position;
            }
        }
    }
}
