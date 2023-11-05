using UnityEngine;

public class rotor_Roll : MonoBehaviour
{
    private float speed = 10f;
    private float currentRotationY;
    // Start is called before the first frame update
    void Start()
    {
        currentRotationY = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentRotationY += speed;
        if (currentRotationY >= 360) currentRotationY -= 360;

        transform.eulerAngles = new Vector3(0, currentRotationY, 0);
    }
}
