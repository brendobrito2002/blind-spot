using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
        PauseController.SetPause(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            PauseController.SetPause(menuCanvas.activeSelf);
        }

    }

    public void Continue()
    {
        menuCanvas.SetActive(false);
        PauseController.SetPause(false);
    }

    public void RestartFase()
    {
        menuCanvas.SetActive(false);
        PauseController.SetPause(false);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void ReturnTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
