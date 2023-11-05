using UnityEngine;

public class GameManager : MonoBehaviour
{
    int currentCamera, oldCamera;
    int currentRotationYCamera, count_rotation;
    public GameObject[] cameras;
    public GameObject[] wallCorners;
    public GameObject wall_Left, wall_Right, wall_Top, wall_Down;
    bool isChangeCamera;
    int speedChangeCamera = 15;
    public GameObject boxHolding;
    public bool isPlaying;
    private int levelPlaying;

    public GameObject levelButtons, completeLevelPopup;
    public GameObject[] controlScheme;
    public GameObject[] moveCameraButtons;

    private bool isControlScheme;

    public static GameManager instanceGameManager;
    // Start is called before the first frame update
    void Awake()
    {
        oldCamera = 0;
        currentCamera = 0;
        currentRotationYCamera = 0;
        count_rotation = 0;
        isChangeCamera = false;
        boxHolding = null;
        isPlaying = true;
        levelPlaying = 0;

        if (instanceGameManager == null) instanceGameManager = this;
        isControlScheme = false;
        changeController();
        // DroneIsHolding = false;
    }

    void Start() {
        playTutorial();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isChangeCamera) {
            currentRotationYCamera += speedChangeCamera;
            if (currentRotationYCamera <= -360) currentRotationYCamera += 360;
            if (currentRotationYCamera >= 360) currentRotationYCamera -= 360;

            cameras[0].transform.eulerAngles = new Vector3(0, currentRotationYCamera, 0);
            count_rotation += 1;

            if (count_rotation == 18) {
                isChangeCamera = false;
            }
            else if (count_rotation == 9) {
                setActiveChangeCamera(oldCamera, false);
                setActiveChangeCamera(currentCamera, true);
            }
        }
    }

    public void endLevel() {
        isPlaying = false;
        // levelButtons.SetActive(true);
        completeLevelPopup.SetActive(true);
    }

    public void playTutorial() {
        isPlaying = true;
        levelPlaying = 0;
        MatrixBoard.instanceMatrixBoard.startTutorial();
        levelButtons.SetActive(true);
        completeLevelPopup.SetActive(false);
        scoreManager.instanceScoreManager.startScore();
        if (!isControlScheme) changeController();
    }

    public void playLevel1() {
        isPlaying = true;
        levelPlaying = 1;
        MatrixBoard.instanceMatrixBoard.startLevel1();
        levelButtons.SetActive(false);
        completeLevelPopup.SetActive(false);
        scoreManager.instanceScoreManager.startScore();
        if (!isControlScheme) changeController();
    }

    public void playLevel2() {
        isPlaying = true;
        levelPlaying = 2;
        MatrixBoard.instanceMatrixBoard.startLevel2();
        levelButtons.SetActive(false);
        completeLevelPopup.SetActive(false);
        if (!isControlScheme) changeController();
    }

    public void replay() {
        if (levelPlaying == 1) playLevel1();
        else if (levelPlaying == 2) playLevel2();
        else if (levelPlaying == 0) playTutorial();
        scoreManager.instanceScoreManager.startScore();
    }

//======================================================//
//++++++++++++++++  MOVEMENT CHARACTER  ++++++++++++++++//
//======================================================//
    public void moveVer(int ver) {
        float newX = DronePlayer.instanceDrone.getPositionX();
        float newZ = DronePlayer.instanceDrone.getPositionZ();

        if (currentCamera == 0) newX += (ver * 6);
        else if (currentCamera == 1) newZ -= (ver * 6);
        else if (currentCamera == 2) newX -= (ver * 6);
        else newZ += (ver * 6);

        movingAllObject(newX, newZ);
    }

    public void moveHor(int hor) {
        float newX = DronePlayer.instanceDrone.getPositionX();
        float newZ = DronePlayer.instanceDrone.getPositionZ();

        if (currentCamera == 0) newZ += (hor * 6);
        else if (currentCamera == 1) newX += (hor * 6);
        else if (currentCamera == 2) newZ -= (hor * 6);
        else newX -= (hor * 6);

        movingAllObject(newX, newZ);
    }

    void movingAllObject(float newX, float newZ) {
        if (isChangeCamera) {
            Debug.Log("IsChangeCamera, Cannot Move!");
            return;
        }
        if (magnetPickUp.instanceMagnet.isPickUp) {
            Debug.Log("IsPickUp, Cannot Move!");
            return;
        }
        if ((newX >  24) || (newX < -24) || (newZ > 24) || (newZ < -24)) {
            Debug.Log("Out of Warehouse, Can not move!!");
            return;
        }
        if (DronePlayer.instanceDrone.isMoving) {
            Debug.Log("IsMoving, Please Wait!");
            return;
        }

        if (boxHolding != null) {
            if (MatrixBoard.instanceMatrixBoard.getBoxColorAt(newX, newZ) != 0) {
                Debug.Log("Get other box Can not move!");
                return;
            }
            else {
                MatrixBoard.instanceMatrixBoard.moveBox(DronePlayer.instanceDrone.getPositionX(), DronePlayer.instanceDrone.getPositionZ(), newX, newZ);
            }
        }

        DronePlayer.instanceDrone.moveToPosition(newX, DronePlayer.instanceDrone.getPositionY(), newZ);
    }
//=========================================+=============//
//+++++++++++++++++++  CHANGE CAMERA  +++++++++++++++++++//
//==================================================+====//
    public void changeCamera(int changeWhere) {
        if (isChangeCamera == false) {
            isChangeCamera = true;
            speedChangeCamera = changeWhere * 5;
            oldCamera = currentCamera;
            count_rotation = 0;

            currentCamera = currentCamera + changeWhere;
            if (currentCamera == 4) currentCamera = 0;
            if (currentCamera == -1) currentCamera = 3;
        }
    }

    void setActiveChangeCamera(int cameraNumber, bool turnON) {
        wallCorners[cameraNumber].SetActive(turnON);

        if (cameraNumber == 0) {
            wall_Left.SetActive(turnON);
            wall_Top.SetActive(turnON);
        }
        else if (cameraNumber == 1) {
            wall_Right.SetActive(turnON);
            wall_Top.SetActive(turnON);
        }
        else if (cameraNumber == 2) {
            wall_Down.SetActive(turnON);
            wall_Right.SetActive(turnON);
        }
        else {
            wall_Left.SetActive(turnON);
            wall_Down.SetActive(turnON);
        }
    }
    public void changeController() {
        isControlScheme = !isControlScheme;
        for (int i = 0; i < controlScheme.Length; i++)
            controlScheme[i].SetActive(isControlScheme);
        for (int i = 0; i < moveCameraButtons.Length; i++)
            moveCameraButtons[i].SetActive(!isControlScheme);
    }
//=======================================================//
//++++++++++++++++++  PICK BOX ACTION  ++++++++++++++++++//
//=======================================================//
    public void pick() {
        if (isChangeCamera) {
            Debug.Log("IsChangeCamera, Cannot Move!");
            return;
        }
        if (DronePlayer.instanceDrone.isMoving) {
            Debug.Log("IsMoving, Cannot Pick!");
            return;
        }

        DronePlayer.instanceDrone.pick();
        if (boxHolding == null) {
            boxHolding = MatrixBoard.instanceMatrixBoard.getBoxObject(DronePlayer.instanceDrone.getPositionX(), DronePlayer.instanceDrone.getPositionZ());
            // if (boxHolding != null) boxHolding.GetComponent<Box>().pick();
        }
        else {
            boxHolding.GetComponent<Box>().drop();
            boxHolding = null;
        }
    }
}
