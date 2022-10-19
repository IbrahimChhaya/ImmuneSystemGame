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
    public GameObject playMenu;
    public GameObject dashMenu;

    public TMP_Text stringLength;
    public Slider slider;

    public GameObject dungeonButton;
    public GameObject bridgeButton;
    public GameObject dashButton;
    public TextMeshProUGUI dashText;

    private Image dungeonImage;
    private Image bridgeImage;

    private string activeMap;

    //public Button playButton;
    public GameObject PlayButton;
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
            stringLength.text = length.ToString() + " chars";
            slider.value = length;
        }

        dungeonImage = dungeonButton.GetComponent<Image>();
        bridgeImage = bridgeButton.GetComponent<Image>();

        var dungeonIteration = PlayerPrefs.GetInt("DungeonIterations");
        var bridgeIteration = PlayerPrefs.GetInt("BridgeIterations");
        if (dungeonIteration == 0 && bridgeIteration == 0)
        {
            dashText.color = new Color(200, 200, 200);
            dashButton.GetComponent<Button>().interactable = false;
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


    public void Dashboard()
    {
        dashMenu.SetActive(true);
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
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void BackPlay()
    {
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
    }

    public void BackDash()
    {
        mainMenu.SetActive(true);
        dashMenu.SetActive(false);
    }

    public void SelectMap()
    {
        gameObject.SetActive(false);
        playMenu.SetActive(true);
    }

    public void PlayGame()
    {
        if (activeMap == "Dungeon")
        {
            //load dungeon
            SceneManager.LoadScene("DungeonScene");
        }
        else if (activeMap == "Bridge")
        {
            //create a bridge scene
            SceneManager.LoadScene("BridgeScene");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DungeonButtonPress()
    {
        //set to white
        dungeonImage.color = new Color32(255, 255, 255, 255);
        //set to default
        bridgeImage.color = new Color32(38, 35, 35, 255);

        activeMap = "Dungeon";

        //activate the button by making it interactable and by changing its text colour
        PlayButton.GetComponent<Image>().color = Color.white;
        Button playButton = PlayButton.GetComponent<Button>();
        playButton.interactable = true;
        TMP_Text playText = playButton.GetComponentInChildren<TMP_Text>();
        playText.color = Color.white;

    }

    public void BridgeButtonPress()
    {
        //set to white
        bridgeImage.color = new Color32(255, 255, 255, 255);
        //set to default
        dungeonImage.color = new Color32(38, 35, 35, 255);

        activeMap = "Bridge";

        //activate the button by making it interactable and by changing its text colour
        PlayButton.GetComponent<Image>().color = Color.white;
        Button playButton = PlayButton.GetComponent<Button>();
        playButton.interactable = true;
        TMP_Text playText = playButton.GetComponentInChildren<TMP_Text>();
        playText.color = Color.white;
    }
}
