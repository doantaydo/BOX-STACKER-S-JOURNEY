using UnityEngine;

public class BoxesPool : MonoBehaviour
{
    public static BoxesPool instanceBoxesPool;
    public GameObject boxRed, boxYellow, boxBlue;

    private GameObject[] redPool, yellowPool, bluePool;
    void Awake() {
        if (instanceBoxesPool == null) instanceBoxesPool = this;
        // initPool();
    }
    void Start() {
        // initPool();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public GameObject getBox(int type) {
        // 1: red
        // 2: yellow
        // 3: blue
        GameObject returnObject = null;
        for (int i = 0; i < 30; i++) {
            if (type == 1) returnObject = redPool[i];
            if (type == 2) returnObject = yellowPool[i];
            if (type == 3) returnObject = bluePool[i];
        }
        return null;
    }

    void initPool() {
        redPool = new GameObject[30];
        yellowPool = new GameObject[30];
        bluePool = new GameObject[30];

        for (int i = 0; i < 30; i++) {
            redPool[i] = Instantiate(boxRed, transform);
            redPool[i].SetActive(false);

            yellowPool[i] = Instantiate(boxRed, transform);
            yellowPool[i].SetActive(false);

            bluePool[i] = Instantiate(boxRed, transform);
            bluePool[i].SetActive(false);
        }
    }

    public void resetPool() {
        for (int i = 0; i < 30; i++) {
            redPool[i].transform.position = transform.position;
            redPool[i].SetActive(false);

            yellowPool[i].transform.position = transform.position;
            yellowPool[i].SetActive(false);

            bluePool[i].transform.position = transform.position;
            bluePool[i].SetActive(false);
        }
    }
}
