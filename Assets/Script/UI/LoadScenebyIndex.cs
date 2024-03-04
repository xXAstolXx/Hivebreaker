using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenebyIndex : MonoBehaviour
{
    [SerializeField]
    private GameObject popUp;
    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void OnQuitButtonClicked()
    {
        popUp.SetActive(true);
    }

    public void OnBackBtnClicked()
    {
        popUp.SetActive(false);
    }
}
