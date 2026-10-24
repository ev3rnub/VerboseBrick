using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    public GameManager someGameManager;
    public float bulletForce = 100;
    private Rigidbody2D rb;
    private Transform tf;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();

        //GameManager.someGameManager.bulletList.Add(this);

        if (rb != null && tf != null)
        {
            rb.AddForce(tf.up * bulletForce);
            // this will ensure we don't run out of memory due to 10000000s of bullets. After 10 seconds the bullet is destroyed.
            StartCoroutine(DestroyAfterTime());
        }

        // after 10 seconds destroy bullet.
        IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(5f);
            Destroy(this.gameObject);
        }        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //collisions NOT WORKING BOOO,
        Debug.Log("BULLET OnCollisionEnter2D fired with: " + collision.gameObject.name + " tag: " + collision.gameObject.tag);
        
        if (collision.gameObject.CompareTag("Brick"))
        {
            // GameManger.Instance....
            Debug.Log("BRICK======");
            someGameManager.targetHealth = collision.gameObject.GetComponent<Health>();
            Destroy(this.gameObject);
            someGameManager.BulletHit();
        }
        else if (collision.gameObject.CompareTag("EnemyPaddle"))
        {
            // GameManger.Instance....Need to change this as I added enemyblock tag. 
            Debug.Log("ENEMYPADDLE======");
            someGameManager.targetHealth = collision.gameObject.GetComponent<Health>();
            Destroy(this.gameObject);
            someGameManager.BulletHit();          
        }
        else if (collision.gameObject.CompareTag("Paddle"))
        {
            // GameManger.Instance....Need to change this as I added enemyblock tag. 
            Debug.Log("PADDLE======");
            someGameManager.targetHealth = collision.gameObject.GetComponent<Health>();
            someGameManager.BulletHit();
        }
    }
}
