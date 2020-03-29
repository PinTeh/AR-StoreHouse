using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject info;
    public GameObject backPanel;
    public GameObject quitConfirmPanel;
    private bool infoActive = false;
    private bool confirmActive = false;
    // Start is called before the first frame update
    void Start()
    {
        info.SetActive(infoActive);
        quitConfirmPanel.SetActive(confirmActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape)) // 返回键
        {
            QuitListen();
        }
    }

    public void OnRecognitionClick()
    {
        SceneManager.LoadScene("ObjectRecognition");
    }

    public void OnDiscoverClick()
    {
        SceneManager.LoadScene("Travel");
    }

    public void OnNavigationClick()
    {
        SceneManager.LoadScene("GroudTest");
    }

    public void OnInfoClick()
    {
        infoActive = !infoActive;
        info.SetActive(infoActive);
        backPanel.SetActive(!infoActive);
    }

    public void QuitListen()
    {
        confirmActive = !confirmActive;
        quitConfirmPanel.SetActive(confirmActive);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
