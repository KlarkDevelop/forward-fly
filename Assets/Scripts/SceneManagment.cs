using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{   
    public void LoadScene(int index)
    {
        SaveSystem.Instante.DoSaveData();
        SceneManager.LoadScene(index);
    } 

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        SaveSystem.Instante.DoSaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
