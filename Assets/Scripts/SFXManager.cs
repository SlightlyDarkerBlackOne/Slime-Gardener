using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource playerDead;
    public AudioSource roseDead;
    public AudioSource playerHurt;
    public AudioSource roseHurt;

    public AudioSource waterSplash;

    public AudioSource alivePlant;

    public AudioSource itemDropped;
    public AudioSource itemPickedUp;

    public AudioSource enemyDead;
    public AudioSource enemyHit;

    public AudioSource projectileFire;
    public AudioSource glassShattering;

    public AudioSource winGame;
    public AudioSource loseGame;

    public AudioSource dash;

    public AudioSource DayAtmosphere;
    public AudioSource NightAtmosphere;

    public AudioSource[] footsteps;
    public AudioSource[] uiButtons;


    #region Singleton
    public static SFXManager Instance { get; private set; }

    // Use this for initialization
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start() {
        PlaySoundTrack(DayAtmosphere);
    }
    public void PlaySound(AudioSource source) {
        source.Play();
    }
    public void StopSound(AudioSource source) {
        source.Stop();
    }
    private void PlayOnLoop(AudioSource source) {
        source.Play();
        source.loop = true;
    }

    public void PlaySoundTrack(AudioSource source) {
        //soundTrack.Stop();
        PlayOnLoop(source);
    }
    public void PlayAtmosphere() {
        if (DayAtmosphere.isPlaying) {
            PlayOnLoop(NightAtmosphere);
            DayAtmosphere.Stop();
        } else if (NightAtmosphere.isPlaying) {
            PlayOnLoop(DayAtmosphere);
            NightAtmosphere.Stop();
        }
    }

    public void PlayRandomFootstep() {
        int random = Random.Range(0, footsteps.Length);
        PlaySound(footsteps[random]);
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(0.1f);
    }
}