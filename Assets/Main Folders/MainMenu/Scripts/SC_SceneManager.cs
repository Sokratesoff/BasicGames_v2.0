using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_SceneManager : MonoBehaviour
{
  private bool isPaused = false;
  [SerializeField]private GameObject breakMenuUI, pauseButton;

  void Start(){
    DontDestroyOnLoad(breakMenuUI.transform.parent);
    breakMenuUI.SetActive(false);
    
    if(isPaused)
      TogglePause();
  }

  void Update(){
    if(SceneManager.GetActiveScene().name == "MainMenu" && (breakMenuUI.activeSelf == true || pauseButton.activeSelf == true)){
      breakMenuUI.SetActive(false);
      pauseButton.SetActive(false);
    }
    else if(pauseButton.activeSelf == false && SceneManager.GetActiveScene().name != "MainMenu")
      pauseButton.SetActive(true);

    if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu"){
      TogglePause();
    }
  }

  public void TogglePause(){
    if (isPaused){
      breakMenuUI.SetActive(false);
      Time.timeScale = 1f;
    }else{
      breakMenuUI.SetActive(true);
      Time.timeScale = 0f;
    }

    isPaused = !isPaused;
  }

  public static void ChangeScene(string sceneName){
    if(sceneName == "CurrentScene"){
      SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
      return;
    }

    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
  }
}
