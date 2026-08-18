using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class SeismicRadarVisualEffect : MonoBehaviour
{
    public float expandDuration = 0.25f;
    public float holdDuration = 1f;
    public float fadeOutDuration = 0.5f;
    public float maxIntensity = 1.5f;
    public AnimationCurve expandCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Light2D light2D;
    private Coroutine currentRoutine;

    void Awake()
    {
        light2D = GetComponent<Light2D>();

        // Ângulo 360° faz o tipo "Spot" iluminar em todas as direções, como uma luz de ponto
        light2D.pointLightOuterAngle = 360f;
        light2D.pointLightInnerAngle = 360f;

        light2D.pointLightOuterRadius = 0f;
        light2D.intensity = 0f;

        // Desligado por padrão para não contribuir nada na cena fora do momento do pulso
        light2D.enabled = false;
    }

    public void PlayPulse(float targetRadius)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine); // cancela um pulso anterior ainda em andamento, se houver

        light2D.enabled = true;
        currentRoutine = StartCoroutine(PulseRoutine(targetRadius));
    }

    IEnumerator PulseRoutine(float targetRadius)
    {
        // Fase 1: raio e intensidade sobem juntos de 0 até o máximo, seguindo a curva de expansão
        float t = 0f;
        while (t < expandDuration)
        {
            t += Time.deltaTime;
            float progress = expandCurve.Evaluate(t / expandDuration);
            light2D.pointLightOuterRadius = Mathf.Lerp(0f, targetRadius, progress);
            light2D.intensity = Mathf.Lerp(0f, maxIntensity, progress);
            yield return null;
        }
        light2D.pointLightOuterRadius = targetRadius;
        light2D.intensity = maxIntensity;

        // Fase 2: fica parado no raio máximo por um tempo, sem animação
        yield return new WaitForSeconds(holdDuration);

        // Fase 3: raio e intensidade descem juntos de volta a 0, dando o efeito de "recolher"
        t = 0f;
        float startIntensity = light2D.intensity;
        float startRadius = light2D.pointLightOuterRadius;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            float progress = t / fadeOutDuration;
            light2D.intensity = Mathf.Lerp(startIntensity, 0f, progress);
            light2D.pointLightOuterRadius = Mathf.Lerp(startRadius, 0f, progress);
            yield return null;
        }
        light2D.intensity = 0f;
        light2D.pointLightOuterRadius = 0f;
        light2D.enabled = false; // desliga de novo até o próximo pulso ser chamado
    }
}