using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreUpdater : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text scoreText;

    [Header("Animation")]
    public float punchScale = 1.2f;
    public float punchDuration = 0.2f;

    private int _score = 0;
    private int _displayedScore = 0;
    private Tween _scoreTween;

    public int CurrentScore => _score;
    
    void Start()
    {
        UpdateScoreDisplay();
    }
    
    public void AddScore(int value)
    {
        if (value <= 0) return;

        _score += value;

        // Kill previous tween if active
        if (_scoreTween != null && _scoreTween.IsActive())
            _scoreTween.Kill();

        // Smooth text number tween
        int startValue = _displayedScore;
        int targetValue = _score;

        _scoreTween = DOTween.To(() => startValue, x =>
                                                  {
                                                      _displayedScore = x;
                                                      scoreText.text = _displayedScore.ToString();
                                                  },
                                targetValue, 0.3f).SetEase(Ease.OutQuad);

        // Small punch animation
        scoreText.transform.DOKill();
        scoreText.transform.DOPunchScale(Vector3.one * (punchScale - 1f), punchDuration, 1, 0.5f);
    }

    public void SetScore(int value)
    {
        _score = value;
        _displayedScore = value;
        scoreText.text = _score.ToString();
    }
    public void ResetScore()
    {
        _score = 0;
        _displayedScore = 0;
        UpdateScoreDisplay();
    }
    public void PlayNoUndoAnimation()
    {
        // kill any previous animation to avoid stacking
        scoreText.transform.DOKill();
        scoreText.DOKill();

        // store original color and scale
        Color originalColor = scoreText.color;
        Vector3 originalScale = scoreText.transform.localScale;

        Sequence seq = DOTween.Sequence();

        // flash red
        seq.Append(scoreText.DOColor(Color.red, 0.15f));

        // bounce effect
        seq.Join(scoreText.transform.DOScale(originalScale * 1.15f, 0.15f));

        // bounce back & restore color
        seq.Append(scoreText.DOColor(originalColor, 0.25f));
        seq.Join(scoreText.transform.DOScale(originalScale, 0.25f));

        // slight shake to emphasize failure
        seq.Append(scoreText.transform
                       .DOShakePosition(0.2f, strength: 10f, vibrato: 20, randomness: 90));

        seq.Play();
    }


    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = _score.ToString();
    }
}
