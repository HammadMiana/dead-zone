using UnityEngine;
using UnityEngine.UI;
 
using System.Collections.Generic;

public class EnemyCountManager : MonoBehaviour
{
    public static EnemyCountManager Instance;

    public Text enemyCountText;

    private List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Scene start par saare enemies auto find
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        enemies.AddRange(foundEnemies);
        UpdateUI();
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            UpdateUI();
        }
    }

    public void UnRegisterEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            UpdateUI();
        }

        if (enemies.Count == 0)
        {
            enemyCountText.text = "All Enemies Eliminated!";
        }
    }

    void UpdateUI()
    {
        enemyCountText.text = "Enemies Left: " + enemies.Count;
    }
}
