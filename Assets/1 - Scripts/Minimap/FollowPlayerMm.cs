using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerMm : MonoBehaviour
{
    public Transform mapLimits;
    public Camera miniMapCamera;
    public Transform player;
    public float playerOffSet = 10f;
    public float sizeMultiplier = 0.7f;

    private void Start()
    {
        if (mapLimits != null)
        {
            // Change the minimap camera size according to the mapLimits
            Vector3 mapSize = mapLimits.GetComponent<Renderer>().bounds.size;
            miniMapCamera.orthographicSize = Mathf.Max(mapSize.x, mapSize.y) * sizeMultiplier;

            // Center the minimap camera into the mapLimits.
            transform.position = new Vector3(mapLimits.position.x, mapLimits.position.y, -0.3f);
        }
    }
    
}
/* public Transform mapLimits;
   public Camera miniMapCamera;
   public Transform player;
   public float playerOffSet = 10f;
   public float sizeMultipliyer = 0.7f;

   private void Start()
   {
       if (mapLimits != null)
       {
           Vector3 mapSize = mapLimits.GetComponent<Renderer>().bounds.size;
           miniMapCamera.orthographicSize = Mathf.Max(mapSize.x, mapSize.y) * 0.7f;
           transform.position = new Vector3(mapLimits.position.x, mapLimits.position.y, -0.3f);
       }
   }

   private void Update()
   {
       if (player != null)
       {
           transform.position = new Vector3(player.position.x, player.position.y * playerOffSet, -0.3f);
       }
   }*/
