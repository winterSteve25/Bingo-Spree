using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using DG.Tweening;
using EasyTransition;
using Player;
using Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CompletionUI : MonoBehaviour
    {
        private static CompletionUI _instance;
        private static readonly int TaskLineSize = -65;

        public static void Show(bool timesUp = false)
        {
            _instance.gameObject.SetActive(true);
            _instance.ResetScreen();
            _instance.StartCoroutine(_instance.PlayAnimation(timesUp));
        }

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text completionText;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private RectTransform firstTaskLine;
        [SerializeField] private CanvasGroup separator;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private CanvasGroup continueButton;
        [SerializeField] private TransitionSettings transitionSettings;

        [SerializeField] private TMP_Text taskTextPrefab;
        [SerializeField] private TMP_Text taskScoreTextPrefab;

        private void Awake()
        {
            _instance = this;
            ResetScreen();
            gameObject.SetActive(false);
        }

        private void ResetScreen()
        {
            canvasGroup.alpha = 0;
            completionText.text = "Checked Out!";
            completionText.transform.localScale = new Vector3(3, 3, 1);
            completionText.GetComponent<CanvasGroup>().alpha = 0;
            timeText.text = "";

            for (int i = 0; i < firstTaskLine.childCount; i++)
            {
                Destroy(firstTaskLine.GetChild(i).gameObject);
            }

            separator.alpha = 0;
            scoreText.text = "Score: 0";
            scoreText.GetComponent<CanvasGroup>().alpha = 0;
            continueButton.alpha = 0;
            continueButton.blocksRaycasts = false;
            continueButton.interactable = false;
        }

        private IEnumerator PlayAnimation(bool timesUp)
        {
            PlayerTasks tasks = PlayerTasks.Instance;

            canvasGroup.DOFade(1, 0.5f);
            yield return new WaitForSeconds(1f);

            #region CompletionText

            if (timesUp)
            {
                completionText.text = "Times Up!";
            }

            completionText.transform.DOScale(new Vector3(1, 1, 1), 0.2f)
                .SetEase(Ease.InElastic);
            completionText.GetComponent<CanvasGroup>().DOFade(1, 0.15f);

            yield return new WaitForSeconds(0.15f);
            
            AudioManager.Play(1, SourceType.UI);

            yield return new WaitForSeconds(0.35f);

            #endregion

            #region TimeText

            float t = 0f;
            DOTween.To(() => t, x =>
                {
                    t = x;
                    timeText.text = $"Time - {(int)t / 60:D2}:{(int)t % 60:D2}:{((int)((t - Math.Floor(t)) * 100)):D2}";
                }, tasks.CompletionTime, 2f)
                .SetEase(Ease.OutCubic);

            timeText.transform.DOPunchScale(new Vector3(1.001f, 1.001f, 1), 2f, vibrato: 4, elasticity: 0.0001f);

            yield return new WaitForSeconds(2.5f);
            timeText.transform.DOScale(new Vector3(1, 1, 1), 0.2f);

            #endregion

            int score = 0;

            #region Tasks

            Vector2 size = firstTaskLine.sizeDelta;
            size.y = Mathf.Abs((timesUp ? 1 : tasks.Tasks.Count) * TaskLineSize);
            firstTaskLine.sizeDelta = size;
            firstTaskLine.anchoredPosition = Vector2.zero;
            LayoutRebuilder.ForceRebuildLayoutImmediate(firstTaskLine);

            int offset = 0;

            foreach (var task in tasks.Tasks)
            {
                var s = task.GetScore();
                score += s;

                foreach (var pen in task.GetPenalties())
                {
                    score -= pen.score;
                }

                yield return SpawnTaskCompletionText(task.LeftText(), task.RightText(), offset);
                yield return new WaitForSeconds(0.1f);
                yield return SpawnTaskScoreText(s, offset, task.GetPenalties());
                yield return new WaitForSeconds(0.2f);
                offset++;

                if (timesUp)
                {
                    break;
                }
            }

            #endregion

            #region Score

            separator.DOFade(1, 0.5f);
            yield return new WaitForSeconds(1f);

            scoreText.GetComponent<CanvasGroup>().DOFade(1, 0.15f);
            
            yield return new WaitForSeconds(0.12f);

            if (score != 0)
            {
                AudioManager.Play(2, SourceType.UI);
            }

            yield return new WaitForSeconds(0.03f);

            t = 0f;
            DOTween.To(() => t, x =>
                {
                    t = x;
                    scoreText.text = $"Score: {t:N0}";
                }, score, 2f)
                .SetEase(Ease.OutExpo);
            
            #endregion

            yield return new WaitForSeconds(0.5f);

            continueButton.blocksRaycasts = true;
            continueButton.interactable = true;
            continueButton.DOFade(1f, 0.2f);
        }

        private IEnumerator SpawnTaskScoreText(int score, int offset, List<Penalty> penalties)
        {
            Vector2 targetLoc = new Vector2(firstTaskLine.rect.width / 2f,
                firstTaskLine.offsetMin.y + firstTaskLine.rect.height + offset * TaskLineSize);

            yield return SpawnTxt(score >= 0 ? $"+{score}" : score.ToString(), Color.white);

            foreach (var pen in penalties)
            {
                yield return new WaitForSeconds(0.2f);
                yield return SpawnTxt($"-{pen.score} / {pen.text}", Color.red, 18, 2);
            }

            IEnumerator SpawnTxt(string text, Color color, float fontSize = -1, float delay = 0.5f)
            {
                TMP_Text txt = Instantiate(taskScoreTextPrefab, transform);
                txt.text = text;
                txt.color = color;
                if (fontSize > 0)
                {
                    txt.fontSize = fontSize;
                }

                LayoutRebuilder.ForceRebuildLayoutImmediate(txt.rectTransform);
                txt.rectTransform.anchoredPosition = new Vector3(targetLoc.x, targetLoc.y - 50);
                var canvasGroup = txt.GetComponent<CanvasGroup>();
                canvasGroup.DOFade(1f, 0.2f);
                txt.rectTransform.DOAnchorPos(targetLoc, 0.2f)
                    .SetEase(Ease.OutCubic);
                yield return new WaitForSeconds(0.2f + delay);
                txt.rectTransform.DOAnchorPos(new Vector2(targetLoc.x, targetLoc.y + 50), 0.2f)
                    .SetEase(Ease.InCubic);
                canvasGroup.DOFade(0f, 0.2f)
                    .OnComplete(() => Destroy(txt.gameObject));
            }
        }

        private IEnumerator SpawnTaskCompletionText(string left, string right, int offset)
        {
            TMP_Text leftText = Instantiate(taskTextPrefab, firstTaskLine, false);
            leftText.text = left;
            leftText.rectTransform.pivot = new Vector2(0, 1);
            LayoutRebuilder.ForceRebuildLayoutImmediate(leftText.rectTransform);
            leftText.rectTransform.anchoredPosition = new Vector3(0, leftText.rectTransform.rect.height);

            TMP_Text rightText = Instantiate(taskTextPrefab, firstTaskLine, false);
            rightText.text = right;
            rightText.horizontalAlignment = HorizontalAlignmentOptions.Right;
            rightText.rectTransform.pivot = new Vector2(1, 1);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rightText.rectTransform);
            rightText.rectTransform.anchoredPosition =
                new Vector3(firstTaskLine.rect.width, rightText.rectTransform.rect.height);

            leftText.rectTransform.DOAnchorPos(new Vector2(0, offset * TaskLineSize), 0.3f).SetEase(Ease.OutExpo);
            rightText.rectTransform.DOAnchorPos(new Vector2(firstTaskLine.rect.width, offset * TaskLineSize), 0.3f)
                .SetEase(Ease.OutExpo);

            yield return new WaitForSeconds(0.05f);

            AudioManager.Play(8, SourceType.UI);
            
            yield return new WaitForSeconds(0.25f);
        }

        public void ContinueToMainMenu()
        {
            TransitionManager.Instance().Transition(0, transitionSettings, 0f);
        }
    }
}