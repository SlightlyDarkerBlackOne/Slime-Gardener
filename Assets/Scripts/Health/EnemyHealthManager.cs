using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour, HealthManager
{

    public int MaxHealth;
    public int CurrentHealth;
    public event EventHandler OnEnemyDeath;

    // Use this for initialization
    void Start() {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update() {
        if (CurrentHealth <= 0) {
            OnEnemyDeath?.Invoke(this, EventArgs.Empty);
            gameObject.SetActive(false);
            Destroy(gameObject);
            SFXManager.Instance.PlaySound(SFXManager.Instance.enemyDead);
        }
    }

    public void Hurt(int damageToGive) {
        CurrentHealth -= damageToGive;

        SFXManager.Instance.PlaySound(SFXManager.Instance.enemyHit);
    }

    public void SetMaxHealth() {
        CurrentHealth = MaxHealth;
    }
}