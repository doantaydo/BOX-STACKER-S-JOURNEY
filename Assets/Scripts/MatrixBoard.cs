using UnityEngine;
using UnityEngine.UI;

public class MatrixBoard : MonoBehaviour
{
    public static MatrixBoard instanceMatrixBoard;

    public Image currentOutputImage, nextOutputImage;

    private int[,] colorMatrix;
    private int[] inputBox, outputBox;
    private int nextBoxInput, nextBoxOutput;

    private GameObject[,] boxMatrix;
    private GameObject[] queueInputBoxes;

    private int timeToCreateNewBox, countTimeCreateNewBox;

    public GameObject[] prefabsBoxes;

    private Color[] colorCode;
    public GameObject outputCombineMachineArrowBlue, outputCombineMachineArrowRed;
    
    void Awake() {
        if (instanceMatrixBoard == null) instanceMatrixBoard = this;

        colorMatrix = new int[9,9];
        boxMatrix = new GameObject[9,9];

        inputBox = null;
        outputBox = null;

        queueInputBoxes = new GameObject[12]; // -24; -30; -36; -42; -48; -54; -60; -66; -72; -78; -84; -90
        nextBoxInput = 0;
        nextBoxOutput = 0;

        timeToCreateNewBox = -1;

        colorCode = new Color[8];
        colorCode[0] = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        colorCode[1] = new Color(184.0f/255.0f, 10.0f/255.0f, 10.0f/255.0f);
        colorCode[2] = new Color(236.0f/255.0f, 222.0f/255.0f, 0.0f);
        colorCode[3] = new Color(0.0f, 0.0f, 179.0f/255.0f);
        colorCode[4] = new Color(246.0f/255.0f, 142.0f/255.0f, 86.0f/255.0f);
        colorCode[5] = new Color(124.0f/255.0f, 0.0f, 181.0f/255.0f);
        colorCode[6] = new Color(44.0f/255.0f, 148.0f/255.0f, 0.0f);
        colorCode[7] = new Color(1.0f, 1.0f, 1.0f);

        outputCombineMachineArrowBlue.SetActive(false);
        outputCombineMachineArrowRed.SetActive(false);

        // startTutorial();
    }

    void FixedUpdate() {
        
        if (GameManager.instanceGameManager.isPlaying) {
            checkInputBoxIntoBoard();
            checkOutputBoxOutTheBoard();
            checkCombineMachine();
        }
    }


    void checkInputBoxIntoBoard() {
        // UPDATE INPUT BOX QUEUE
        if (countTimeCreateNewBox == 0 && nextBoxInput < inputBox.Length) {
            for (int i = 0; i < 12; i++) {
                if (queueInputBoxes[i] == null) {
                    countTimeCreateNewBox = timeToCreateNewBox;
                    queueInputBoxes[i] = createBox(inputBox[nextBoxInput++]);
                    break;
                }
            }
        }
        if (nextBoxInput < inputBox.Length && countTimeCreateNewBox > 0) countTimeCreateNewBox--;
        PutBoxQueue();
    }

    void PutBoxQueue() {
        if (colorMatrix[0,4] == 0 && queueInputBoxes[0] != null) {
            putBoxAt(queueInputBoxes[0], -24, 0);
            queueInputBoxes[0] = null;

            for (int i = 1; i < 12; i++) {
                if (queueInputBoxes[i-1] == null) {
                    queueInputBoxes[i-1] = queueInputBoxes[i];
                    queueInputBoxes[i] = null;
                }
            }
        }

        for (int i = 0; i < 12; i++) {
            if (queueInputBoxes[i] != null) moveBoxToPositionX(queueInputBoxes[i], (-30 - 6*i));
        }
    }

    void checkOutputBoxOutTheBoard() {
        // UPDATE OUTPUT BOX QUEUE
        if (outputBox != null) {
            if (nextBoxOutput < outputBox.Length && colorMatrix[8,4] != 0 && truck.instanceTruck.isWaitingOrder) {
                if (boxMatrix[8,4].GetComponent<Box>().getPositionY() == 2.62f) {
                    if (colorMatrix[8,4] == outputBox[nextBoxOutput]) scoreManager.instanceScoreManager.changeScore(10);

                    Destroy(boxMatrix[8,4]);
                    colorMatrix[8,4] = 0;
                    boxMatrix[8,4] = null;
                    nextBoxOutput++; 
                    truck.instanceTruck.MoveOutWarHouse();    
                }
            }
            if (nextBoxOutput == outputBox.Length) {
                GameManager.instanceGameManager.endLevel();
            }
        }
        //UPDATE OUTPUT BOARD
        updateOutPutOrderBoard();
    }

