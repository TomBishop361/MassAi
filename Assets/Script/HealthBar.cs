using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;

    public void setHealth(float health)
    {
        healthBar.fillAmount = health;
    }

    public void hideHealthBar()
    {
        gameObject.SetActive(false);
    }

    public void displayHealthBar()
    {
        gameObject.SetActive(true);
    }
}
