using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;

public class Shooting : MonoBehaviour
{
    private Transform aimTransform;
    Vector3 aimDirection;
    public GameObject arrowPrefab;
    public Transform endPointPosition;
    private float bowAttackCooldown = 0;
    public float bowAttackTime;
    private float angle;

    public bool rangedWeaponEquiped;

    private void Awake() {
        aimTransform = transform.Find("Animation").Find("Weapons").Find("Aim");
        aimDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming() {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

        Vector3 playerCenterPosition = new Vector3(transform.position.x - 0.05f,
                transform.position.y + 0.3f, transform.position.z);

        aimDirection = (mousePosition - playerCenterPosition).normalized;
        angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }
    private void HandleShooting() {
        if (!PlayerController2D.Instance.playerFrozen && rangedWeaponEquiped) {
            if (Input.GetButtonDown("Fire1")) {
                //if (EventSystem.current.IsPointerOverGameObject()) return;

            } else if (Input.GetButtonUp("Fire1") && bowAttackCooldown > 0) {

            } else if (Input.GetButtonUp("Fire1") && bowAttackCooldown <= 0) {
                //if (EventSystem.current.IsPointerOverGameObject()) return;
                //SFXManager.Instance.PlayBowFireSound();

                bowAttackCooldown = bowAttackTime;

                SingleArrow();
                //FireMultipleArrows(6);
            }
            bowAttackCooldown -= Time.deltaTime;
        }
    }
    private void SingleArrow() {
        GameObject arrow = Instantiate(arrowPrefab, endPointPosition.position, Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = aimDirection * 15.0f;
        arrow.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);
        //Range for now
        Destroy(arrow, 0.3f);
    }
    private void FireMultipleArrows(int numOfArrows) {
        float offset = 30f;
        for (int i = 0; i < numOfArrows; i++) {
            Quaternion newAngle = Quaternion.AngleAxis((offset * (i - (numOfArrows / 2))), transform.up);
            if (i % 2 == 0)
                aimTransform.eulerAngles = new Vector3(0, 0, angle + i * offset);
            else
                aimTransform.eulerAngles = new Vector3(0, 0, angle - i * offset);
            SingleArrow();
        }
    }

    public void SetActiveAimWeaponTransform(Transform newAimTransform) {
        aimTransform = newAimTransform;
        endPointPosition = aimTransform.Find("EndPointPosition").transform;
        bowAttackTime = aimTransform.Find("Bow").GetComponent<RangedWeapon>().baseAttackTime;

        rangedWeaponEquiped = true;
    }
    public void RangedWeaponNotEquipped() {
        rangedWeaponEquiped = false;
    }
}