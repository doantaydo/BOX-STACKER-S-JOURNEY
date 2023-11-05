using UnityEngine;

public abstract class movementObject : MonoBehaviour
{
    protected float speed = 0.6f;
    protected Vector3 currentPosition, nextPosition;

    protected virtual void Awake() {
        currentPosition = transform.position;
        nextPosition = transform.position;
    }

    public abstract void pick();

    public float getPositionX() => currentPosition.x;
    public float getPositionY() => currentPosition.y;
    public float getPositionZ() => currentPosition.z;

    public void moveToPosition(float X, float Y, float Z) {
        nextPosition.x = X;
        nextPosition.y = Y;
        nextPosition.z = Z;
    }

    protected void moveObject() {
        if (currentPosition.x < nextPosition.x) currentPosition.x = Mathf.Round((currentPosition.x + speed) * 100) / 100;
        if (currentPosition.x > nextPosition.x) currentPosition.x = Mathf.Round((currentPosition.x - speed) * 100) / 100;

        if (currentPosition.y < nextPosition.y) currentPosition.y = Mathf.Round((currentPosition.y + speed) * 100) / 100;
        if (currentPosition.y > nextPosition.y) currentPosition.y = Mathf.Round((currentPosition.y - speed) * 100) / 100;

        if (currentPosition.z < nextPosition.z) currentPosition.z = Mathf.Round((currentPosition.z + speed) * 100) / 100;
        if (currentPosition.z > nextPosition.z) currentPosition.z = Mathf.Round((currentPosition.z - speed) * 100) / 100;

        transform.position = currentPosition;
    }
}
