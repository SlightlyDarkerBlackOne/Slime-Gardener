using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class CameraShakePlayer : MonoBehaviour
{
    public ShakeData shakeData;
    
    public void ShakeOnHit() {
        CameraShakerHandler.Shake(shakeData);
    }
}
