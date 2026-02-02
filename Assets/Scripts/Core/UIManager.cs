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
        PlayPanel.SetActive(false);
        StartPanel.SetActive(true);
    }
    public void Play()
    {
        PlayPanel.SetActive(true);
        StartPanel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
