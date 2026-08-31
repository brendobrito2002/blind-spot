using TMPro;
using UnityEngine;

public class AbilityCooldownUI : MonoBehaviour
{
    [SerializeField] private SeismicRadarAbility ability;
    [SerializeField] private TMP_Text time;

    void Update()
    {
        if (ability.IsOnCooldown)
        {
            time.text = Mathf.CeilToInt(ability.CooldownTimer).ToString();
        }
        else
        {
            time.text = "READY";
        }
    }
}