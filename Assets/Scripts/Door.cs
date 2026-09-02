using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject interactionText;
    [SerializeField] private LevelOneController levelOneController;
    [SerializeField] private GameObject mapTransition;

    private bool playerNearby = false;

    void Start()
    {
        if (interactionText != null)
            interactionText.SetActive(false);

        if (mapTransition != null)
            mapTransition.SetActive(false);
    }

    void Update()
    {
        if (!playerNearby)
            return;

        if (levelOneController.IsDoorOpen())
        {
            if (interactionText != null)
                interactionText.SetActive(false);

            if (mapTransition != null)
                mapTransition.SetActive(true);
        }
        else
        {
            if (interactionText != null)
                interactionText.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = true;

        if (!levelOneController.IsDoorOpen())
        {
            if (interactionText != null)
                interactionText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = false;

        if (interactionText != null)
            interactionText.SetActive(false);
    }
}