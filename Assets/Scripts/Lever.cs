using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private GameObject leverInteraction;
    [SerializeField] private LevelOneController levelOneController;

    private SpriteRenderer spriteRenderer;
    private bool playerNearby = false;
    private bool activated = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = inactiveSprite;
        leverInteraction.SetActive(false);
    }

    void Update()
    {
        if (playerNearby && !activated && Input.GetKeyDown(KeyCode.E))
        {
            Activate();
        }
    }

    private void Activate()
    {
        activated = true;

        spriteRenderer.sprite = activeSprite;
        leverInteraction.SetActive(false);

        levelOneController.OpenDoor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (activated)
            return;

        playerNearby = true;
        leverInteraction.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = false;
        leverInteraction.SetActive(false);
    }
}