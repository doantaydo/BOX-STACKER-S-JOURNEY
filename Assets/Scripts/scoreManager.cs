using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour
{
    public Text currentScoreText, bestScoreText, finalScoreText;
    public static scoreManager instanceScoreManager;

    private int currentScore;
    void Awake() {
        if (instanceScoreManager == null) instanceScoreManager = this;
        currentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentScoreText.text = "Score: " + currentScore.ToString("D5");
        finalScoreText.text = "Score: " + currentScore.ToString("D5");
        bestScoreText.text = "Best: 00000";
    }

    public void changeScore(int delta) {
        currentScore += delta;
        if (currentScore < 0) currentScore = 0;
    }

    public void startScore() {
        currentScore = 0;
    }

    public int getScore() => currentScore;
}
