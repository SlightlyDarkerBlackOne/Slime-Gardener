using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBucketWithWater : MonoBehaviour
{
    public GameObject bucket;
    public Sprite fullBucket;
    public Sprite emptyBucket;
    public Slider slider;

    public float bucketFillTime = 2f;

    public bool bucketFull;

    private Coroutine coroutine;
    private int i;

    private void Start() {
        slider.value = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (slider.value < 100) {
                bucketFull = false;
                PlayerController2D.Instance.GetComponent<PlayerAimWeapon>().canShoot = bucketFull;
                coroutine = StartCoroutine(FillBucket());
            }
        }
        Debug.Log("Enter");
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (slider.value < 100) {
                bucketFull = false;
                PlayerController2D.Instance.GetComponent<PlayerAimWeapon>().canShoot = bucketFull;
                slider.value = 0;
                bucket.GetComponent<SpriteRenderer>().sprite = emptyBucket;
                StopAllCoroutines();
                //StopCoroutine(coroutine);
                i = 0;
            }
            
        }
        Debug.Log("Exit");
    }

    private IEnumerator FillBucket() {
        Debug.Log("StartFilling");
        for (i = 1; i <= 10; i++) {
            slider.value = i * 10;
            yield return new WaitForSeconds(bucketFillTime/10);
            Debug.Log(i);
            if (slider.value == 100) {
                bucketFull = true;
                PlayerController2D.Instance.GetComponent<PlayerAimWeapon>().canShoot = bucketFull;
                PlayerController2D.Instance.GetComponent<PlayerAimWeapon>().bullets = 10;
                break;

            }
        }
        bucket.GetComponent<SpriteRenderer>().sprite = fullBucket;
    }

   public void EmptyBucket() {
        slider.value = 0;
        bucket.GetComponent<SpriteRenderer>().sprite = emptyBucket;
    }
}
