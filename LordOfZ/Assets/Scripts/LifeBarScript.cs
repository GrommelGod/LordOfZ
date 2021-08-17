using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarScript : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private Image lifeBarFront;
    [SerializeField] private float updateSpeedSeconds = .5f;

    private void Awake()
    {
        character.OnHealthPctChanged += HandleHealthChanged;
    }

    public void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }

    private IEnumerator ChangeToPct(float pct)
    {
        //Debug.Log("here");
        float preChangePct = lifeBarFront.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            lifeBarFront.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        lifeBarFront.fillAmount = pct;
    }

    void LateUpdate()
    {
        transform.LookAt(GameManager.Instance.cam.transform);
    }
}
