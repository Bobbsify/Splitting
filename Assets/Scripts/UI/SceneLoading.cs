using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    [SerializeField] private Image _progressBar;

    int scene;

    void Start()
    {
        scene = GameObject.Find("sceneLoadingInfo").GetComponent<SceneToLoad>().value;
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        //create async operation, quando la scena sarà caricata verrà caricata automaticamente
        AsyncOperation gameLevelLoading = SceneManager.LoadSceneAsync(scene);

        while (gameLevelLoading.progress < 1)
        {
            //progress bar fill = async operation progress
            _progressBar.fillAmount = gameLevelLoading.progress;
            yield return new WaitForEndOfFrame();
        }

        //load game scene when finished
        yield return new WaitForEndOfFrame();
    }
}
