using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class TransitionEdgeToBox : MonoBehaviour
{
    public float transitionTime = 2f;
    private float transitionTimer = 0f;
    private bool isTransitioning = false;

    private BoxCollider2D boxCollider;
    private EdgeCollider2D edgeCollider;

    private Vector2[] originalEdgePoints;
    private Vector2[] targetEdgePoints;

    private int playerLayer; // The layer index for the "Player" layer

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        // Store the original EdgeCollider2D points
        originalEdgePoints = edgeCollider.points;

        // Calculate the target EdgeCollider2D points to match the BoxCollider2D
        targetEdgePoints = CalculateEdgePointsToMatchBox();

        // Get the layer index for the "Player" layer
        playerLayer = LayerMask.NameToLayer("Player");

    }

    private Vector2[] CalculateEdgePointsToMatchBox()
    {
        // Calculate the corner points of the box
        Vector2 size = boxCollider.size;
        Vector2 offset = boxCollider.offset;

        Vector2 topLeft = offset - size / 2f;
        Vector2 topRight = offset + new Vector2(size.x / 2f, -size.y / 2f);
        Vector2 bottomLeft = offset + new Vector2(-size.x / 2f, size.y / 2f);
        Vector2 bottomRight = offset + size / 2f;

        // Create an array with the target EdgeCollider2D points
        return new Vector2[] { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
    }

    private Vector2[] CalculateIntermediateEdgePoints(Vector2[] startPoints, Vector2[] endPoints, float t)
    {
        // Interpolate between start and end points based on time
        Vector2[] lerpedPoints = new Vector2[startPoints.Length];
        for (int i = 0; i < startPoints.Length; i++)
        {
            lerpedPoints[i] = Vector2.Lerp(startPoints[i], endPoints[i], t);
        }

        // Create an array with the intermediate EdgeCollider2D points
        return lerpedPoints;
    }

    private IEnumerator TransitionEdgeCollider()
    {
        isTransitioning = true;

        while (transitionTimer < transitionTime)
        {
            // Interpolate between original and target points based on time
            float t = transitionTimer / transitionTime;
            Vector2[] intermediatePoints = CalculateIntermediateEdgePoints(originalEdgePoints, targetEdgePoints, t);

            // Set the EdgeCollider2D points
            edgeCollider.points = intermediatePoints;

            // Update the timer
            transitionTimer += Time.deltaTime;

            yield return null;
        }

        // Ensure that the EdgeCollider2D ends up with the exact target points
        edgeCollider.points = targetEdgePoints;

        isTransitioning = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(TransitionEdgeCollider());
        }
    }

}
