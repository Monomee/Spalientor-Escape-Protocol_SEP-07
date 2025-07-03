using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SpawnManager spawnManager;

    private void OnEnable()
    {
        Instance = this;
    }
    private void OnDisable()
    {
        Instance = null;
    }
    
    public Vector3 GetNextPoint()
    {
        return spawnManager.patrolPoints[Random.Range(0, spawnManager.patrolPoints.Count)].position;
    }

    public Gun FindGunInChild(Transform parent)
    {
        if (parent == null)
        {
            return null;
        }

        Gun[] guns = parent.GetComponentsInChildren<Gun>(true);

        if (guns.Length > 0)
        {
            return guns[0];
        }

        return null;
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMenu() { 
        SceneManager.LoadScene("Menu");
    }
}
