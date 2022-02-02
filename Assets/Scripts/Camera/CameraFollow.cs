using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform cameraTarget;

    public float cameraSpeed;

    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    //Defines an instance of the weather effect and changes its position in the FixedUpdate function the same as the camera
    private void Start() {
        cameraTarget = PlayerController2D.Instance.gameObject.transform;
        //instantiatedEffect = Instantiate(weatherEffect, transform.position, Quaternion.identity);
    }

    private void FixedUpdate() {
        if (cameraTarget != null) {
            var newPos = Vector2.Lerp(transform.position, cameraTarget.position,
                Time.deltaTime * cameraSpeed);

            var vect3 = new Vector3(newPos.x, newPos.y, -10f);

            var clampX = Mathf.Clamp(vect3.x, minX, maxX);
            var clampY = Mathf.Clamp(vect3.y, minY, maxY);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }

    /*Changes level borders for the main camera
    Checks if the new values are 0 so we can change only maxY position of the 
    camera on a single level to simplify entering into houses*/
    public void ChangeLevelBorders(float minxNew, float minyNew, float maxxNew, float maxyNew) {
        if (minxNew != 0)
            minX = minxNew;
        if (minyNew != 0)
            minY = minyNew;

        if (maxxNew != 0)
            maxX = maxxNew;
        if (maxyNew != 0)
            maxY = maxyNew;
    }
}
