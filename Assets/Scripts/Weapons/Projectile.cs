using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damageToGive;
    public int critChance;
    public int critMultiplier;
    private int currentDamage;

    public GameObject destroyEffect;
    public GameObject missEffect;
    public bool isEnemyObject;
    public string tag;
    
    private Transform player;
    private Vector2 target;

    public float mineDestroyTime = 5f;

    public bool projectileRotation;


    private void Start() {
        player = PlayerController2D.Instance.transform;
        target = new Vector2(player.position.x, player.position.y);
        if (projectileRotation) {
            transform.LookAt(player.transform);
        }
        StartCoroutine(DestroyMe());
    }

    private void Update() {
        if(tag == "Player")
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

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
            Destroy(gameObject);
    }
    IEnumerator DestroyMe() {
        yield return new WaitForSeconds(mineDestroyTime);
        GameObject effect = Instantiate(missEffect, transform.position, Quaternion.identity);
        yield return null;
        Destroy(this.gameObject);
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