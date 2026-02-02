using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject PlayPanel;
    public GameObject StartPanel;

    public TMP_Text movesText;
    public TMP_Text matchesText;
    public GameObject winPanel;

    private void Awake()
    {
        if (Instance == null)
        Instance = this;
        else Destroy(Instance);
    }
    // Start is called before the first frame update
    void Start()
    {
        StarGame();
    }
    public void Play()
    {
        PlayPanel.SetActive(true);
        StartPanel.SetActive(false);
    }
    public void StarGame()
    {
        PlayPanel.SetActive(false);
        StartPanel.SetActive(true);
    }
   
    public void UpdateMoves(int moves)
    {
        movesText.text = moves.ToString();
    }

    public void UpdateMatches(int matches)
    {
        matchesText.text = matches.ToString();
    }

    public void ShowWin()
    {
        winPanel.SetActive(true);
    }
}
