using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UITitle : MonoBehaviour
{
    [SerializeField] private GameObject manual;
    [SerializeField] private TextMeshProUGUI kill;
    [SerializeField] private Score score;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "ending")
        {
            kill = GameObject.Find("kill").GetComponent<TextMeshProUGUI>();
            score = GameObject.Find("ScoreInfo").GetComponent<Score>();
            kill.text = "KILL : " + score.getkillcount();
            score.setkillcount(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() { SceneManager.LoadScene("main"); }

    public void CloseManual() { if (!manual) return; manual.SetActive(false); }
    public void OpenManual() { if (!manual) return; manual.SetActive(true); }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
