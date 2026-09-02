using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [Header("Telas")]
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject messageScreen;
    [SerializeField] private GameObject screenReaderOne;
    [SerializeField] private GameObject screenReaderTwo;
    [SerializeField] private GameObject screenReaderThree;

    [Header("Escolhas")]
    [SerializeField] private GameObject acceptButton;
    [SerializeField] private GameObject refuseButton;

    [Header("Glitch")]
    [SerializeField] private GameObject glitch;

    [Header("Reality Show")]
    [SerializeField] private GameObject realityShow;
    [SerializeField] private GameObject stage;
    [SerializeField] private GameObject showman;
    [SerializeField] private GameObject stage2;
    [SerializeField] private GameObject door;

    [Header("Próxima cena")]
    [SerializeField] private string nextSceneName;

    private int currentScreen = 0;

    void Start()
    {
        blackScreen.SetActive(true);
        messageScreen.SetActive(false);

        screenReaderOne.SetActive(false);
        screenReaderTwo.SetActive(false);
        screenReaderThree.SetActive(false);

        acceptButton.SetActive(false);
        refuseButton.SetActive(false);

        if (glitch != null)
            glitch.SetActive(false);

        realityShow.SetActive(false);
        stage.SetActive(false);
        showman.SetActive(false);
        stage2.SetActive(false);
        door.SetActive(false);

        currentScreen = 0;
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentScreen != 3)
            {
                NextScreen();
            }
        }
    }

    void NextScreen()
    {
        if (currentScreen == 0)
        {
            blackScreen.SetActive(false);
            messageScreen.SetActive(true);
            screenReaderOne.SetActive(true);

            currentScreen = 1;
        }
        else if (currentScreen == 1)
        {
            screenReaderOne.SetActive(false);
            screenReaderTwo.SetActive(true);

            currentScreen = 2;
        }
        else if (currentScreen == 2)
        {
            screenReaderTwo.SetActive(false);
            screenReaderThree.SetActive(true);

            acceptButton.SetActive(true);
            refuseButton.SetActive(true);

            currentScreen = 3;
        }
        else if (currentScreen == 4)
        {
            stage.SetActive(false);
            showman.SetActive(true);

            currentScreen = 5;
        }
        else if (currentScreen == 5)
        {
            showman.SetActive(false);
            stage2.SetActive(true);

            currentScreen = 6;
        }
        else if (currentScreen == 6)
        {
            stage2.SetActive(false);
            door.SetActive(true);

            currentScreen = 7;
        }
        else if (currentScreen == 7)
        {
            SceneManager.LoadSceneAsync(nextSceneName);
        }
    }

    public void Accept()
    {
        messageScreen.SetActive(false);
        screenReaderThree.SetActive(false);
        acceptButton.SetActive(false);
        refuseButton.SetActive(false);

        realityShow.SetActive(true);
        stage.SetActive(true);

        currentScreen = 4;
    }

    public void Refuse()
    {
        if (glitch != null)
        {
            glitch.SetActive(true);
            Invoke(nameof(DisableGlitch), 0.8f);
        }
    }

    void DisableGlitch()
    {
        glitch.SetActive(false);
    }
}