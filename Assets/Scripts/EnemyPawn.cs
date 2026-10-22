//Course: GPE104
   //Prof: Matthew Henry
   //Student: Chad V

   using UnityEngine;
   using System.Collections;
   using System.Collections.Generic;

   public class EnemyPawn: Pawn
   {
       public float moveSpeed = 3;
       public Transform tfBase;

       // move transform by some Vector3 * moveSpeed * Time.deltaTime.
       public override void Move(Vector3 someVec3)
       {
           tfBase.position += someVec3 * moveSpeed * Time.deltaTime;
       }

       // rotate the turret by some angle
       public override void Rotate(float someAngle)
       {
            Debug.Log("Would have rotated turrent.");
       }

       // on start get ref to Transform and Shooter.
       public override void Start()
       {
           tfBase = transform;
       }

   }