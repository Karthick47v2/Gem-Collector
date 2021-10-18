using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{
    public void onStartClick(){
        SceneManager.LoadScene("Main");
    }

    public void onExitClick(){
        Application.Quit();
    }
}
