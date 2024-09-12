using System;
using System.Collections;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;

namespace Tasks
{
    public class CompletionUI : MonoBehaviour
    {
        private static CompletionUI _instance;

        public static void Show()
        {
            _instance.gameObject.SetActive(true);
            _instance.ResetScreen();
            _instance.StartCoroutine(_instance.PlayAnimation());
        }

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text completionText;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private RectTransform firstTaskLine;
        [SerializeField] private CanvasGroup separator;
        [SerializeField] private TMP_Text scoreText;

        [SerializeField] private TMP_Text taskTextPrefab;

        private void Awake()
        {
            _instance = this;
            ResetScreen();
            gameObject.SetActive(false);
        }

        private void ResetScreen()
        {
            canvasGroup.alpha = 0;
            completionText.transform.localScale = new Vector3(3, 3, 1);
            completionText.GetComponent<CanvasGroup>().alpha = 0;
            timeText.text = "Time - 00:00:00";

            for (int i = 0; i < firstTaskLine.childCount; i++)
            {
                Destroy(firstTaskLine.GetChild(i).gameObject);
            }

            separator.alpha = 0;
            scoreText.text = "Score: 0";
        }

        private IEnumerator PlayAnimation()
        {
            canvasGroup.DOFade(1, 0.5f);
            yield return new WaitForSeconds(1f);

            completionText.transform.DOScale(new Vector3(1, 1, 1), 0.2f)
                .SetEase(Ease.InQuad);
            completionText.GetComponent<CanvasGroup>().DOFade(1, 0.15f);
            
            yield return new WaitForSeconds(0.5f);
            
            PlayerTasks tasks = PlayerTasks.Instance;

            float t = 0f;
            float cd = 0.4f;
            
            DOTween.To(() => t, x =>
            {
                cd += x - t;
                t = x;
                
                timeText.text = $"Time - {(int)t / 60:D2}:{(int)t % 60:D2}:{((int)((t - Math.Floor(t)) * 100)):D2}";

                if (cd >= 0.4f)
                {
                    timeText.transform.DOPunchScale(new Vector3(1.000000001f, 1.000000001f, 1), 0.2f, vibrato:5, elasticity:0.2f);
                    cd = 0;
                }
            }, tasks.CompletionTime, 2f);

            yield return new WaitForSeconds(2.5f);
            timeText.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
        }
    }
}