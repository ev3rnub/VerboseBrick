using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 500;
    public int minHealth = 0;
    public int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if (gameObject.CompareTag("Brick")){
            currentHealth = 25;
            Debug.Log($"Default Health: 25, setting current health to of {transform.name} to default");
        }
        else if (gameObject.CompareTag("Paddle"))
        {
            currentHealth = 200;
            GameManager.someGameManager.paddleHealth = currentHealth;
        }
        else
        {
            currentHealth = 100;
            Debug.Log($"Default Health: {maxHealth}, setting current health to of {transform.name} to default");
        }
          
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            Debug.Log($"KILL {this.name}");
            GameManager.someGameManager.SomeGameObjectDied(this.gameObject);
            Die();

        }
    }

    public int AddHealth(int someAmt)
    {
        if (currentHealth <= maxHealth && someAmt != null)
        {
            currentHealth += someAmt;
        }
        else
        {
            currentHealth = maxHealth;
        }

        return currentHealth;
    }

    public int SubHealth(int someAmt)
    {
        if (currentHealth >= minHealth && someAmt != null)
        {
            currentHealth -= someAmt;
        }
        else
        {
            currentHealth = minHealth;
        }

        return currentHealth;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
