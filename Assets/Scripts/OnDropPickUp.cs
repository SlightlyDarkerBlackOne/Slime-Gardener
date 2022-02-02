using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDropPickUp : MonoBehaviour
{
    public PlayerAimWeapon.WeaponState state;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            PlayerController2D.Instance.GetComponent<PlayerAimWeapon>().state = state;
            Destroy(gameObject, 0.1f);
        }
    }
}