using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject PlayPanel;
    public GameObject StartPanel;

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
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
