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
   public GameObject someBullet;
   public GameObject someBulletSpawn;
   public AudioSource someAudioSource;
   public AudioClip someAudioClip;

   // move transform by some Vector3 * moveSpeed * Time.deltaTime.
   public override void Move(Vector3 someVec3)
   {
       tfBase.position += someVec3 * moveSpeed * Time.deltaTime;
   }

   // rotate the turret by some angle
   public override void Rotate(float someAngle)
   {
        //NOT USED
        Debug.Log("Would have rotated turrent.");
   }

   public override void Shoot()
   {
        var someOffsetVec3 = new Vector3(-0.2f, 0f, 0f);
        var somePos = someBulletSpawn.transform.position - someOffsetVec3;
        GameObject spawned = Instantiate(someBullet, somePos, Quaternion.identity);
        spawned.transform.rotation = tfBase.rotation;
   }

   // on start get ref to Transform and Shooter.
   public override void Start()
   {
       tfBase = transform;
   }

   void OnDestroy()
   {
        Vector3 someVec3 = tfBase.position;
        if (someAudioClip!= null)
        {
            AudioSource.PlayClipAtPoint(someAudioClip, someVec3);    
        }
        
   }

}