//Course: GPE104
//Prof: Matthew Henry
//Student: Chad V

using UnityEngine;
using System.Collections;

public class PaddlePawn: Pawn
{
   public float moveSpeed = 3;
   public float turnSpeed = 90;
   public Transform tfBase;
   public Transform tfTurrent;
   public GameObject someBullet;
   public GameObject someBulletSpawn;
   public Vector2 paddleVelocity;
   private Vector2 previousPosition;

   // move transform by some Vector3 * moveSpeed * Time.deltaTime.
   public override void Move(Vector3 someVec3)
   {
       tfBase.position += someVec3 * moveSpeed * Time.deltaTime;
   }

   // rotate the turret by some angle
   public override void Rotate(float someAngle)
   {
       tfTurrent.Rotate(0,0,someAngle * turnSpeed * Time.deltaTime);
   }

   public override void Shoot()
   {
        var staggerCnt = UnityEngine.Random.Range(0,1);
        if (staggerCnt == 1)
        {
            var someOffsetVec3 = new Vector3(-0.2f, 0f, 0f);
            var somePos = someBulletSpawn.transform.position - someOffsetVec3;
            GameObject spawned = Instantiate(someBullet, somePos, Quaternion.identity);
            spawned.transform.rotation = tfTurrent.rotation;
        }
        else
        {
            var someOffsetVec3 = new Vector3(0.2f, 0f, 0f);
            var somePos = someBulletSpawn.transform.position + someOffsetVec3;
            GameObject spawned = Instantiate(someBullet, somePos, Quaternion.identity);
            spawned.transform.rotation = tfTurrent.rotation;
        }
   }

   // on start get ref to Transform and Shooter.
   public override void Start()
   {
        previousPosition = tfBase.position;
        tfBase = transform.Find("Heroiblock_0");
        tfTurrent = transform.Find("Heroiblock_0/Heroi_0");
   }


    public void UpdatePaddleVelocity()
    {
        Vector2 currentPos = new Vector2(tfBase.position.x, tfBase.position.y);
        paddleVelocity = (currentPos - previousPosition) / Time.deltaTime;
        previousPosition = tfBase.position;
    }       
}