using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewSceneLoad : MonoBehaviour
{
    [SerializeField]
    string newScene;
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(newScene);
    }
}
