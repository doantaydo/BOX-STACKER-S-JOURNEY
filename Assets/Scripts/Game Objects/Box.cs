using UnityEngine;


public class Box : movementObject
{
    public int boxColor = 0;

    void FixedUpdate()
    {
        moveObject();

        if (getPositionX() > 100) Destroy(this);
    }

    public override void pick() {
        moveToPosition(getPositionX(), 5.62f, getPositionZ());
    }

    public void drop() {
        moveToPosition(getPositionX(), 2.62f, getPositionZ());
    }

    public void teleport(float X, float Y, float Z) {
        Vector3 newPosition = transform.position;
        newPosition.x = X;
        newPosition.y = Y;
        newPosition.z = Z;
        transform.position = newPosition;
    }

}
