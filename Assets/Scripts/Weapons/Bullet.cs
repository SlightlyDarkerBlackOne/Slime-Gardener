using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;

    private void OnEnable() {
        Invoke("Destroy", 4f);
    }
    private void Start() {
        moveSpeed = 5f;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void SetMoveDirection(Vector2 dir) {
        moveDirection = dir;
    }
    private void Destroy() {
        gameObject.SetActive(false);
    }
    private void OnDisable() {
        CancelInvoke();
    }
}
