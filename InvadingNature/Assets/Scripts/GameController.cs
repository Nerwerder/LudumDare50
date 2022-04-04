using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
/// <summary>
/// The GameController is the only class that is allowed to check for player inputs and will distribute those to the right Elements.
/// </summary>
public class GameController : MonoBehaviour
{
    Player player = null;
    FollowCamera playerCamera = null;


    //
    public GameObject mainMenuRoot;
    public GameObject gameOverMenu;


    void Start()
    {
        player = FindObjectOfType<Player>();
        Debug.Assert(player != null, "GameController was not able to find a Player in the Scene");
        playerCamera = FindObjectOfType<FollowCamera>();
        Debug.Assert(playerCamera != null, "GameController was not able to find a Camera in the Scene");
    }

    void Update()
    {
        //PLAYER
        player.Move(Input.GetAxis("Vertical"));
        player.Rotate(Input.mousePosition);
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            player.HighSpeed = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)) {
            player.HighSpeed = false;
        }
        if(Input.GetMouseButtonDown(0)) {
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray, 100f);
            player.Interact(hits);
        }
        if(Input.GetMouseButtonUp(0)) {
            player.StopInteracting();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenuRoot.SetActive(!mainMenuRoot.activeSelf);
        }
        //CAMERA
        playerCamera.Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    public void StartGame() {
        mainMenuRoot.SetActive(false);
        PlantController pc = FindObjectOfType<PlantController>();
        pc.enableFlowers = true;
        pc.enableTrees = true;
    }

    public void GameOver() {
        gameOverMenu.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuidGame()
    {
        Application.Quit();
    }
}