using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerMm : MonoBehaviour
{
   public Camera miniMapCamera;
   public Transform player;
   public float playerOffSet = 10f;
   public float sizeMultipliyer = 0.7f;

   private void Start()
   {
     
   }

   private void Update()
   {
       if (player != null)
       {
           transform.position = new Vector3(player.position.x, player.position.y + playerOffSet, -0.3f);
       }
   }
}

