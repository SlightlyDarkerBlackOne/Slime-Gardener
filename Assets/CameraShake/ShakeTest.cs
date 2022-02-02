using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class ShakeTest : MonoBehaviour
{
    public ShakeData shakeData;

    private void Start() {
        CameraShakerHandler.Shake(shakeData);
    }
}
