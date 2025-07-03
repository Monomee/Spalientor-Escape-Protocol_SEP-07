using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextMapScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextMap();
        }
    }
    void LoadNextMap()
    {
        SceneManager.LoadScene("Map2");
    }
}
