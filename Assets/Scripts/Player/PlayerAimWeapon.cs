using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimWeapon : MonoBehaviour {
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }
    Transform aimTransform;
    Transform aimGunEndPointTransform;
    Animator aimAnimator;
    Animator shotgunAnimator;

    bool facingRight = true;
    bool canShoot = true;

    float shootingTime = 0f;
    const float shootingCooldown = 1f;

    void Awake() {
        aimTransform = transform.Find("Aim");
        aimAnimator = aimTransform.Find("Muzzle").GetComponent<Animator>();
        shotgunAnimator = aimTransform.GetComponent<Animator>();
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
    }

    void Update() {
        HandleAiming();
        Cooldown();
    }

    void OnEnable() {
        PlayerMovementInputSystem.OnWalkEvent += SetFacingDirection;
    }

    void OnDisable() {
        PlayerMovementInputSystem.OnWalkEvent -= SetFacingDirection;
    }

    public void HandleShooting(InputAction.CallbackContext ctx) {
        if (IsInState("AimShoot_Shoot")) {
            return;
        }
        if (canShoot && ctx.started) {
            aimAnimator.SetTrigger("Shoot");
            OnShoot?.Invoke(this, new OnShootEventArgs {
                gunEndPointPosition = aimGunEndPointTransform.position,
                shootPosition = GetMouseWorldPosition()
            });
            shootingTime = 0f;
            StartCoroutine(WaitAndPump());
        }
    }

    public static Vector3 GetMouseWorldPosition() {
        Vector3 mouseScreenPosition = Input.mousePosition; // Get mouse position in screen space
        mouseScreenPosition.z = 0f; // Set the Z value
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition); // Convert to world space
        return mouseWorldPosition;
    }

    void SetFacingDirection(float walkingDirection) {
        if (walkingDirection > 0) {
            facingRight = true;
        }
        else if (walkingDirection < 0) {
            facingRight = false;
        }
    }

    void HandleAiming() {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //Debug.Log(angle);
        if (facingRight) {
            //TODO fix the angle

            if ((angle >= 0 && angle <= 50) || (angle <= 0 && angle >= -50)) {
                aimTransform.eulerAngles = new Vector3(0, 0, angle);
            }
            else if (angle > 50 && angle < 180) {
                aimTransform.eulerAngles = new Vector3(0, 0, angle);
            }
            else if (angle > -180 && angle < -50) {
                aimTransform.eulerAngles = new Vector3(0, 0, -50);
            }
        }
        else {
            if ((angle >= 0 && angle <= 50) || (angle <= 0 && angle >= -50)) {
                aimTransform.eulerAngles = new Vector3(0, 180, angle);
            }
            else if (angle > 50 && angle < 180) {
                aimTransform.eulerAngles = new Vector3(0, 180, angle);
            }
            else if (angle > -180 && angle < -50) {
                aimTransform.eulerAngles = new Vector3(0, 180, -50);
            }
        }
    }

    void Cooldown() {
        shootingTime += Time.deltaTime;
        if (shootingTime > shootingCooldown) {
            canShoot = true;
        }
        else {
            canShoot = false;
        }
    }

    IEnumerator WaitAndPump() {
        yield return new WaitForSeconds(0.2f);
        shotgunAnimator.SetTrigger("Pump");
    }

    bool IsInState(string stateName) {
        AnimatorStateInfo stateInfo = aimAnimator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName);
    }


}
