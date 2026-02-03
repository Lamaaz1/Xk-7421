using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject PlayPanel;
    public GameObject StartPanel;

    public TMP_Text movesText;
    public TMP_Text matchesText;
    public GameObject winPanel;

    public RectTransform restartButton;
    public Toggle[] levelToggles;


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
        winPanel.SetActive(false);
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
        AnimateWinButton(restartButton);
    }
    public void AnimateWinButton(RectTransform btn)
    {
        StartCoroutine(Pop(btn));
    }

    private IEnumerator Pop(RectTransform t)
    {
        Vector3 start = Vector3.one;
        Vector3 big = Vector3.one * 1.2f;

        float time = 0;

        while (time < 0.15f)
        {
            time += Time.deltaTime;
            t.localScale = Vector3.Lerp(start, big, time / 0.15f);
            yield return null;
        }

        time = 0;

        while (time < 0.15f)
        {
            time += Time.deltaTime;
            t.localScale = Vector3.Lerp(big, start, time / 0.15f);
            yield return null;
        }
    }
    public void NextLevel()
    {
        PlayPanel.SetActive(true);
        StartPanel.SetActive(false);
        winPanel.SetActive(false);
    }
    public void SetLevelToggle(int index)
    {
        for (int i = 0; i < levelToggles.Length; i++)
            levelToggles[i].isOn = (i == index);
    }

}
