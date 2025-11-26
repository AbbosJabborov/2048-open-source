using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class GameOverManager : MonoBehaviour
    {
        public GameObject gameOverPanel;
        public GameObject winPanel;

        public float fadeDuration = 0.5f;
        private CanvasGroup _gameOverCanvas;
        private CanvasGroup _winCanvas;

        void Awake()
        {
            SetupPanel(gameOverPanel, ref _gameOverCanvas);
            SetupPanel(winPanel, ref _winCanvas);
        }

        void SetupPanel(GameObject panel, ref CanvasGroup cg)
        {
            if (panel == null) return;
            cg = panel.GetComponent<CanvasGroup>() ?? panel.AddComponent<CanvasGroup>();
            cg.alpha = 0;
            panel.SetActive(false);
        }

        public void TriggerGameOver()
        {
            if (gameOverPanel == null) return;
            gameOverPanel.SetActive(true);
            _gameOverCanvas.DOFade(1f, fadeDuration);
        }

        public void TriggerWin()
        {
            if (winPanel == null) return;
            winPanel.SetActive(true);
            _winCanvas.DOFade(1f, fadeDuration);
        }
        public void RestartLevel()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(sceneBuildIndex: SceneManager.GetActiveScene().buildIndex);
        }
    }
}