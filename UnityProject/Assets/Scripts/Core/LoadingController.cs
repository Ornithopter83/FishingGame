using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishingGame
{
    /// <summary>
    /// Hardware-independent loading flow. Original visuals and manager
    /// integrations are connected later without changing the scene contract.
    /// </summary>
    public sealed class LoadingController : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        private float minimumDisplaySeconds = 1f;

        private string status = "Preparing...";
        private float progress;

        private void Start()
        {
            StartCoroutine(ContinueFlow());
        }

        private void OnGUI()
        {
            const float width = 460f;
            const float height = 180f;
            Rect panel = new Rect((Screen.width - width) * 0.5f, (Screen.height - height) * 0.5f, width, height);

            GUI.Box(panel, "Loading");
            GUI.Label(new Rect(panel.x + 24f, panel.y + 46f, width - 48f, 24f), status);
            GUI.HorizontalSlider(new Rect(panel.x + 24f, panel.y + 84f, width - 48f, 24f), progress, 0f, 1f);
            GUI.Label(new Rect(panel.x + 24f, panel.y + 116f, width - 48f, 24f),
                "Spine / Serial / Addressables integrations are not connected yet.");
        }

        private IEnumerator ContinueFlow()
        {
            string target = SceneSession.NextSceneName;
            if (string.IsNullOrWhiteSpace(target) || target == SceneSession.LoadingSceneName)
            {
                target = SceneSession.BootstrapSceneName;
            }

            status = "Safe loading delay...";
            float elapsed = 0f;
            while (elapsed < minimumDisplaySeconds)
            {
                elapsed += Time.unscaledDeltaTime;
                progress = minimumDisplaySeconds <= 0f ? 1f : Mathf.Clamp01(elapsed / minimumDisplaySeconds);
                yield return null;
            }

            status = "Opening " + target + "...";
            progress = 1f;
            GameLog.Info("Loading scene: " + target);

            AsyncOperation operation = SceneManager.LoadSceneAsync(target, LoadSceneMode.Single);
            if (operation == null)
            {
                status = "Failed to open " + target;
                GameLog.Error(status);
                yield break;
            }

            while (!operation.isDone)
            {
                yield return null;
            }
        }
    }
}

