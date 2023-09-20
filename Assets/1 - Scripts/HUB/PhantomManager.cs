using System;
using UnityEngine;
using DG.Tweening;
using DG;
using Random = UnityEngine.Random;

public class PhantomManager : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform waypointInit;
    public float moveSpeed = 5.0f;
    public Transform phantom;

    private bool moving = true;
    /*private int currentWaypointIndex = 0;

    private void Update()
    {
        // Calculating the direction towards the current waypoint
        Vector3 targetDirection = waypoints[currentWaypointIndex].position - transform.position;

        // Moving the object towards the current waypoint
        transform.Translate(targetDirection.normalized * moveSpeed * Time.deltaTime);

        // Check if the object has reached the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

            // If we reached the end, teleport to the first waypoint
            if (currentWaypointIndex == 0)
            {
                transform.position = waypoints[0].position;
            }
        }
    }*/
    private void Start()
    {
        phantom.position = waypointInit.position;
       
        Move();
    }

    void Move()
    {
        moving = false;
        phantom.DOMove(waypoints[0].position, Random.Range(10f, 12f)).OnComplete(() =>
        {
            phantom.DOMove(waypoints[1].position, Random.Range(3f, 5f)).OnComplete(() =>
            {
                phantom.position = waypointInit.position;
                moving = true;
                DOTween.Kill(transform);
            });
        });
    }
   private void Update()
   {
       if (moving == true)
       {
           Move();
       }
       /*phantom.DOMove(waypoints[1].position, Random.Range(1f, 2f)).OnComplete(() =>
       {
           phantom.DOMove(waypoints[2].position, Random.Range(1f, 2f)).OnComplete(() =>
           {
               phantom.position = waypoints[0].position;
           });
       });*/
    }
}
