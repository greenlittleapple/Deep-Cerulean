using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneIndex : MonoBehaviour {

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
}