    public void updateNextBox() {
        nextBoxOutput++;

        //UPDATE OUTPUT BOARD
        updateOutPutOrderBoard();
    }

    void updateOutPutOrderBoard() {
        if (outputBox == null) currentOutputImage.color = colorCode[0];
        else if (nextBoxOutput >= outputBox.Length) currentOutputImage.color = colorCode[0];
        else currentOutputImage.color = colorCode[outputBox[nextBoxOutput]];
    }

    private int newBoxType;
    
    void checkCombineMachine() {
        // PUT MATERIALS INTO MACHINE
        if (colorMatrix[2,5] != 0 && colorMatrix[4,5] != 0 && colorMatrix[2,5] != colorMatrix[4,5]) {
            if (boxMatrix[2,5].GetComponent<Box>().getPositionY() == 2.62f  && boxMatrix[4,5].GetComponent<Box>().getPositionY() == 2.62f) {
                if (colorMatrix[6,7] != 0) {
                    outputCombineMachineArrowBlue.SetActive(false);
                    outputCombineMachineArrowRed.SetActive(true);
                }
                else {
                    newBoxType = colorMatrix[2,5] + colorMatrix[4,5] + 1;

                    moveBox(getTruePosition(2), getTruePosition(5), getTruePosition(2), getTruePosition(7));
                    moveBox(getTruePosition(4), getTruePosition(5), getTruePosition(4), getTruePosition(7));
                }
            }
        }

        if (colorMatrix[6,7] == 0) {
            outputCombineMachineArrowBlue.SetActive(false);
            outputCombineMachineArrowRed.SetActive(false);
        }
        else if (colorMatrix[2,5] == 0 || colorMatrix[4,5] == 0) {
            if (colorMatrix[6,7] == 4 || colorMatrix[6,7] == 5 || colorMatrix[6,7] == 6) {
                outputCombineMachineArrowBlue.SetActive(true);
                outputCombineMachineArrowRed.SetActive(false);
            }
            else {
                outputCombineMachineArrowBlue.SetActive(false);
                outputCombineMachineArrowRed.SetActive(false);
            }
        }

        if (colorMatrix[2,7] != -1) {
            if (boxMatrix[2,7].GetComponent<Box>().getPositionZ() == getTruePosition(7) && boxMatrix[4,7].GetComponent<Box>().getPositionZ() == getTruePosition(7)) {
                Destroy(boxMatrix[2,7]);
                boxMatrix[2,7] = null;
                colorMatrix[2,7] = -1;

                Destroy(boxMatrix[4,7]);
                boxMatrix[4,7] = null;
                colorMatrix[4,7] = -1;

                if (newBoxType != -1) {
                    outputCombineMachineArrowBlue.SetActive(true);
                    outputCombineMachineArrowRed.SetActive(false);
                    GameObject newCombinedBox = createBox(newBoxType);
                    putBoxAt(newCombinedBox, getTruePosition(6), getTruePosition(7));
                    newBoxType = -1;
                }
            }
        }
    }

    void moveBoxToPositionX(GameObject box, int X) {
        Box boxScript = box.GetComponent<Box>();
        boxScript.moveToPosition(X, boxScript.getPositionY(), boxScript.getPositionZ());
    }

