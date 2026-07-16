using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FishingGame
{
    /// <summary>
    /// PC-safe loading flow. Visuals are reconstructed from the original Loading Scene,
    /// while Serial and Addressables remain optional backends.
    /// </summary>
    public sealed class LoadingController : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        private float minimumDisplaySeconds = 1.8f;

        private Text statusLabel;
        private Text dotsLabel;
        private Image progressFill;
        private string status = "PREPARING";
        private float progress;

        private void Awake()
        {
            statusLabel = FindComponent<Text>("StatusText");
            dotsLabel = FindComponent<Text>("LoadingDots");
            progressFill = FindComponent<Image>("ProgressFill");
        }

        private void Start()
        {
            StartCoroutine(ContinueFlow());
        }

        private void Update()
        {
            if (statusLabel != null)
            {
                statusLabel.text = status;
            }

            if (dotsLabel != null)
            {
                int count = 1 + Mathf.FloorToInt(Time.unscaledTime * 2f) % 3;
                dotsLabel.text = new string('•', count);
            }

            if (progressFill != null)
            {
                progressFill.fillAmount = progress;
            }
        }

        private void OnGUI()
        {
            if (statusLabel != null)
            {
                return;
            }

            GUI.Label(new Rect(24f, 24f, 500f, 30f), status);
        }

        private IEnumerator ContinueFlow()
        {
            string target = SceneSession.NextSceneName;
            if (string.IsNullOrWhiteSpace(target) || target == SceneSession.LoadingSceneName)
            {
                target = SceneSession.TitleSceneName;
            }

            status = "PC MODE  •  LOADING";
            float elapsed = 0f;
            while (elapsed < minimumDisplaySeconds)
            {
                elapsed += Time.unscaledDeltaTime;
                progress = minimumDisplaySeconds <= 0f ? 1f : Mathf.Clamp01(elapsed / minimumDisplaySeconds);
                yield return null;
            }

            status = "OPENING  " + target.ToUpperInvariant();
            progress = 1f;
            GameLog.Info("Loading scene in PC mode: " + target);

            AsyncOperation operation = SceneManager.LoadSceneAsync(target, LoadSceneMode.Single);
            if (operation == null)
            {
                status = "FAILED TO OPEN  " + target.ToUpperInvariant();
                GameLog.Error(status);
                yield break;
            }

            while (!operation.isDone)
            {
                yield return null;
            }
        }

        private static T FindComponent<T>(string objectName) where T : Component
        {
            GameObject target = GameObject.Find(objectName);
            return target == null ? null : target.GetComponent<T>();
        }
    }
}
