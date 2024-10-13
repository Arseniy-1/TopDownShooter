using TMPro;
using UnityEngine;

public class HealthText : HealthView
{
    [SerializeField] private TextMeshProUGUI _healthView;

    protected override void ShowHealth(float currentHealth, float maxHealth)
    {
        _healthView.text = Mathf.Round(currentHealth).ToString();
    }
}
