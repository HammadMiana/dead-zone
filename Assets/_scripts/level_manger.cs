using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class level_manger : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    void Start()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
    }
    public void NextPanel()
    {
        panel1.SetActive(false);
        panel2.SetActive(true);
    }
    public void BackPanel()
    {
        panel2.SetActive(false);
        panel1.SetActive(true);
    }
    public void BackTOMenu()
    {
        SceneManager.LoadScene("reload");
    }
    public void level1()
    {
        SceneManager.LoadScene("level1");
    }
    public void level2()
    {
        SceneManager.LoadScene("level2");
    }
    public void level3()
    {
        SceneManager.LoadScene("level3");
    }
    public void level4()
    {
        SceneManager.LoadScene("level4");
    }
    public void level5()
    {
        SceneManager.LoadScene("level5");
    }
    public void level6()
    {
        SceneManager.LoadScene("level6");
    }
    public void level7()
    {
        SceneManager.LoadScene("level7");
    }
    public void level8()
    {
        SceneManager.LoadScene("level8");
    }
}
