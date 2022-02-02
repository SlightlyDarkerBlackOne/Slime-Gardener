using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class RoseLadyHealthManager : MonoBehaviour, HealthManager
{

    public float MaxHealth;
    public float CurrentHealth;

    private bool flashActive;
    public float flashLength;
    private float flashCounter;

    private SpriteRenderer playerSprite;

    public event Action<float> OnCurrentHealthChange;

    void Start() {
        CurrentHealth = MaxHealth;
        playerSprite = transform.Find("Flower").GetComponent<SpriteRenderer>();
        OnCurrentHealthChange?.Invoke(CurrentHealth);
    }


    // Update is called once per frame
    void Update() {
        if (CurrentHealth <= 0) {
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
        SFXManager.Instance.PlaySound(SFXManager.Instance.roseDead);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        SetToMaxHealth();
    }

    public void Hurt(int damageToGive) {
        CurrentHealth -= damageToGive;
        OnCurrentHealthChange.Invoke(CurrentHealth);
        Debug.Log("Proso invoke");
        SFXManager.Instance.PlaySound(SFXManager.Instance.roseHurt);
        this.GetComponent<CameraShakePlayer>().ShakeOnHit();
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
