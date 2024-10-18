using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace Menu
{
    [AddComponentMenu("GameDev/Menu/Loading Scene")]
    public class LoadingScene : MonoBehaviour
    {
        public GameObject loadingScenePanel;
        public Image progressBar;
        public Text progressText;

        IEnumerator LoadAsynchronously(int sceneIndex)
        {
            loadingScenePanel.SetActive(true);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                progressBar.fillAmount = progress;
                // progressText.text = $"{progress*100:P0}";
                progressText.text = $"{progress:P0}";
                yield return null;
            }
        }

        public void LoadLevelAsync(int sceneIndex)
        {
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }
    }
}