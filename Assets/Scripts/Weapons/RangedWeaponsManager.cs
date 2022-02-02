using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponsManager : MonoBehaviour
{
    public List<GameObject> listOfRangedWeapons;

    private GameObject equipped;
    private int numberOfArrowsToShoot;

    private void Start() {
        equipped = listOfRangedWeapons[0];
    }
}
