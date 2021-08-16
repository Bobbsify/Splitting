using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Splitting
{ 
    public class SettingsMenu : MonoBehaviour
    {
        //public Settings settings;
        public AudioMixer mixer;

        /*
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
         */

        public void SetMasterVolume(float volume)
        {
            mixer.SetFloat("Master", volume);
        }

        public void SetMusicVolume(float volume)
        {
            mixer.SetFloat("Music", volume);
        }

        public void SetSoundEffectsVolume(float volume)
        {
            mixer.SetFloat("Sound Effects", volume);
        }
        
    }
}