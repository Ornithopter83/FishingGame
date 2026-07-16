using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishingGame.UI
{
    /// <summary>
    /// PC adapter for the original Title card-wait state. The original flow enters
    /// build index 2 through Loading after payment/card input; Enter, Space, or a
    /// left click provides that input when cabinet hardware is unavailable.
    /// </summary>
    public sealed class TitleController : MonoBehaviour
    {
        private bool transitionStarted;

        private void Awake()
        {
            RuntimeModeSettings.Current = RuntimeMode.Pc;
        }

        private void Start()
        {
            GameLog.Info("Original Title card-wait screen ready. Enter/Space/click simulates cabinet input in PC mode.");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                StartPcDemo();
            }
        }

        public void StartPcDemo()
        {
            if (transitionStarted)
            {
                return;
            }

            transitionStarted = true;
            SceneSession.NextSceneName = SceneSession.ResultSceneName;
            GameLog.Info("PC cabinet input accepted on Title; opening original build-index-2 flow through Loading.");
            SceneManager.LoadScene(SceneSession.LoadingSceneName, LoadSceneMode.Single);
        }
    }
}
