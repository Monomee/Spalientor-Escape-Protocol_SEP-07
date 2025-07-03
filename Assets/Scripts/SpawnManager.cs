using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    Transform spawnPoint;
    GameObject enemy;
    public GameObject enemyPrefab;
    public int numberOfEnemies = 10;
    public List<GameObject> pooledEnemy;
    public List<Transform> patrolPoints;

    private void FixedUpdate()
    {
        enemy = GetPooledObject();
        if (enemy == null)
        {
            return;
        }
        spawnPoint = patrolPoints[Random.Range(0, patrolPoints.Count)];
        enemy.transform.position = spawnPoint.position;
        enemy.SetActive(true);
        enemy.GetComponent<BaseEnemyController>().ResetState();
        enemy.GetComponent<BaseCharacter>().isAlive = true;
        enemy.GetComponent<BaseCharacter>().isDead = false;         
    }
    private void Start()
    {
        pooledEnemy = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            tmp = Instantiate(enemyPrefab);
            tmp.SetActive(false);
            pooledEnemy.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (!pooledEnemy[i].activeInHierarchy)
            {
                return pooledEnemy[i];
            }
        }
        return null;
    }
}
