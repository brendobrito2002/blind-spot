using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialController : MonoBehaviour
{
    [Header("Caixas")]
    [SerializeField] private GameObject narrativeBox;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject movementTutorialBox;
    [SerializeField] private GameObject exitBox;

    [Header("Textos")]
    [SerializeField] private TMP_Text narrativeText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text movementTutorialText;
    [SerializeField] private TMP_Text exitText;

    private int currentStep = 0;

    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private SeismicRadarAbility radarAbility;

    void Start()
    {
        playerMovement.TutorialLocked = true;
        narrativeBox.SetActive(true);
        dialogueBox.SetActive(false);
        movementTutorialBox.SetActive(false);
        exitBox.SetActive(false);

        narrativeText.text = "[Você sente uma sensação estranha.]";

        currentStep = 1;
    }

    void Update()
    {
        if (currentStep == 1 && Input.GetMouseButtonDown(0))
        {
            FirstDialogue();
        }
        else if (currentStep == 2 && Input.GetMouseButtonDown(0))
        {
            ShowRadarTutorial();
        }
    }

    void FirstDialogue()
    {
        narrativeBox.SetActive(false);
        dialogueBox.SetActive(true);

        dialogueText.text = "Eu consigo sentir as paredes…";

        currentStep = 2;
    }

    void ShowRadarTutorial()
    {
        dialogueBox.SetActive(false);
        narrativeBox.SetActive(true);

        narrativeText.text = "[Aperte SPACE para ativar radar]";

        radarAbility.TutorialLocked = false;

        currentStep = 3;
    }

    public void RadarActivated()
    {
        if (currentStep != 3)
            return;

        narrativeBox.SetActive(false);

        movementTutorialBox.SetActive(true);
        movementTutorialText.text = "[Use WASD para se movimentar]";

        playerMovement.TutorialLocked = false;

        currentStep = 4;

        StartCoroutine(FinishMovementTutorial());
    }

    public void MovementTutorialFinished()
    {
        if (currentStep != 4)
            return;

        movementTutorialBox.SetActive(false);

        exitBox.SetActive(true);
        exitText.text = "Encontre a saída";

        currentStep = 5;

        StartCoroutine(HideExitBox());
    }

    IEnumerator HideExitBox()
    {
        yield return new WaitForSeconds(4f);

        exitBox.SetActive(false);

        currentStep = 6;
    }

    IEnumerator FinishMovementTutorial()
    {
        yield return new WaitForSeconds(4f);

        MovementTutorialFinished();
    }
}