using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour {
    // Start is called before the first frame update
    private Transform aimTransform;

    private Animator aimAnimator;

    private void Awake() {
        aimTransform = transform.Find("Aim");
        aimAnimator = aimTransform.GetComponent<Animator>();
    }

    private void Update() {
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming() {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void HandleShooting() {
        if (Input.GetMouseButtonDown(0)) {
            aimAnimator.SetTrigger("Shoot");
        }
    }


    public static Vector3 GetMouseWorldPosition() {
        Vector3 mouseScreenPosition = Input.mousePosition; // Get mouse position in screen space
        mouseScreenPosition.z = 0f; // Set the Z value
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition); // Convert to world space
        return mouseWorldPosition;
    }
}
