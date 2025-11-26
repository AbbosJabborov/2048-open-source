using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class PauseManager: MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;
    
        private bool _isPaused;
    
        public void PauseTrigger()
        {
            _isPaused = !_isPaused;
            pausePanel.SetActive(_isPaused);
            Time.timeScale = _isPaused ? 0 : 1;
        }

        public void BackToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }
    }
}