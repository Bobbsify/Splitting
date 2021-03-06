using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    [CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 1)]
    public class Settings : ScriptableObject
    {
        public GameSettings currentSettings;
    }

    [Serializable]
    public class GameSettings
    {
        public Resolution resolution;

        [Header("Sound")]
        public float masterVolume;
        public float musicVolume;
        public float soundEffectsVolume;

        [Header("Video")]
        public bool filmGrain;
        public float gamma;
    }
}