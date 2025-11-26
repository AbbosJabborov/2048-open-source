using TMPro;
using UnityEngine;

namespace Manager
{
    public class TimedMode : MonoBehaviour
    {
        public ImageTimer timer;
        public GameOverManager gameOverManager;
        public ScoreUpdater scoreUpdater;
        public TMP_Text goalText;

        private int _targetScore;
        private bool _gameEnded = false;

        void Start()
        {
            _targetScore = GameModeSettings.scoreTarget;
            UpdateGoalUI();

            timer.OnTimerEnd += OnTimerFinished;
            timer.StartTimer();
        }

        void Update()
        {
            if (_gameEnded) return;

            if (scoreUpdater.CurrentScore >= _targetScore)
                WinEarly();
        }

        void OnTimerFinished()
        {
            if (_gameEnded) return; // prevents double result

            if (scoreUpdater.CurrentScore >= _targetScore)
                gameOverManager.TriggerWin();
            else
                gameOverManager.TriggerGameOver();

            _gameEnded = true;
        }

        void WinEarly()
        {
            _gameEnded = true;
            timer.StopTimer(); // stop ticking
            gameOverManager.TriggerWin();
        }
        void UpdateGoalUI()
        {
            if (goalText != null)
                goalText.text = $"Goal: {_targetScore} Pts in {Mathf.RoundToInt(timer.totalTime)}s";
        }

    }
}