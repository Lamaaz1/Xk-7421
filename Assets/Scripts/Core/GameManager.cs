using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BoardManager board;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RestartRound()
    {
        UIManager.Instance.StarGame();
        board.ResetGame();
    }
}
