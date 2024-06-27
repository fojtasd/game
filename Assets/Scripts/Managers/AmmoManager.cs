
public class AmmoManager {
    int currentAmountOfAmmo;
    readonly int maxAmmo;


    public AmmoManager(int currentAmountOfAmmo, int maxAmmo) {
        this.currentAmountOfAmmo = currentAmountOfAmmo;
        this.maxAmmo = maxAmmo;
    }

    public int GetCurrentAmountOfAmmo() {
        return currentAmountOfAmmo;
    }

    public int GetMaxAmmo() {
        return maxAmmo;
    }

    public void AddAmmo(int amountOfAmmo) {
        if (currentAmountOfAmmo + amountOfAmmo >= maxAmmo) {
            currentAmountOfAmmo = maxAmmo;
        } else {
            currentAmountOfAmmo += amountOfAmmo;
        }
    }
}
