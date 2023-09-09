using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerMm : MonoBehaviour
{
   public Transform player;
   public float playerOffSet = 10f;


   private void Update()
   {
      transform.position = new Vector3(player.position.x,player.position.y + playerOffSet, -0.3f);
   }
}
