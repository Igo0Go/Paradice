using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject settingPanel;

    private AsyncOperation _asyncOperation;

    // Use this for initialization
    void Start()
    {
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        Time.timeScale = 1;
        _asyncOperation = SceneManager.LoadSceneAsync("LOADING!!");
        _asyncOperation.allowSceneActivation = false;
        LoadLevel.name = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf)
            {
                ContinueButtonClick();
            }
            if (settingPanel.activeSelf)
            {
                BackSettingButtonClick();
            }
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ContinueButtonClick()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RetryButtonClick()
    {
        LoadLevel.name = SceneManager.GetActiveScene().name;
        _asyncOperation.allowSceneActivation = true;
        Time.timeScale = 1;
    }

    public void SettingButtonClick()
    {
        pausePanel.SetActive(false);
        settingPanel.SetActive(true);
    }

    public void ExidButtonClick()
    {
        LoadLevel.name = "Menu";
        _asyncOperation.allowSceneActivation = true;
        Time.timeScale = 1;
    }

    public void BackSettingButtonClick()
    {
        pausePanel.SetActive(true);
        settingPanel.SetActive(false);
    }
}
