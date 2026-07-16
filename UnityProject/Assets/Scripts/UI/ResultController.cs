using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FishingGame.UI
{
    public sealed class ResultController : MonoBehaviour
    {
        private static readonly string[] FishNames = { "BLUEFIN TUNA", "RED SEA BREAM", "GIANT GROUPER" };
        private static readonly string[] Ranks = { "S", "A", "SS" };
        private static readonly string[] Weights = { "128.4 kg", "42.7 kg", "201.8 kg" };

        private Text fishName;
        private Text rank;
        private Text weight;
        private int index;

        private void Awake()
        {
            fishName = Find<Text>("FishNameText");
            rank = Find<Text>("RankValueText");
            weight = Find<Text>("WeightValueText");
            Bind("PreviousButton", Previous);
            Bind("NextButton", Next);
            Bind("ReturnButton", ReturnToTitle);
        }

        private void Start()
        {
            Refresh();
            GameLog.Info("Result preview ready in PC mode.");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                Previous();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                Next();
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
            {
                ReturnToTitle();
            }
        }

        public void Previous()
        {
            index = (index + FishNames.Length - 1) % FishNames.Length;
            Refresh();
        }

        public void Next()
        {
            index = (index + 1) % FishNames.Length;
            Refresh();
        }

        public void ReturnToTitle()
        {
            SceneSession.NextSceneName = SceneSession.TitleSceneName;
            SceneManager.LoadScene(SceneSession.LoadingSceneName, LoadSceneMode.Single);
        }

        private void Refresh()
        {
            if (fishName != null) fishName.text = FishNames[index];
            if (rank != null) rank.text = Ranks[index];
            if (weight != null) weight.text = Weights[index];
        }

        private static void Bind(string objectName, UnityEngine.Events.UnityAction action)
        {
            Button button = Find<Button>(objectName);
            if (button != null)
            {
                button.onClick.AddListener(action);
            }
        }

        private static T Find<T>(string objectName) where T : Component
        {
            GameObject target = GameObject.Find(objectName);
            return target == null ? null : target.GetComponent<T>();
        }
    }
}
