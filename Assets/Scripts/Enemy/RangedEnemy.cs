using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float speed;
    public float chasingDistance;
    public float stoppingDistance;
    public float retreatDistance;

    private float timeBtwShots;
    public float startTimeBtwShots = 1f;

    public GameObject projectile;
    private Transform player;
    public Transform shootingPoint;
    public Transform middlePosition;

    public bool boss;
    private bool bossMusicStarted = false;

    private void Start() {
        player = PlayerController2D.Instance.transform;

        timeBtwShots = startTimeBtwShots;
    }
    private void Update() {
        ChaseOrStayOrRetreat();
        SpawnProjectile();
    }

    private void SpawnProjectile() {
        if (timeBtwShots <= 0 && Vector2.Distance(transform.position, player.position) < stoppingDistance) {
            Instantiate(projectile, shootingPoint.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        } else {
            timeBtwShots -= Time.deltaTime;
        }
    }

    private void ChaseOrStayOrRetreat() {
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance && Vector2.Distance(transform.position, player.position) < chasingDistance) {
            transform.position = Vector2.MoveTowards(transform.position,
                player.position, speed * Time.deltaTime);

        } else if (Vector2.Distance(transform.position, player.position) < stoppingDistance
            && Vector2.Distance(transform.position, player.position) > retreatDistance) {
            transform.position = this.transform.position;
            if (boss) {
                StartBossFight();
            }
        } else if (Vector2.Distance(transform.position, player.position) < retreatDistance) {
            transform.position = Vector2.MoveTowards(transform.position,
                player.position, -speed * Time.deltaTime);
        }
    }

    private void StartBossFight() {
        //Start boss music
        if (bossMusicStarted != true) {
            //SFXManager.Instance.PlaySound(SFXManager.Instance.bossSpawnGrunt);
            //SFXManager.Instance.PlayBossMusic(SFXManager.Instance.bossTrackDrums);
            bossMusicStarted = true;
        }

    }

    //Showing chasing and unchasing distance gizmos
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(middlePosition.position, retreatDistance);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(middlePosition.position, chasingDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(middlePosition.position, stoppingDistance);

    }
}