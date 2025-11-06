using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Oxide.Core;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;
using UnityEngine;

#if CARBON
using Carbon.Base;
#endif

namespace Oxide.Plugins
{
    [Info("rAlwaysDay", "Ftuoil Xelrash", "1.0.35")]
    public class rAlwaysDay : RustPlugin
    {
        #region Fields

        private static rAlwaysDay _instance;

        private AlwaysDayEngine _engine;

        #endregion

        #region Config

        private static Configuration _config;

        private class Configuration
        {
            [JsonProperty(PropertyName = "Auto-Skip Settings")]
            public AutoSkipSettings AutoSkip = new()
            {
                StartTime = "20:50",
                EndTime = "21:00",
                SetTime = "07:00"
            };

            public VersionNumber Version;
        }

        private class AutoSkipSettings
        {
            [JsonProperty(PropertyName = "Auto-skip start time")]
            public string StartTime;

            [JsonProperty(PropertyName = "Auto-skip end time")]
            public string EndTime;

            [JsonProperty(PropertyName = "Time to set after skip")]
            public string SetTime;

            #region Cache

            [JsonIgnore] public TimeSpan StartTimeSpan;
            [JsonIgnore] public TimeSpan EndTimeSpan;
            [JsonIgnore] public TimeSpan SetTimeSpan;

            public void Init()
            {
                try
                {
                    StartTimeSpan = TimeSpan.Parse(StartTime);
                    EndTimeSpan = TimeSpan.Parse(EndTime);
                    SetTimeSpan = TimeSpan.Parse(SetTime);
                }
                catch (Exception ex)
                {
                    _instance.PrintError($"Invalid time format in configuration: {ex.Message}");
                    _instance.PrintError("Using default time values");
                    StartTimeSpan = TimeSpan.Parse("20:50");
                    EndTimeSpan = TimeSpan.Parse("21:00");
                    SetTimeSpan = TimeSpan.Parse("07:00");
                }
            }

            #endregion
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            try
            {
                _config = Config.ReadObject<Configuration>();

                if (_config == null)
                    throw new Exception();

                ValidateConfig();

                if (_config.Version < Version)
                    UpdateConfigValues();

                SaveConfig();
            }
            catch (Exception ex)
            {
                PrintError("Your configuration file contains an error. Using default configuration values.");
                LoadDefaultConfig();
                Debug.LogException(ex);
            }
        }

        private void ValidateConfig()
        {
            // Simple validation - just ensure we have valid time settings
            // The AutoSkipSettings.Init() method handles time format validation
        }

        protected override void SaveConfig()
        {
            Config.WriteObject(_config);
        }

        protected override void LoadDefaultConfig()
        {
            _config = new Configuration();
        }

        private void UpdateConfigValues()
        {
            _config.Version = Version;
            PrintWarning("Config update completed!");
        }

        #endregion

        #region Hooks

        private void Init()
        {
            _instance = this;
        }

        private void OnServerInitialized()
        {
            _config.AutoSkip.Init();
            InitEngine();
        }


        private void Unload()
        {
            DestroyEngine();
            _instance = null;
            _config = null;
        }

        #endregion

        #region Component

        private class AlwaysDayEngine : FacepunchBehaviour
        {
            #region Fields

            private uint componentSearchAttempts;
            private TOD_Time timeComponent;
            private TimeSpan CurrentTime => TOD_Sky.Instance.Cycle.DateTime.TimeOfDay;

            #endregion

            #region Init

            private void Awake()
            {
                GetTimeComponent();
            }

            private void GetTimeComponent()
            {
                if (TOD_Sky.Instance == null)
                {
                    ++componentSearchAttempts;
                    if (componentSearchAttempts < 50)
                    {
                        Invoke(GetTimeComponent, 3);
                        return;
                    }

                    _instance.PrintError("Failed to find TOD_Sky component after 50 attempts (150 seconds). Plugin will not function.");
                    return;
                }

                timeComponent = TOD_Sky.Instance.Components.Time;

                if (timeComponent == null)
                {
                    _instance.PrintError("Could not fetch time component. Plugin will not work without it.");
                    return;
                }

                // Use simple always-on OnMinute for reliability
                timeComponent.OnMinute += OnMinute;
            }

            #endregion

            #region Main

            public void ForceDayTime()
            {
                // Always instant skip - jump directly to configured time
                var targetTime = new DateTime(
                    TOD_Sky.Instance.Cycle.DateTime.Year,
                    TOD_Sky.Instance.Cycle.DateTime.Month,
                    TOD_Sky.Instance.Cycle.DateTime.Day,
                    0, 0, 0
                ).Add(_config.AutoSkip.SetTimeSpan);

                if (TOD_Sky.Instance.Cycle.DateTime > targetTime)
                    targetTime = targetTime.AddDays(1);

                var hoursToAdd = (targetTime - TOD_Sky.Instance.Cycle.DateTime).TotalHours;
                timeComponent.AddHours((float)hoursToAdd, false);
                
                _instance.Puts($"Night skipped - time set to {_config.AutoSkip.SetTime}");
            }

            public void OnMinute()
            {
                var currentTime = CurrentTime;

                // Check auto-skip window
                if (currentTime >= _config.AutoSkip.StartTimeSpan &&
                    currentTime < _config.AutoSkip.EndTimeSpan)
                {
                    ForceDayTime();
                }
            }

            #endregion

            #region Destroy

            public void Kill()
            {
                DestroyImmediate(this);
            }

            private void OnDestroy()
            {
                CancelInvoke();

                if (timeComponent != null)
                {
                    timeComponent.OnMinute -= OnMinute;
                }

                Destroy(gameObject);
                Destroy(this);
            }

            #endregion
        }

        #endregion

        #region Utils

        private void InitEngine()
        {
            _engine = new GameObject().AddComponent<AlwaysDayEngine>();
        }

        private void DestroyEngine()
        {
            if (_engine != null)
                _engine.Kill();
        }


        #endregion
    }
}