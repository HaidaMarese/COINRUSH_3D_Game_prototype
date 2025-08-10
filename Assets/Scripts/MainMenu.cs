using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject startButton;
    public GameObject titleText;

    public void StartGame()
    {
        startButton.SetActive(false);
        titleText.SetActive(false);
        GameManager.Instance.StartTimer();
    }
}
