#if UNITY_EDITOR
using System;
using FishingGame.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FishingGame.Editor
{
    /// <summary>
    /// Runs the Task 06 PC-only scene loop without requiring any serial hardware.
    /// The verifier is safe to invoke from the Editor menu or Unity batch mode.
    /// </summary>
    [InitializeOnLoad]
    public static class Task06FlowVerifier
    {
        private const string ActiveKey = "FishingGame.Task06Flow.Active";
        private const string StageKey = "FishingGame.Task06Flow.Stage";
        private const string StartedKey = "FishingGame.Task06Flow.Started";
        private const string PassedKey = "FishingGame.Task06Flow.Passed";
        private const string MessageKey = "FishingGame.Task06Flow.Message";
        private const string MovementStartedKey = "FishingGame.Task06Flow.MovementStarted";
        private const string ShipXKey = "FishingGame.Task06Flow.ShipX";
        private const string ShipYKey = "FishingGame.Task06Flow.ShipY";
        private const string ShipZKey = "FishingGame.Task06Flow.ShipZ";
        private const double TimeoutSeconds = 20d;

        static Task06FlowVerifier()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            if (SessionState.GetBool(ActiveKey, false) && EditorApplication.isPlaying)
            {
                AttachUpdate();
            }
        }

        [MenuItem("Fishing Game/Verify Task 06 PC Flow")]
        public static void StartVerification()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                throw new InvalidOperationException("Exit Play Mode before starting Task 06 verification.");
            }

            SessionState.SetBool(ActiveKey, true);
            SessionState.SetInt(StageKey, 0);
            SessionState.SetString(StartedKey, DateTime.UtcNow.Ticks.ToString());
            SessionState.SetBool(PassedKey, false);
            SessionState.SetString(MessageKey, "Verification did not finish.");

            EditorSceneManager.OpenScene("Assets/Scenes/Menu/Title.unity", OpenSceneMode.Single);
            EditorApplication.EnterPlaymode();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (!SessionState.GetBool(ActiveKey, false))
            {
                return;
            }

            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                AttachUpdate();
                return;
            }

            if (state != PlayModeStateChange.EnteredEditMode)
            {
                return;
            }

            EditorApplication.update -= VerifyUpdate;
            bool passed = SessionState.GetBool(PassedKey, false);
            string message = SessionState.GetString(MessageKey, "Verification ended without a result.");
            SessionState.SetBool(ActiveKey, false);

            if (passed)
            {
                Debug.Log("[FishingGame] Task 06 PC flow verification passed: " + message);
            }
            else
            {
                Debug.LogError("[FishingGame] Task 06 PC flow verification failed: " + message);
            }

            if (Application.isBatchMode)
            {
                EditorApplication.Exit(passed ? 0 : 1);
            }
        }

        private static void AttachUpdate()
        {
            EditorApplication.update -= VerifyUpdate;
            EditorApplication.update += VerifyUpdate;
        }

        private static void VerifyUpdate()
        {
            if (!EditorApplication.isPlaying)
            {
                return;
            }

            if (ElapsedSeconds() > TimeoutSeconds)
            {
                Finish(false, "Timed out while waiting for the restored scene loop.");
                return;
            }

            string sceneName = SceneManager.GetActiveScene().name;
            int stage = SessionState.GetInt(StageKey, 0);

            if (stage == 0 && sceneName == SceneSession.TitleSceneName)
            {
                TitleController title = UnityEngine.Object.FindObjectOfType<TitleController>();
                TitleStatusView status = UnityEngine.Object.FindObjectOfType<TitleStatusView>();
                TitleShipMotion ship = UnityEngine.Object.FindObjectOfType<TitleShipMotion>();
                TitleIdleLoop idleLoop = UnityEngine.Object.FindObjectOfType<TitleIdleLoop>();
                if (title == null || status == null || ship == null || idleLoop == null || UnityEngine.Object.FindObjectOfType<Canvas>() == null)
                {
                    Finish(false, "Title opened without its controller, mutable status UI, idle loop, ship motion, or Canvas.");
                    return;
                }

                status.SetPaymentMessage("PC CARD TEST");
                status.SetTensionLevel(4);
                Text payment = GameObject.Find("payment_text")?.GetComponent<Text>();
                Text tension = GameObject.Find("power_lv")?.GetComponent<Text>();
                if (payment == null || payment.text != "PC CARD TEST" || tension == null || tension.text != "장력 : 4")
                {
                    Finish(false, "The Title payment or tension UI did not accept a runtime update.");
                    return;
                }

                Vector3 position = ship.transform.position;
                SessionState.SetFloat(ShipXKey, position.x);
                SessionState.SetFloat(ShipYKey, position.y);
                SessionState.SetFloat(ShipZKey, position.z);
                SessionState.SetString(MovementStartedKey, DateTime.UtcNow.Ticks.ToString());
                idleLoop.StartVerificationCycle();
                SessionState.SetInt(StageKey, 10);
                return;
            }

            if (stage == 10 && sceneName == SceneSession.TitleSceneName && ElapsedSince(MovementStartedKey) > 0.35d)
            {
                TitleController title = UnityEngine.Object.FindObjectOfType<TitleController>();
                TitleShipMotion ship = UnityEngine.Object.FindObjectOfType<TitleShipMotion>();
                TitleIdleLoop idleLoop = UnityEngine.Object.FindObjectOfType<TitleIdleLoop>();
                Vector3 before = new Vector3(
                    SessionState.GetFloat(ShipXKey, 0f),
                    SessionState.GetFloat(ShipYKey, 0f),
                    SessionState.GetFloat(ShipZKey, 0f));
                if (title == null || ship == null || idleLoop == null || (ship.transform.position - before).sqrMagnitude < 0.0001f)
                {
                    Finish(false, "The restored Title ship or idle loop was not available in Play Mode.");
                    return;
                }

                if (idleLoop.CompletedCycles < 1 || idleLoop.CurrentStage != TitleIdleLoop.Stage.Title)
                {
                    return;
                }

                SessionState.SetInt(StageKey, 1);
                title.StartPcDemo();
                return;
            }

            if (stage == 1 && sceneName == SceneSession.ResultSceneName)
            {
                ResultController result = UnityEngine.Object.FindObjectOfType<ResultController>();
                Text fishName = GameObject.Find("FishNameText")?.GetComponent<Text>();
                if (result == null || fishName == null)
                {
                    Finish(false, "LastScene opened without its result controller or fish label.");
                    return;
                }

                string before = fishName.text;
                result.Next();
                if (fishName.text == before)
                {
                    Finish(false, "The PC result-selection action did not update the displayed fish.");
                    return;
                }

                SessionState.SetInt(StageKey, 2);
                result.ReturnToTitle();
                return;
            }

            if (stage == 2 && sceneName == SceneSession.TitleSceneName)
            {
                Finish(true, "Title -> Loading -> LastScene -> Loading -> Title");
            }
        }

        private static double ElapsedSeconds()
        {
            return ElapsedSince(StartedKey);
        }

        private static double ElapsedSince(string key)
        {
            string rawTicks = SessionState.GetString(key, "0");
            return long.TryParse(rawTicks, out long ticks)
                ? TimeSpan.FromTicks(DateTime.UtcNow.Ticks - ticks).TotalSeconds
                : TimeoutSeconds + 1d;
        }

        private static void Finish(bool passed, string message)
        {
            if (SessionState.GetInt(StageKey, 0) == 99)
            {
                return;
            }

            SessionState.SetInt(StageKey, 99);
            SessionState.SetBool(PassedKey, passed);
            SessionState.SetString(MessageKey, message);
            EditorApplication.update -= VerifyUpdate;
            EditorApplication.ExitPlaymode();
        }
    }
}
#endif