    public void startTutorial() {
        inputBox = new int[] {1, 2, 3, 1, 2, 3, 3, 2, 1, 2, 3, 2, 1, 3, 2, 2, 1, 2, 3, 1, 3, 2, 3, 1, 2, 3, 1, 3, 2, 1, 2, 3, 2, 1, 3, 2, 2, 1, 2, 3, 1, 3, 2, 1, 3, 2, 2, 1, 2, 3, 3, 1, 2, 3, 1, 2, 3, 3, 2, 1, 2, 3, 2};
        // inputBox = new int[] {1, 2, 3, 1, 2, 3, 3, 2, 1, 1, 3, 1, 3};
        nextBoxInput = 0;
        // outputBox = new int[] {1, 3, 2, 4, 6, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5};
        outputBox = new int[] {1, 3, 2, 4, 6, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        nextBoxOutput = 0;
        timeToCreateNewBox = 500;
        countTimeCreateNewBox = 0;
        initMatrix();
    }

    public void startLevel1() {
        inputBox = new int[] {3, 2, 1, 2, 1, 1, 1, 3, 3, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        nextBoxInput = 0;
        outputBox = new int[] {1, 3, 2, 4, 5, 5, 2, 6, 1, 4};
        nextBoxOutput = 0;
        timeToCreateNewBox = 500;
        countTimeCreateNewBox = 0;
        initMatrix();
    }

    public void startLevel2() {
        inputBox = new int[] {2, 3, 3, 2, 1, 3, 1, 1, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        nextBoxInput = 0;
        outputBox = new int[] {6, 2, 5, 1, 1, 2, 6, 4, 3, 4};
        nextBoxOutput = 0;
        timeToCreateNewBox = 500;
        countTimeCreateNewBox = 0;
        initMatrix();
    }

    void initMatrix() {
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                colorMatrix[i, j] = (j == 8 || j == 0) ? -1 : 0;
                if (boxMatrix[i, j] != null) {
                    Destroy(boxMatrix[i, j]);
                }
                boxMatrix[i, j] = null;
            }
        }

        initDropPosition();

        for (int i = 0; i < 12; i++) {
            if (queueInputBoxes[i] != null) Destroy(queueInputBoxes[i]);
        }

        DronePlayer.instanceDrone.resetPosition();
        truck.instanceTruck.resetPosition();

        for (int i = 0; i < outputBox.Length; i++) {
            if (outputBox[i] == -1) outputBox[i] = (int)Random.Range(1, 7);
        }
        // scoreManager.instanceScoreManager.resetScore();
    }

    void initDropPosition() {
        colorMatrix[0, 1] = -1;
        colorMatrix[2, 1] = -1;
        colorMatrix[3, 1] = -1;
        colorMatrix[4, 1] = -1;
        colorMatrix[7, 1] = -1;

        colorMatrix[6, 2] = -1;

        colorMatrix[0, 6] = -1;
        colorMatrix[1, 6] = -1;
        colorMatrix[2, 6] = -1;
        colorMatrix[3, 6] = -1;
        colorMatrix[4, 6] = -1;

        colorMatrix[0, 7] = -1;
        colorMatrix[1, 7] = -1;
        colorMatrix[2, 7] = -1;
        colorMatrix[3, 7] = -1;
        colorMatrix[4, 7] = -1;
        colorMatrix[5, 7] = -1;
        colorMatrix[8, 7] = -1;
    }

    public void putBoxAt(GameObject box, float trueX, float trueZ) {
        int X = getBoardPosition(trueX);
        int Z = getBoardPosition(trueZ);

        boxMatrix[X, Z] = box;
        if (box == null) colorMatrix[X, Z] = 0;
        else {
            Box boxScript = boxMatrix[X, Z].GetComponent<Box>();
            boxScript.moveToPosition(trueX, boxScript.getPositionY(), trueZ);
            colorMatrix[X, Z] = boxScript.boxColor;
        }
    }

    public void moveBox(float fromX, float fromZ, float toX, float toZ) {
        putBoxAt(boxMatrix[getBoardPosition(fromX), getBoardPosition(fromZ)], toX, toZ);
        putBoxAt(null, fromX, fromZ);
    }

    public void pickBox(float trueX, float trueZ) {
        int X = getBoardPosition(trueX);
        int Z = getBoardPosition(trueZ);

        Box boxScript = boxMatrix[X, Z].GetComponent<Box>();
        boxScript.pick();
    }

    public void dropBox(float trueX, float trueZ) {
        int X = getBoardPosition(trueX);
        int Z = getBoardPosition(trueZ);

        Box boxScript = boxMatrix[X, Z].GetComponent<Box>();
        boxScript.drop();
    }

    public int getBoxColorAt(float trueX, float trueZ) => colorMatrix[getBoardPosition(trueX), getBoardPosition(trueZ)];
    public GameObject getBoxObject(float trueX, float trueZ) => boxMatrix[getBoardPosition(trueX), getBoardPosition(trueZ)];

    private GameObject createBox(int type) {
        if (type == -1) {
            int randomNumber = (int)Random.Range(1, 4);
            return Instantiate(prefabsBoxes[randomNumber]);
        }
        else return Instantiate(prefabsBoxes[type]);
    }

    private float getTruePosition(int boardPosition) => (6 * boardPosition - 24);
    private int getBoardPosition(float truePosition) => (int)((truePosition + 24) / 6);

    public void printBoard() {
        for (int i = 0; i < 9; i++) {
            Debug.Log(colorMatrix[i,0] + " " + colorMatrix[i,1] + " " + colorMatrix[i,2] + " " + colorMatrix[i,3] + " " + colorMatrix[i,4] + " " + colorMatrix[i,5] + " " + colorMatrix[i,6]+ " " + colorMatrix[i,7]+ " " + colorMatrix[i,8]);
        }
        Debug.Log("End Print");
    }
}
