using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private int killcount;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        killcount = 0;
        spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "main")
        {
            killcount = spawnManager.getKillCount();
        }
    }

    public int getkillcount() { return killcount; }
    public void setkillcount(int x) { killcount = 0; }
}
