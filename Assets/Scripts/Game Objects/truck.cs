using UnityEngine;
using UnityEngine.UI;

public class truck : movementObject
{
    public static truck instanceTruck;
    public bool isWaitingOrder;

    int timer;

    public Image clock_circle;
    private Color[] colorSet;


    protected override void Awake() {
        if (instanceTruck == null) instanceTruck = this;
        isWaitingOrder = false;
        timer = -1;

        colorSet = new Color[3];
        colorSet[0] = new Color(0.0f, 1.0f, 0.0f);
        colorSet[1] = new Color(1.0f, 1.0f, 0.0f);
        colorSet[2] = new Color(1.0f, 0.0f, 0.0f);
        // speed = 1f;
        base.Awake();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (getPositionX() == 120 && GameManager.instanceGameManager.isPlaying) MoveToWareHouse();

        if (isWaitingOrder == false && getPositionX() <= 43) {
            isWaitingOrder = true;
            timer = 6000;
        }

        if (timer == 0) {
            MatrixBoard.instanceMatrixBoard.updateNextBox();
            MoveOutWarHouse();
        }
        if (timer > 0) {
            clock_circle.fillAmount = timer * 1.0f /6000.0f;
            if (timer >= 3000) clock_circle.color = colorSet[0];
            else if (timer >= 1500) clock_circle.color = colorSet[1];
            else clock_circle.color = colorSet[2];
        }

        timer--;

        moveObject();
        transform.eulerAngles = new Vector3(0, 90, 0);
    }

    public void MoveToWareHouse() {
        moveToPosition(43,-2.5f,0);
    }

    public void MoveOutWarHouse() {
        moveToPosition(120,-2.5f,0);
        isWaitingOrder = false;
        timer = -1;
    }
    public void resetPosition() {
        currentPosition.x = 120;
        currentPosition.y = -2.5f;
        currentPosition.z = 0;
        nextPosition.x = 120;
        nextPosition.y = -2.5f;
        nextPosition.z = 0;
        transform.position = currentPosition;
    }

    public override void pick() {}
}
