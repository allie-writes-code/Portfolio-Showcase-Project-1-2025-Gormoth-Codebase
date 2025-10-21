using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private Stat baseHealthStat;

    private int maxHealth;
    private int currentHealth;

    [SerializeField]
    private DeathLogic myDeathLogic;

    [SerializeField]
    private Image healthBarImage;
    [SerializeField]
    private Canvas healthBarCanvas;

    [SerializeField]
    public DelegateBroadcaster deathBroadcast;

    private void Start()
    {
        maxHealth = currentHealth = Mathf.RoundToInt(baseHealthStat.Value);
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    private void Update()
    {
        if (healthBarImage != null && healthBarCanvas != null)
        {
            if (currentHealth < maxHealth)
            {
                if (!healthBarCanvas.gameObject.activeInHierarchy) healthBarCanvas.gameObject.SetActive(true);
                float healthPercent = (float)currentHealth / (float)maxHealth;
                healthBarImage.fillAmount = healthPercent;
            }
            else
            {
                healthBarImage.fillAmount = 1;
                if (healthBarCanvas.gameObject.activeInHierarchy) healthBarCanvas.gameObject.SetActive(false);
            }
        }
    }

    public void Kill()
    {
        Hurt(currentHealth);
    }

    public void Hurt(int amt)
    {
        currentHealth -= amt;

        if (CurrentHealth <= 0) 
        {
            if (deathBroadcast != null) { deathBroadcast.InvokeMe(); }

            if (myDeathLogic != null)
            {
                if (myDeathLogic.DeathObject == null) myDeathLogic.DeathObject = this.gameObject;
                myDeathLogic.Die();
            }
        }
    }

    public void Heal(int amt)
    {
        currentHealth += amt;

        if (CurrentHealth > maxHealth) { currentHealth = maxHealth; }
    }
}
