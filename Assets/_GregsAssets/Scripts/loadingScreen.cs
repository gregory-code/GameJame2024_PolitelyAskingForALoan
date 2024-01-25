using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadingScreen : MonoBehaviour
{
    public Image screen;
    [SerializeField] bool load;

    public void Start()
    { 
        if(load)
            StartCoroutine(Show());
    }

    public void StartLoadScene(string sceneTitle)
    {
        StartCoroutine(Load(sceneTitle));
    }

    public void Update()
    {
        int fill = (load) ? 1 : 0 ;
        screen.fillAmount = Mathf.Lerp(screen.fillAmount, fill, 8 * Time.deltaTime);
    }

    private IEnumerator Show()
    {
        yield return new WaitForSeconds(0.5f);
        load = false;
    }

    private IEnumerator Load(string sceneTitle)
    {
        load = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneTitle);
    }
}
