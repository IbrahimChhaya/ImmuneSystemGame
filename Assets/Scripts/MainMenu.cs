using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject hammingOn;
    public GameObject hammingOff;

    public GameObject levenshteinOn;
    public GameObject levenshteinOff;

    public GameObject unnamedOn;
    public GameObject unnamedOff;

    public GameObject mainMenu;
    public GameObject optionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void enableHamming()
    {
        if(!hammingOn.activeSelf)
        {
            hammingOn.SetActive(true);
            hammingOff.SetActive(false);

            levenshteinOn.SetActive(false);
            levenshteinOff.SetActive(true);

            unnamedOn.SetActive(false);
            unnamedOff.SetActive(true);
        }
    }

    public void enableLevenshtein()
    {
        if (!levenshteinOn.activeSelf)
        {
            levenshteinOn.SetActive(true);
            levenshteinOff.SetActive(false);

            hammingOn.SetActive(false);
            hammingOff.SetActive(true);

            unnamedOn.SetActive(false);
            unnamedOff.SetActive(true);
        }
    }

    public void enableUnnamed()
    {
        if (!unnamedOn.activeSelf)
        {
            unnamedOn.SetActive(true);
            unnamedOff.SetActive(false);

            hammingOn.SetActive(false);
            hammingOff.SetActive(true);

            levenshteinOn.SetActive(false);
            levenshteinOff.SetActive(true);
        }
    }

    public void BackOptions()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
