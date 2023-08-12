using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void Btn1()
    {
        SceneManager.LoadScene(1);
    }

    public void Btn2()
    {
        SceneManager.LoadScene(2);
    }
}
