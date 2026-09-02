using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalController : MonoBehaviour
{
    [Header("Showman")]
    [SerializeField] private GameObject showman;

    [Header("Diálogo inicial")]
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject dialogue2;
    [SerializeField] private GameObject dialogue3;

    [Header("Opções")]
    [SerializeField] private GameObject eyes;
    [SerializeField] private GameObject money;

    [Header("Diálogo dos finais")]
    [SerializeField] private GameObject finalDialogue;

    [Header("Final - Visão")]
    [SerializeField] private GameObject box2;
    [SerializeField] private GameObject eyesFinal;

    [Header("Final - Dinheiro")]
    [SerializeField] private GameObject box3;
    [SerializeField] private GameObject moneyFinal;
    [SerializeField] private GameObject moneyFinal2;

    private int currentStep = 1;

    void Start()
    {
        showman.SetActive(true);

        dialogue.SetActive(true);
        dialogue2.SetActive(false);
        dialogue3.SetActive(false);

        eyes.SetActive(false);
        money.SetActive(false);

        finalDialogue.SetActive(false);

        box2.SetActive(false);
        box3.SetActive(false);

        moneyFinal.SetActive(false);
        moneyFinal2.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextDialogue();
        }
    }

    void NextDialogue()
    {
        if (currentStep == 1)
        {
            dialogue.SetActive(false);
            dialogue2.SetActive(true);

            currentStep = 2;
        }
        else if (currentStep == 2)
        {
            dialogue2.SetActive(false);
            dialogue3.SetActive(true);

            currentStep = 3;
        }
        else if (currentStep == 3)
        {
            eyes.SetActive(true);
            money.SetActive(true);

            currentStep = 4;
        }
        else if (currentStep == 6)
        {
            moneyFinal.SetActive(false);
            moneyFinal2.SetActive(true);

            currentStep = 7;

            StartCoroutine(ReturnToTitleScreen());
        }
    }

    public void ChooseEyes()
    {
        showman.SetActive(false);

        eyes.SetActive(false);
        money.SetActive(false);

        finalDialogue.SetActive(true);

        box2.SetActive(true);
        eyesFinal.SetActive(true);

        currentStep = 5;

        StartCoroutine(ReturnToTitleScreen());
    }

    public void ChooseMoney()
    {
        showman.SetActive(false);

        eyes.SetActive(false);
        money.SetActive(false);

        finalDialogue.SetActive(true);

        box3.SetActive(true);
        moneyFinal.SetActive(true);
        moneyFinal2.SetActive(false);

        currentStep = 6;
    }

    IEnumerator ReturnToTitleScreen()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("titlescreen");
    }
}