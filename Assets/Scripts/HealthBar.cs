using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour
{
    public RoseLadyHealthManager rlHM;
    public PlayerHealthManager pHM;
    // Start is called before the first frame update
    void Start()
    {
        if(rlHM != null)
            rlHM.OnCurrentHealthChange += UpdateSlider;
        else if(pHM != null)
            pHM.OnCurrentHealthChange += UpdateSlider;
    }

    // Update is called once per frame
    void UpdateSlider(float value)
    {
        GetComponent<Slider>().value = value;
    }
}
