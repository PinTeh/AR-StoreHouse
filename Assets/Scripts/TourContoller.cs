using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TourContoller : MonoBehaviour
{

    public GameObject eiffelTower;
    public GameObject duLouvre;
    public GameObject loadScreen;
    public Slider slider;
    public Text loadText;
    public static string defaultSelect = "eiffelTower";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnEiffelTowerClick()
    {
        defaultSelect = "eiffelTower";
        StartCoroutine(LoadScene());
    }

    public void OnDuLouvreClick()
    {
        defaultSelect = "zyds";
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {

        loadScreen.SetActive(true);
        yield return new WaitForSeconds(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync("preview");

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            slider.value = operation.progress;
            loadText.text = operation.progress * 100 + "%";
            
            if(operation.progress >= 0.9f)
            {
                slider.value = 1;

                //loadText.text = "press anykey to continue";
                //if (Input.anyKeyDown)
                //{
                //    operation.allowSceneActivation = true;
                //}

                operation.allowSceneActivation = true;

            }

            yield return null;
        }
    }

}
