using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenu;
    public GameObject joystick;
    public GameObject fire;
    public GameObject Crosshair;
    public GameObject Pausebutton;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenu != null)
            pauseMenu.SetActive(true);
            joystick.SetActive(false);
            fire.SetActive(false);
            Pausebutton.SetActive(false);
            Crosshair.SetActive(false);
    }
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);
            joystick.SetActive(true);
            fire.SetActive(true);
            Pausebutton.SetActive(true);
            Crosshair.SetActive(true);
    }
    public void ReplayGame()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("reload");
    }
}
