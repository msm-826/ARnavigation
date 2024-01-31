using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public void LoadScene(int SceneIndex) { 
        SceneManager.LoadScene(SceneIndex, LoadSceneMode.Single);
    }
    
    public void QuitApplication () { 
        Application.Quit();
    }
}