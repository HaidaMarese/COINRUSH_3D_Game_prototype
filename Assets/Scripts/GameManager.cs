using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public int score = 0;
    public float startTime = 60f;
    private float timeLeft;
    public bool isGameActive = false;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public GameObject startButton;
    public GameObject titleText;
    public GameObject endGamePanel;
    public TMP_Text endScoreText;
    public TMP_Text bestScoreText;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 0f; // pause at start
        UpdateScoreUI();
        UpdateTimerUI(startTime);
        if (endGamePanel) endGamePanel.SetActive(false);
    }

    void Update()
    {
        if (!isGameActive) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;
        UpdateTimerUI(timeLeft);

        if (timeLeft <= 0f) EndGame();
    }

    public void StartTimer()
    {
        if (isGameActive) return;

        // ? Play start sound
        AudioManager.Instance.PlayStart();

        score = 0;
        UpdateScoreUI();

        timeLeft = startTime;
        UpdateTimerUI(timeLeft);

        isGameActive = true;
        Time.timeScale = 1f;

        if (startButton) startButton.SetActive(false);
        if (titleText)   titleText.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(CountdownTicks());
    }

    IEnumerator CountdownTicks()
    {
        int lastWhole = Mathf.CeilToInt(timeLeft);
        while (isGameActive && timeLeft > 0f)
        {
            int nowWhole = Mathf.CeilToInt(timeLeft);
            if (timeLeft <= 5f && nowWhole != lastWhole)
                AudioManager.Instance.PlayTick(); // ? Beep in last 5 sec

            lastWhole = nowWhole;
            yield return null;
        }
    }

    public void AddScore(int amount)
    {
        if (!isGameActive) return;
        score += amount;
        UpdateScoreUI();
    }

    public void EndGame()
    {
        if (!isGameActive) return;
        isGameActive = false;

        // ? Play game over sound
        AudioManager.Instance.PlayGameOver();

        int best = PlayerPrefs.GetInt("BestScore", 0);
        if (score > best) { best = score; PlayerPrefs.SetInt("BestScore", best); }

        if (endGamePanel) endGamePanel.SetActive(true);
        if (endScoreText)  endScoreText.text  = $"Score: {score}";
        if (bestScoreText) bestScoreText.text = $"Best: {best}";

        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateScoreUI() => scoreText.text = $"Score: {score}";
    void UpdateTimerUI(float t) => timerText.text = $"Time: {Mathf.CeilToInt(t)}";
}
