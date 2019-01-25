using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	// Goes to scene described by string
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Exits application
    public void ExitApplication()
    {
        Application.Quit();
    }

    // Disable an object
    public void DisableObject(GameObject objectToDisable)
    {
        objectToDisable.SetActive(false);
    }

}
