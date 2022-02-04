using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public string tag;
    public GameObject destroyEffect;
    public GameObject missEffect;
    public bool isEnemyObject;

    public int damageToGive;
    public int critChance;
    public int critMultiplier;
    private int currentDamage;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(tag)) {
            DestroyProjectile(collision);
            collision.gameObject.GetComponent<HealthManager>().Hurt(DamageCalculation());
        }
    }
    private void DestroyProjectile(Collider2D collision) {

        //PlaySound
        if (destroyEffect != null) {
            GameObject effect = Instantiate(destroyEffect, collision.transform);
            Destroy(effect, 3f);
        }
        if (isEnemyObject != true)
            gameObject.SetActive(false);
    }

    public int DamageCalculation() {
        currentDamage = damageToGive - PlayerStats.Instance.currentDefense;
        currentDamage = Crit(currentDamage);
        if (currentDamage <= 0)
            currentDamage = 1;

        return currentDamage;
    }

    private int Crit(int damage) {
        int crit = Random.Range(0, 100);
        if (crit > (100 - critChance))
            return currentDamage *= critMultiplier;
        else {
            return damage;
        }
    }
}
