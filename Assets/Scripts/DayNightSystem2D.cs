using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum DayCycles // Enum with day and night cycles
{
    Day,
    Night,
}

public class DayNightSystem2D : MonoBehaviour
{
    [Header("Controllers")]

    [Tooltip("Global light 2D component, we need to use this object to place light in all map objects")]
    public UnityEngine.Experimental.Rendering.Universal.Light2D globalLight; // global light

    [Tooltip("This is a current cycle time, you can change for private float but we keep public only for debug")]
    public float cycleCurrentTime = 0; // current cycle time

    [Tooltip("This is a cycle max time in seconds, if current time reach this value we change the state of the day and night cyles")]
    public float cycleMaxTime = 60; // duration of cycle

    [Tooltip("Enum with multiple day cycles to change over time, you can add more types and modify whatever you want to fits on your project")]
    public DayCycles dayCycle = DayCycles.Day; // default cycle

    [Header("Cycle Colors")]

    [Tooltip("(Mid) Day color, you can adjust based on best color for this cycle")]
    public Color day; // Eg: 10:00 at 16:00

    [Tooltip("Night color, you can adjust based on best color for this cycle")]
    public Color night; // Eg: 20:00 at 00:00

    public float dayIntensity;
    public float nightIntensity;

    public GameObject nightBullet;
    public GameObject[] plants;
    public GameObject enemies;

    [Header("Objects")]
    [Tooltip("Objects to turn on and off based on day night cycles, you can use this example for create some custom stuffs")]
    public Light2D[] mapLights; // enable/disable in day/night states
    private List<Color> mapLightsColors = new List<Color>();

    void Start() {
        dayCycle = DayCycles.Day; // start with sunrise state
        globalLight.color = day; // start global color at sunrise
        SFXManager.Instance.PlayAtmosphere();

        foreach (Light2D light in mapLights) {
            if (light.transform.parent.GetComponent<SpriteRenderer>() != null && light.gameObject.name == "LampLight") {
                Color colorToStore = light.transform.parent.GetComponent<SpriteRenderer>().color;
                mapLightsColors.Add(colorToStore);
            }
        }
        ControlLightMaps(false);
    }

    void Update() {
        // Update cycle time
        cycleCurrentTime += Time.deltaTime;

        if(cycleCurrentTime >= cycleMaxTime) {
            StartNight();
        }

        // Mid Day state
        if (dayCycle == DayCycles.Day) {
            ControlLightMaps(false);
            globalLight.color = day;
            globalLight.intensity = dayIntensity;

            PlayerController2D.Instance.GetComponent<PlayerAimWeapon>().isNight = false;

        }


        // Night state
        if (dayCycle == DayCycles.Night) {
            ControlLightMaps(true); // enable map lights (disable only in day states)
            globalLight.color = night;
            globalLight.intensity = nightIntensity;

            PlayerController2D.Instance.GetComponent<PlayerAimWeapon>().isNight = true;
            PlayerController2D.Instance.GetComponent<PlayerAimWeapon>().bulletGO = nightBullet;

            //enable enemies, disable plants
            foreach (GameObject plant in plants) {
                plant.gameObject.SetActive(false);
            }
            enemies?.gameObject.SetActive(true);
        }
    }

    void ControlLightMaps(bool status) {
        int i = 0;
        // loop in light array of objects to enable/disable
        if (mapLights.Length > 0)
            foreach (Light2D _light in mapLights) {
                _light.gameObject.SetActive(status);
                if (_light.gameObject.name == "LampLight") {
                    if (status) {
                        _light.transform.parent.GetComponent<SpriteRenderer>().color = mapLightsColors[i];
                    } else {
                        _light.transform.parent.GetComponent<SpriteRenderer>().color = Color.grey;
                    }
                    i++;
                }
            }
    }

    public void StartNight() {
        if (dayCycle == DayCycles.Day) {
            SFXManager.Instance.PlayAtmosphere();
        }
        cycleCurrentTime = 0; // back to 0 (restarting cycle time)
        dayCycle = DayCycles.Night; // change cycle states
    }
}