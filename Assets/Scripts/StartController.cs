using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    public GameObject loadScreen;
    public Slider slider;
    public Text loadText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadScene()
    {

        loadScreen.SetActive(true);
        yield return new WaitForSeconds(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync("Menu");

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            slider.value = operation.progress;
            loadText.text = operation.progress * 100 + "%";

            if (operation.progress >= 0.9f)
            {
                slider.value = 1;

                operation.allowSceneActivation = true;

            }

            yield return null;
        }
    }
}
