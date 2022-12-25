using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("PAUSE")]
    private bool activePause;
    public GameObject pauseMenu;
    public GameObject gameHUD;
    public AudioSource buttonAud;

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        TooglePause();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayButtonSound(AudioClip _clip)
    {
        buttonAud.clip = _clip;
        buttonAud.Play();
    }


    public void TooglePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activePause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        activePause = true;
        gameHUD.SetActive(false);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        activePause = false;
        gameHUD.SetActive(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
