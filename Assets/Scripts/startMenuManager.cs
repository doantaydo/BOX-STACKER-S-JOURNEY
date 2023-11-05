using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startMenuManager : MonoBehaviour
{
    public GameObject LoadingScene, PopupButton;
    bool isLoading = false;

    public Image progress_bar;
    public GameObject drone;

    float percent = 0.0f;
    float speed_increase = 0.004f;

    Vector3 currentDronePosition;

    void Awake() {
        LoadingScene.SetActive(false);
        PopupButton.SetActive(true);

        currentDronePosition = drone.transform.position;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLoading) {
            percent += speed_increase;

            if (percent >= 1.0f) {
                progress_bar.fillAmount = 1;
                currentDronePosition.x = 6.66f;
                drone.transform.position = currentDronePosition;
            }
            else {
                progress_bar.fillAmount = percent;
                currentDronePosition.x = 6.66f * 2 * percent - 6.66f;
                drone.transform.position = currentDronePosition;
            }

            if (percent > 0.81f) speed_increase = 0.008f;
            else if (percent > 0.8f) speed_increase = 0.00005f;

            if (percent > 1.5f) changeScene();
        }
    }
    public void StartLoadingScene() {
        LoadingScene.SetActive(true);
        PopupButton.SetActive(false);
        isLoading = true;

        progress_bar.fillAmount = 0;
    }
    void changeScene() {
        SceneManager.LoadScene("MainGameplay");
    }
}
