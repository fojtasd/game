using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] private PlayerShootingManager playerAimWeapon;

    private void Start() {
        playerAimWeapon.OnShoot += PlayerAimWeapon_OnShoot;
    }

    private void PlayerAimWeapon_OnShoot(object sender, PlayerShootingManager.OnShootEventArgs e) {
        CreateWeaponTracer(e.shootPosition, e.gunEndPointPosition);
    }

    private void CreateWeaponTracer(Vector3 shootPosition, Vector3 gunEndPointPosition) {

    }
}
