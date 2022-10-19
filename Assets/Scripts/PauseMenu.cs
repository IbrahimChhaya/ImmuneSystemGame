using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {

    }

    public void QuitToMenu()
    {
        Debug.Log("ddwad");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }
}
