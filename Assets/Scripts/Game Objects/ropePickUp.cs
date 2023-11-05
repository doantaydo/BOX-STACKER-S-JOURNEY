using UnityEngine;

public class ropePickUp : movementObject
{
    public static ropePickUp instanceRope;

    // Start is called before the first frame update
    protected override void Awake()
    {
        if (instanceRope == null) instanceRope = this;
        speed = 0.3f;
        base.Awake();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (magnetPickUp.instanceMagnet.isPickUp) {
            currentPosition = transform.position;

            moveObject();

            if (currentPosition.y == 8.5f) nextPosition.y = 10;
        }
    }

    public override void pick() {
        nextPosition = transform.position;
        nextPosition.y = 8.5f;
    }
}
