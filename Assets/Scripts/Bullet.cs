using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    public GameManager someGameManager;
    public float bulletForce = 100;
    private Rigidbody2D rb;
    private Transform tf;
    public AudioSource hitAudioSource;
    public AudioClip hitAudioClip;
    public AudioClip hitAudioClipII;
    
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
            // activate particle system on bullet hit from player/self.
            ParticleSystem[] someVFX = collision.gameObject.GetComponentsInChildren<ParticleSystem>();
            if (someVFX == null)
            {
                Debug.Log("SOMEVFX is NULL");
            }
            else
            {
                foreach (ParticleSystem somePS in someVFX)
                {
                    somePS.transform.localScale = new Vector3(.5f, .5f, .5f);
                    somePS.Play();
                }
                
            }
            Debug.Log("BULLET HIT SOUND");
            hitAudioSource.PlayOneShot(hitAudioClip, .9f);
            Debug.Log("BRICK======");
            someGameManager.targetHealth = collision.gameObject.GetComponent<Health>();
            Destroy(this.gameObject, hitAudioClip.length);
            someGameManager.BulletHit();
            
        }
        else if (collision.gameObject.CompareTag("EnemyPaddle"))
        {
            
            // activate particle system on bullet hit from player/self.
            ParticleSystem[] someVFX = collision.gameObject.GetComponentsInChildren<ParticleSystem>();
            if (someVFX == null)
            {
                Debug.Log("SOMEVFX is NULL");
            }
            else
            {
                foreach( ParticleSystem somePS in someVFX)
                {
                    somePS.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    somePS.Play();
                }
                
            }
            hitAudioSource.PlayOneShot(hitAudioClip, .9f);
            Debug.Log("ENEMYPADDLE======");
            someGameManager.targetHealth = collision.gameObject.GetComponent<Health>();
            Destroy(this.gameObject, hitAudioClip.length);
            someGameManager.BulletHit();
            
        }
        else if (collision.gameObject.CompareTag("Paddle"))
        {
            // GameManger.Instance....Need to change this as I added enemyblock tag. 
            Debug.Log("PADDLE======");
            someGameManager.targetHealth = collision.gameObject.GetComponent<Health>();
            someGameManager.BulletHit();
        }
        else if (collision.gameObject.CompareTag("WallBot"))
        {
            Debug.Log("====== BULLET HIT BOTWALL ======");
        }
        else if (collision.gameObject.CompareTag("Ball"))
        {
            hitAudioSource.PlayOneShot(hitAudioClipII, .9f);
        }
    }
}
