using UnityEngine;

public class magnetPickUp : movementObject
{
    public static magnetPickUp instanceMagnet;
    public bool isPickUp;
    // Start is called before the first frame update
    protected override void Awake()
    {
        if (instanceMagnet == null) instanceMagnet = this;
        isPickUp = false;
        // speed = 0.3f;
        base.Awake();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPickUp) {
            currentPosition = transform.position;

            moveObject();

            if (currentPosition.y == 9) {
                // Debug.Log("Stop Pick!");
                isPickUp = false;
            }

            if (currentPosition.y == 6) {
                nextPosition.y = 9;
                if (GameManager.instanceGameManager.boxHolding != null) {
                    GameManager.instanceGameManager.boxHolding.GetComponent<Box>().pick();
                }
            }
        }
        
    }

    public override void pick() {
        // Debug.Log("Magnet Pick!");
        isPickUp = true;
        nextPosition = transform.position;
        nextPosition.y = 6;
    }
}
