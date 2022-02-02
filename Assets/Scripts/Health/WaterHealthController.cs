using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class WaterHealthController : MonoBehaviour, HealthManager { 

    public float MaxHealth;
    public float CurrentHealth;

    private bool flashActive;
    public float flashLength;
    private float flashCounter;

    public Sprite aliveSprite;
    private SpriteRenderer playerSprite;

    public event Action<float> OnCurrentHealthChange;

    private bool isAlive = true;
    void Start() {
        CurrentHealth = MaxHealth;
        playerSprite = GetComponent<SpriteRenderer>();
        OnCurrentHealthChange?.Invoke(CurrentHealth);
    }


    // Update is called once per frame
    void Update() {
        if (CurrentHealth <= 0 && isAlive) {
            Dead();
        }

        Flash();
    }

    //Flashing the player sprite with white when he takes damage
    private void Flash() {
        if (flashActive) {
            if (flashCounter > flashLength * 0.66f) {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            } else if (flashCounter > flashLength * 0.33f) {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            } else if (flashCounter > 0) {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            } else {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
                flashActive = false;
            }

            flashCounter -= Time.deltaTime;
        }
    }
    private void Dead() {
        SFXManager.Instance.PlaySound(SFXManager.Instance.alivePlant);
        //particles

        //change sprite
        playerSprite.sprite = aliveSprite;
        Destroy(GetComponent<BoxCollider2D>());
        isAlive = false;
    }

    public void Hurt(int damageToGive) {
        CurrentHealth -= damageToGive;
        //OnCurrentHealthChange.Invoke(CurrentHealth);
        Debug.Log("Proso invoke");
        SFXManager.Instance.PlaySound(SFXManager.Instance.waterSplash);
        if (GetComponent<CameraShakePlayer>() != null) {
            this.GetComponent<CameraShakePlayer>().ShakeOnHit();
        }
        
        flashActive = true;
        flashCounter = flashLength;
    }

    public void SetToMaxHealth() {
        CurrentHealth = MaxHealth;
        OnCurrentHealthChange?.Invoke(CurrentHealth);
    }

    public void IncreaseMaxHealth(float newMaxHealth) {
        MaxHealth = newMaxHealth;

        SetToMaxHealth();
    }

    public void Heal(float healAmount) {
        CurrentHealth += healAmount;
        if (CurrentHealth >= MaxHealth) {
            CurrentHealth = MaxHealth;
        }
        OnCurrentHealthChange?.Invoke(CurrentHealth);
        //SFXManager.Instance.PlaySound(SFXManager.Instance.playerHealed);
    }
}
