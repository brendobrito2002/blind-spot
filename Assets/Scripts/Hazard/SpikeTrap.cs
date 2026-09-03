using System.Collections;
using UnityEngine;

namespace BlindSpot.Hazards
{
    public class SpikeTrap : MonoBehaviour
    {
        private enum TrapState { Retracted, Warning, Extended }

        [Header("Ciclo da Armadilha")]
        [SerializeField] private float retractedDuration = 2f;
        [SerializeField] private float warningDuration = 0.6f;
        [SerializeField] private float extendedDuration = 1f;
        [SerializeField] private bool randomizeStartOffset = true;

        [Header("Detecção de Dano")]
        [Tooltip("Collider filho, com tag \"KillZone\" e Is Trigger = true. " +
                 "Fica desativado exceto durante o estado Extended.")]
        [SerializeField] private Collider2D killZoneCollider;

        [Header("Visual (Pode ser animação se der tempo)")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite retractedSprite;
        [SerializeField] private Sprite warningSprite;
        [SerializeField] private Sprite extendedSprite;

        [Header("Áudio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip warningClip;
        [SerializeField] private AudioClip popClip;

        [Header("Reação ao Sonar (opcional, cosmético)")]
        [SerializeField] private GameObject revealHighlight; // ex: um contorno/sprite extra
        [SerializeField] private float revealDuration = 1f;

        private TrapState currentState = TrapState.Retracted;

        private void Start()
        {
            if (revealHighlight != null)
            {
                revealHighlight.SetActive(false);
            }

            StartCoroutine(TrapCycle());
        }

        private IEnumerator TrapCycle()
        {
            if (randomizeStartOffset)
            {
                yield return new WaitForSeconds(Random.Range(0f, retractedDuration));
            }

            while (true)
            {
                SetState(TrapState.Retracted);
                yield return new WaitForSeconds(retractedDuration);

                SetState(TrapState.Warning);
                yield return new WaitForSeconds(warningDuration);

                SetState(TrapState.Extended);
                yield return new WaitForSeconds(extendedDuration);
            }
        }

        private void SetState(TrapState newState)
        {
            currentState = newState;

            switch (newState)
            {
                case TrapState.Retracted:
                    if (spriteRenderer != null) spriteRenderer.sprite = retractedSprite;
                    break;

                case TrapState.Warning:
                    if (spriteRenderer != null) spriteRenderer.sprite = warningSprite;
                    PlayClip(warningClip);
                    break;

                case TrapState.Extended:
                    if (spriteRenderer != null) spriteRenderer.sprite = extendedSprite;
                    PlayClip(popClip);
                    break;
            }

            // O PlayerMovement já mata o jogador ao tocar qualquer collider com tag "KillZone" —
            // só precisamos ligar/desligar esse collider junto com o estado.
            if (killZoneCollider != null)
            {
                killZoneCollider.enabled = (newState == TrapState.Extended);
            }
        }

        private void PlayClip(AudioClip clip)
        {
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }

        // Chamado pelo PulseAbility (igual aos mobs). Aqui é só cosmético:
        // revela visualmente a armadilha por um tempo, sem afetar o ciclo dela.
        public void OnPulseHit(float duration)
        {
            if (revealHighlight != null)
            {
                StopCoroutine(nameof(HideRevealAfterDelay));
                revealHighlight.SetActive(true);
                StartCoroutine(HideRevealAfterDelay());
            }
        }

        private IEnumerator HideRevealAfterDelay()
        {
            yield return new WaitForSeconds(revealDuration);
            if (revealHighlight != null)
            {
                revealHighlight.SetActive(false);
            }
        }

    }
}