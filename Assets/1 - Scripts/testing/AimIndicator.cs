using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    public Transform pointA;    // The pivot point
    public Transform pointB;    // The point to aim towards
    public float stretchingSpeed = 1.0f;
    public float rotationSpeed = 45.0f;
    public float maxXScale = 10.0f; // Maximum X scale limit

    private Transform spriteTransform;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteTransform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSpriteVisible(false);
    }

    private void Update()
    {
        if (pointA.position != pointB.position)
        {
            SetSpriteVisible(true);

            if (pointA != null && pointB != null)
            {
                // Calculate the direction from point A to point B
                Vector3 directionAB = pointB.position - pointA.position;

                // Rotate the sprite to face the direction
                float angle = Mathf.Atan2(directionAB.y, directionAB.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                spriteTransform.rotation = Quaternion.RotateTowards(spriteTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Stretch the sprite along the X-axis to match the distance between point A and point B
                float distanceAB = directionAB.magnitude;
                float newXScale = Mathf.Clamp(distanceAB, 0, maxXScale); // Limit X scale
                spriteTransform.localScale = new Vector3(newXScale, spriteTransform.localScale.y, spriteTransform.localScale.z);

                // Position the sprite's pivot at point A
                spriteTransform.position = pointA.position;
            }
        }
        else if (pointA.position == pointB.position)
        {
            SetSpriteVisible(false);
        }
    }

    // Helper method to set sprite visibility
    private void SetSpriteVisible(bool isVisible)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = isVisible;
        }
    }
}
