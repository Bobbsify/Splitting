using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{ 
    public class SettingsMenu : MonoBehaviour
    {
        public Settings settings;

        public void SetMasterVolume(float volume)
        {
            settings.currentSettings.masterVolume = volume;
        }

        public void SetMusicVolume(float volume)
        {
            settings.currentSettings.musicVolume = volume;
        }

        public void SetSoundEffectsVolume(float volume)
        {
            settings.currentSettings.soundEffectsVolume = volume;
        }
    }
}