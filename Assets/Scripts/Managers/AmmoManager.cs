
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

    public void AddAmmo(int amountOfAmmo) {
        currentAmountOfAmmo += amountOfAmmo;
        if (currentAmountOfAmmo > maxAmmo) {
            currentAmountOfAmmo = maxAmmo;
        }
    }
}
