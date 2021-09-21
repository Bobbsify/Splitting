using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Splitting
{ 
    public class SettingsMenu : MonoBehaviour
    {
        //public Settings settings;
        public Settings settingsScriptableObject;

        public AudioMixer mixer;

        public Volume volumeProfile;
        
        public void SetMasterVolume(float volume)
        {
           settingsScriptableObject.currentSettings.masterVolume = volume;
            UpdateVolume("Master", volume);
        }
    
        public void SetMusicVolume(float volume)
        {
           settingsScriptableObject.currentSettings.musicVolume = volume;
            UpdateVolume("Music", volume);
        }
    
        public void SetSoundEffectsVolume(float volume)
        {
            settingsScriptableObject.currentSettings.soundEffectsVolume = volume;
            UpdateVolume("Sound Effects", volume);
        }

        public void SetFilmGrain(bool status)
        {
            settingsScriptableObject.currentSettings.filmGrain = status;
            UpdateFilmGrain(status);
        }

        public void SetGamma(float gamma)
        {
            settingsScriptableObject.currentSettings.gamma = gamma;
            UpdateGamma(gamma);
        }

        private void UpdateVolume(string mixerName, float amount)
        {
           mixer.SetFloat(mixerName, amount);
        }

        private void UpdateGamma(float amount)
        {
            LiftGammaGain gammaComponent;
            volumeProfile.profile.TryGet(out gammaComponent);
            Vector4Parameter gamma = gammaComponent.gamma;
            Vector4 updatedGamma = new Vector4(gamma.value.x, gamma.value.y, gamma.value.z, amount);
            gamma.SetValue(new Vector4Parameter(updatedGamma,gamma.overrideState));
        }

        private void UpdateFilmGrain(bool state)
        {
            FilmGrain filmGrain;
            volumeProfile.profile.TryGet(out filmGrain);
            filmGrain.active = state;
        }

        private void UpdateAll()
        {
            UpdateVolume("Master", settingsScriptableObject.currentSettings.masterVolume);
            UpdateVolume("Music", settingsScriptableObject.currentSettings.musicVolume);
            UpdateVolume("Sound Effects", settingsScriptableObject.currentSettings.soundEffectsVolume);
            UpdateGamma(settingsScriptableObject.currentSettings.gamma);
            UpdateFilmGrain(settingsScriptableObject.currentSettings.filmGrain);
        }

        private void OnLevelWasLoaded(int level)
        {
            GameObject.Find("Global Volume").TryGetComponent(out volumeProfile);
            UpdateAll();
        }



        /*
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

        public void SetGamma(float gamma)
        {
            volumeProfile.GetComponent<LiftGammaGain>().gamma.value = new Vector4(0,0,0,gamma);
        }
        */
    }
}