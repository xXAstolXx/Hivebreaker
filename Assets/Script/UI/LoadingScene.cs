using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private Image progressFillBar;

    private Scene currentScene;

    private AsyncOperation loadingOperation = null;

    private bool isLoading = false;

    // Start is called before the first frame update
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        loadingOperation = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        //loadingOperation.allowSceneActivation = false;
        isLoading = true;
    }

    private void Update()
    {
        if(loadingOperation == null || isLoading == false)
        {
            return;
        }

        if(!loadingOperation.isDone)
        {
            progressFillBar.fillAmount = loadingOperation.progress;
        }

        if(loadingOperation.isDone)
        {
            isLoading = false;
            progressFillBar.fillAmount = 1;
            //loadingOperation.allowSceneActivation = true;
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }
}