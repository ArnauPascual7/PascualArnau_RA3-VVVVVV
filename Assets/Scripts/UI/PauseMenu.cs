using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour, InputSystem_Actions.IMenuActions
{
    private InputSystem_Actions inputActions;
    [SerializeField] private GameObject PauseMenuUI;
    [SerializeField] private GameObject PauseButton;

    public static bool GameIsPaused = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Menu.SetCallbacks(this);

        PauseMenuUI.SetActive(false);
        PauseButton.SetActive(true);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        PauseButton.SetActive(true);

        Time.timeScale = 1f;

        GameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        PauseButton.SetActive(false);

        Time.timeScale = 0f;

        GameIsPaused = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Debug.Log("Quitting game...");

        Application.Quit();
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
}
