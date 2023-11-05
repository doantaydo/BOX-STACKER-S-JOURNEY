using UnityEngine;

public class DronePlayer : movementObject
{
    public static DronePlayer instanceDrone;
    public bool isMoving;

    protected override void Awake() {
        if (instanceDrone == null) instanceDrone = this;
        isMoving = false;
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // need to test this :))
        isMoving = (currentPosition.x != nextPosition.x) || (currentPosition.y != nextPosition.y) || (currentPosition.z != nextPosition.z);

        moveObject();
    }

    public void resetPosition() {
        currentPosition.x = 0;
        currentPosition.y = 0;
        currentPosition.z = 0;
        nextPosition.x = 0;
        nextPosition.y = 0;
        nextPosition.z = 0;
        transform.position = currentPosition;
    }

    public override void pick() {
        // Debug.Log("Drone Pick!");
        ropePickUp.instanceRope.pick();
        magnetPickUp.instanceMagnet.pick();
    }
}
