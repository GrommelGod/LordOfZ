using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] public bool panelShow = false;
    [SerializeField] private GameObject panelGo;
    [SerializeField] private Image arrow;
    public Transform hiddenPos;
    public Transform shownPos;
    [SerializeField] private float duration = 2.5f;
    private bool sliding = false;
    [SerializeField] private GameObject runnerButton;
    [SerializeField] private GameObject tankButton;

    public void Update()
    {
        if (GameManager.Instance.currentLevelNumber >= 3)
        {
            runnerButton.SetActive(true);
        }

        if (GameManager.Instance.currentLevelNumber >= 6)
        {
            tankButton.SetActive(true);
        }
    }

    public void ShowHidePanel()
    {
        if (!sliding)
        {
            panelShow = !panelShow;

            if (!panelShow)
            {
                StartCoroutine(PanelSlide(shownPos.position));
            }
            else
            {
                StartCoroutine(PanelSlide(hiddenPos.position));
            }
        }
    }

    public IEnumerator PanelSlide(Vector2 targetPos)
    {
        sliding = true;
        float time = 0;
        Vector2 startPos = panelGo.transform.position;

        while (time < duration)
        {
            panelGo.transform.position = Vector2.Lerp(startPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        panelGo.transform.position = targetPos;
        sliding = false;
    }
}
