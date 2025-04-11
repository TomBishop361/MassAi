
using UnityEngine;

public class Health : MonoBehaviour
{
    float maxHealth;
    float currentHealth = 0;
    public HealthBar healthBar;

    public delegate void HealthDepleted();
    public event HealthDepleted healthDepleted;

    public float _maxHealth { private get {  return maxHealth; } set { maxHealth = value; currentHealth += (maxHealth - currentHealth); } }

    public void AdjustHealth(int healthChange)
    {
        Debug.Log(healthChange);
        currentHealth = Mathf.Clamp(currentHealth + healthChange, 0, maxHealth);

        healthBar.setHealth((float)currentHealth / (float)maxHealth);

        if (currentHealth < maxHealth) healthBar.displayHealthBar();

        if (currentHealth == maxHealth) healthBar.hideHealthBar();

        if(currentHealth == 0) healthDepleted?.Invoke();
    }
}
