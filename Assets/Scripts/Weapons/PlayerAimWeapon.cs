using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;

public class PlayerAimWeapon : MonoBehaviour
{
    public enum WeaponState
    {
        Basic,
        Burst,
    }

    public WeaponState state = WeaponState.Basic;

    private Transform aimTransform;
    private Animator aimAnimator;
    Vector3 aimDirection;
    public GameObject bulletGO;
    public Transform endPointPosition;
    private float attackCooldown;
    public float fireRate = 0.1f;
    private float spreadFactor = 5;
    public float pelletCount = 5;
    private float angle;

    public float burst = 5f;


    public float ConeSize = 5f;

    public bool rangedWeaponEquiped;

    public bool canShoot;
    public bool isNight;
    public int bullets = 10;

    public FillBucketWithWater bucket;

    private void Awake()
    {
        aimTransform = transform.Find("Animation").Find("Weapons").Find("Aim");
        aimAnimator = aimTransform.GetComponentInChildren<Animator>();
        aimDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

        Vector3 playerCenterPosition = new Vector3(transform.position.x - 0.05f,
            transform.position.y + 0.3f, transform.position.z);

        aimDirection = (mousePosition - playerCenterPosition).normalized;
        angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void HandleShooting()
    {
        if (!PlayerController2D.Instance.playerFrozen && rangedWeaponEquiped)
        {
            if (Input.GetButtonUp("Fire1") && attackCooldown <= 0 && ((canShoot && bullets > 0) || isNight))
            {
                bullets--;
                if(bullets <= 0) {
                    bullets = 0;
                    bucket.EmptyBucket();
                }
                //if (EventSystem.current.IsPointerOverGameObject()) return;
                //TODO: set fire sound
                // SFXManager.Instance.PlayBowFireSound();

                //  aimAnimator.SetTrigger("Shoot");
                //  aimAnimator.SetBool("Drawing", false);
                attackCooldown = fireRate;

                switch (state)
                {
                    case WeaponState.Basic:
                        BasicBulletShooting();
                        break;
                    case WeaponState.Burst:
                        StartCoroutine(burstFireShooting());
                        break;
                    default:
                        BasicBulletShooting();
                        break;
                }
            }

            attackCooldown -= Time.deltaTime;
        }
    }

    private void BasicBulletShooting()
    {
        GameObject bullet = Instantiate(this.bulletGO, endPointPosition.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = aimDirection * 15.0f;
        bullet.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);

        SFXManager.Instance.PlaySound(SFXManager.Instance.alivePlant);

        //Range for now
        Destroy(bullet, 0.8f);
    }

    private void ShotGunShooting()
    {
        /*GameObject bullet = Instantiate(this.bulletGO, endPointPosition.position, Quaternion.identity);
         bullet.GetComponent<Rigidbody2D>().velocity = aimDirection * 15.0f;*/

        for (int i = 0; i < pelletCount; i++)
        {
            Quaternion pelletRot = transform.rotation;
            pelletRot.x += Random.Range(-spreadFactor, spreadFactor);
            pelletRot.y += Random.Range(-spreadFactor, spreadFactor);
            GameObject bullet = Instantiate(this.bulletGO, endPointPosition.position, pelletRot);
            bullet.GetComponent<Rigidbody2D>().velocity = aimDirection * 15.0f;

            // pellet = Instantiate(pelletPrefab, transform.position, pelletRot);
            //pellet.velocity = transform.forward*pelletSpeed;
        }
    }

    public void SetActiveAimWeaponTransform(Transform newAimTransform)
    {
        aimTransform = newAimTransform;
        aimAnimator = aimTransform.GetComponentInChildren<Animator>();
        endPointPosition = aimTransform.Find("EndPointPosition").transform;
        fireRate = aimTransform.Find("Bow").GetComponent<RangedWeapon>().baseAttackTime;

        rangedWeaponEquiped = true;
    }

    public void RangedWeaponNotEquipped()
    {
        rangedWeaponEquiped = false;
    }

    IEnumerator burstFireShooting()
    {
        for (int i = 0; i < burst; i++)
        {
            GameObject bullet = Instantiate(this.bulletGO, endPointPosition.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = aimDirection * 15.0f;
            //bullet.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);


            yield return new WaitForSeconds(fireRate);
        }
    }
}