using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int minHealth = 0;
    public int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"Default Health: {maxHealth}, setting current health to of {transform.name} to default");
        currentHealth = maxHealth;  
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
