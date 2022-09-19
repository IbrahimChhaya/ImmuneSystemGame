using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject hammingOn;
    public GameObject hammingOff;

    public GameObject levenshteinOn;
    public GameObject levenshteinOff;

    public GameObject jaroOn;
    public GameObject jaroOff;

    public GameObject mainMenu;
    public GameObject optionsMenu;

    public TMP_Text stringLength;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("activeDistance"))
        {
            string activeDistance = PlayerPrefs.GetString("activeDistance");
            switch(activeDistance)
            {
                case "H":
                    hammingHelper();
                    break;
                case "L":
                    levenshteinHelper();
                    break;
                case "J":
                    jaroHelper();
                    break;
            }
        }
        if(PlayerPrefs.HasKey("signatureLength"))
        {
            int length = PlayerPrefs.GetInt("signatureLength");
            stringLength.text = length.ToString();
            slider.value = length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void lengthUpdate(float value)
    {
        int length = Mathf.RoundToInt(value);
        stringLength.text = length + " chars";
        PlayerPrefs.SetInt("signatureLength", length);
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void hammingHelper()
    {
        hammingOn.SetActive(true);
        hammingOff.SetActive(false);

        levenshteinOn.SetActive(false);
        levenshteinOff.SetActive(true);

        jaroOn.SetActive(false);
        jaroOff.SetActive(true);
    }

    public void enableHamming()
    {
        if(!hammingOn.activeSelf)
        {
            hammingHelper();
            PlayerPrefs.SetString("activeDistance", "H");
        }
    }

    private void levenshteinHelper()
    {
        levenshteinOn.SetActive(true);
        //EnemyController.activeDistance = "L";
        levenshteinOff.SetActive(false);

        hammingOn.SetActive(false);
        hammingOff.SetActive(true);

        jaroOn.SetActive(false);
        jaroOff.SetActive(true);
    }

    public void enableLevenshtein()
    {
        if (!levenshteinOn.activeSelf)
        {
            levenshteinHelper();
            PlayerPrefs.SetString("activeDistance", "L");
        }
    }

    private void jaroHelper()
    {
        jaroOn.SetActive(true);
        jaroOff.SetActive(false);

        hammingOn.SetActive(false);
        hammingOff.SetActive(true);

        levenshteinOn.SetActive(false);
        levenshteinOff.SetActive(true);
    }

    public void enableJaro()
    {
        if (!jaroOn.activeSelf)
        {
            jaroHelper();
            PlayerPrefs.SetString("activeDistance", "J");
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
