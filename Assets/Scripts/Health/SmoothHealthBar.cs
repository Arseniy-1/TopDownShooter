using UnityEngine;
using System.Collections;

public class SmoothHealthBar : HealthBar
{
    [SerializeField] private float _smoothDecreaseDuration = 0.5f;

    private Coroutine _coroutine;

    protected override void ShowHealth(float currentHealth, float maxHealth)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SmoothHealthChanging(currentHealth, maxHealth));
    }

    private IEnumerator SmoothHealthChanging(float currentHealth, float maxHealth)
    {
        float elapsedTime = 0f;
        float previousValue = HealthBarView.value;
        float targetValue = currentHealth / maxHealth;

        while (elapsedTime < _smoothDecreaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedPosition = elapsedTime / _smoothDecreaseDuration;
            float intermediateValue = Mathf.Lerp(previousValue, targetValue, normalizedPosition);

            HealthBarView.value = intermediateValue;

            yield return null;
        }
    }
}
