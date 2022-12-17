using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource buttonAud;

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
}
