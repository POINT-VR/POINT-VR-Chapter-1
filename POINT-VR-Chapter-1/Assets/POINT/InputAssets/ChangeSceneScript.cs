using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DevLocker.Utils;

public class ChangeSceneScript : MonoBehaviour
{
    [Tooltip("Transition to this scene")]
    [SerializeField] private SceneReference scene;
    [SerializeField] GameObject player;

    public void UIChangeScene()
    {
        //Ensure scene and player are set
        if (scene != null && player != null) 
        {
            SceneManager.LoadScene(scene.SceneName);

            //Solves issue where loading the scene puts the player in a pause state
            if (player != null && player.TryGetComponent(out PauseController pauseController))
            {
                pauseController.Unpause();
            }
        }
 
    }
}
