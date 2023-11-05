using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Down_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isHolding = false;

    private int holdTime = 0;
    private int requiredHoldTime = 30;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        holdTime = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        holdTime = 0;
    }

    void FixedUpdate()
    {
        if (isHolding)
        {
            if (holdTime == 0) {
                GameManager.instanceGameManager.moveVer(-1);
                holdTime = requiredHoldTime;
            }
            else holdTime -= 1;
        }
    }
}
