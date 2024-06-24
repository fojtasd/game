using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] private PlayerAimWeapon playerAimWeapon;

    private void Start() {
        playerAimWeapon.OnShoot += PlayerAimWeapon_OnShoot;
    }

    private void PlayerAimWeapon_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e) {
        Debug.Log("mouse position: " + e.shootPosition);
        Debug.Log("shoot postion: " + e.gunEndPointPosition);
        Debug.DrawLine(e.shootPosition, e.gunEndPointPosition, Color.white, 5f);
        CreateWeaponTracer(e.shootPosition, e.gunEndPointPosition);
    }

    private void CreateWeaponTracer(Vector3 shootPosition, Vector3 gunEndPointPosition) {
        Debug.DrawLine(gunEndPointPosition, shootPosition, Color.white, 5f);
    }
}
