using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingNumbers : MonoBehaviour
{

    public float moveSpeed;
    private int damageNumber;

    public TextMeshProUGUI displayNumber;

    // Update is called once per frame
    void Update() {
        displayNumber.text = "" + damageNumber;
        transform.position = new Vector3(transform.position.x, transform.position.y
            + (moveSpeed * Time.deltaTime), transform.position.z);
    }

    public void SetDamageNumber(int damage) {
        damageNumber = damage;
    }
}