
public class HealthManager {
    int currentHealth;
    readonly int maxHealth;

    public enum HealthState {
        Healthy,
        SlighlyHurt,
        Hurt,
        BadlyHurt,
        Critical,
        Death
    }
    public HealthManager(int health, int maxHealth) {
        this.maxHealth = maxHealth;
        currentHealth = health;
    }

    public int GetHealth() {
        return currentHealth;
    }

    public void TakeDamage(int damageAmount) {
        currentHealth -= damageAmount;
    }

    public void Heal(int healAmount) {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }

    public float GetHealthPercentage() {
        return (float)currentHealth / maxHealth;
    }

    public HealthState GetHealthState() {
        float healthPercentage = GetHealthPercentage();
        if (healthPercentage > 0.8f) {
            return HealthState.Healthy;
        } else if (healthPercentage > 0.6f) {
            return HealthState.SlighlyHurt;
        } else if (healthPercentage > 0.4f) {
            return HealthState.Hurt;
        } else if (healthPercentage > 0.2f) {
            return HealthState.BadlyHurt;
        } else if (healthPercentage > 0f) {
            return HealthState.Critical;
        } else {
            return HealthState.Death;
        }
    }
}
