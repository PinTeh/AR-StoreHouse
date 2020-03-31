using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecognitionController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mapShow;
    public GameObject imageListShow;
    public bool isMapShow = false;
    public bool isImageListShow = false;

    void Start()
    {
        mapShow.SetActive(isMapShow);
        imageListShow.SetActive(isImageListShow);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMapShowClick()
    {
        isMapShow = !isMapShow;
        mapShow.SetActive(isMapShow);
    }

    public void OnImageListShowClick()
    {
        isImageListShow = !isImageListShow;
        imageListShow.SetActive(isImageListShow);
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
