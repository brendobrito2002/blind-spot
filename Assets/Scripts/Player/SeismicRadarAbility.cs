using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SeismicRadarAbility : MonoBehaviour
{
    public float pulseRadius = 6f;
    public float cooldownTime = 5f;
    public Key pulseKey = Key.Space;
    public SeismicRadarVisualEffect visualEffect;

    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    public bool IsOnCooldown => isOnCooldown;
    public float CooldownTimer => cooldownTimer;

    // Vai de 0 (acabou de usar) até 1 (pronto pra usar de novo), conforme o cooldown esvazia
    public float CooldownProgress => isOnCooldown ? Mathf.Clamp01(1f - (cooldownTimer / cooldownTime)) : 1f;

    void Update()
    {
        var playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null && playerMovement.isDead) return;

        // Só dispara se a tecla foi apertada NESTE frame (evita repetir enquanto segura) e não está em cooldown
        if (Keyboard.current != null && Keyboard.current[pulseKey].wasPressedThisFrame && !isOnCooldown)
        {
            EmitPulse();
        }
    }

    void EmitPulse()
    {
        var playerMovement = GetComponent<PlayerMovement>();

        if (visualEffect != null)
        {
            playerMovement.OnSeismicRadarAnimation();
        }

        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        // Contagem regressiva frame a frame até liberar o próximo uso
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
        while (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }
        isOnCooldown = false;
        cooldownTimer = 0f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, pulseRadius);
    }

    public void OnRadarAnimationVSFStart()
    {
        visualEffect.PlayPulse(pulseRadius);
    }
}