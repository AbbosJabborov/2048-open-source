using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class MenuManager: MonoBehaviour
    {
        public void PlaySurvivalMode()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }

        public void PlayTimedMode(int scoreTarget)
        {
            Time.timeScale = 1;
            GameModeSettings.scoreTarget = scoreTarget;
            SceneManager.LoadScene(sceneBuildIndex: 2);
        }
        public void SfxOffTrigger()
        {
            if (Mathf.Approximately(AudioListener.volume, 1))
            {
                AudioListener.volume = 0;
            }
            else
            {
                AudioListener.volume = 1;
            }
        }
    }
}