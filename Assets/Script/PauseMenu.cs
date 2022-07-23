using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] int noOfCharacter;
    public static PauseMenu instance;
    int holdOriginalCharacterCount;
    int noOfcharacterCubes;
    bool loose;


    [Header("Panels")]
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject failPanels;

    [SerializeField] TextMeshProUGUI levelText;
    // Start is called before the first frame update
    private void Start()
    {
        loose = false;
        instance = this; 
        winPanel.SetActive(false);
        failPanels.SetActive(false);

        noOfcharacterCubes = GameObject.FindGameObjectsWithTag("insidebox").Length;
        holdOriginalCharacterCount = noOfCharacter;

        if (PlayerPrefs.GetInt("Index") == SceneManager.sceneCountInBuildSettings)
        {
            levelText.text = (PlayerPrefs.GetInt("IndexNo")).ToString();
        }
    }
    public void MoveNextLvl()
    {
        if (PlayerPrefs.GetInt("Index") == SceneManager.sceneCountInBuildSettings)
        {
            PlayerPrefs.SetInt("IndexNo", (PlayerPrefs.GetInt("IndexNo") + 1));//save index no for looping;
            SceneManager.LoadScene("FirstScene");
        }
        if (PlayerPrefs.GetInt("Index") < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            //save level index
            PlayerPrefs.SetInt("Index", (SceneManager.GetActiveScene().buildIndex) + 1);
            PlayerPrefs.SetInt("IndexNo", SceneManager.GetActiveScene().buildIndex);//save index no for looping;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CountNoCharacterReached()
    {
        noOfCharacter--;
        if(noOfCharacter<=0)
        {
            // call win panel
           
            Invoke("WinPanel", 0f);
            AudioManager.instance.Play("win");
        }
    }

    public void WinPanel()
    {
        winPanel.SetActive(true);
    }
    
    public void LooseCheck()
    {

        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        //check if the cube with character is destroyed without spawnning any charcter before disabling
        var count = noOfcharacterCubes - GameObject.FindGameObjectsWithTag("insidebox").Length;
        var temp =  GameObject.FindGameObjectsWithTag("insidebox").Length + GameObject.FindGameObjectsWithTag("character").Length;
  
        if (temp < holdOriginalCharacterCount)
        {
            LooseCondition();
        }
    }
    
    public void LooseCondition()
    {
        if (!loose)
        {
            //call loose panel
            AudioManager.instance.Play("fail");
            failPanels.SetActive(true);   //fail panel
            loose = true;
        }
    }

    public void PressedBomb() //loose condition
    {
        Invoke("Restart", 1.5f);
    }
}
