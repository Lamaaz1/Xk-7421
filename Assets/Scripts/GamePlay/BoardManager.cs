using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DifficultyData
{
    public string name;
    public int rows;
    public int cols;
}

public class BoardManager : MonoBehaviour
{
    [Header("Board Setup")]
    public CardController cardPrefab;
    public Transform boardParent;
    public List<Sprite> cardSprites;

    [Header("Difficulty")]
    public List<DifficultyData> difficulties;
    public int selectedDifficulty = 0;

    private int rows;
    private int cols;

    private List<CardController> revealedCards = new List<CardController>();
    private bool isChecking = false;

    // called by Play button
    public void StartGame()
    {
        ClearBoard();
        ApplyDifficulty();
        GenerateBoard();
        UIManager.Instance.Play();
    }

    // called by difficulty buttons
    public void SelectDifficulty(int index)
    {
        selectedDifficulty = index;
    }

    private void ApplyDifficulty()
    {
        DifficultyData d = difficulties[selectedDifficulty];
        rows = d.rows;
        cols = d.cols;
    }

    private void GenerateBoard()
    {
        int total = rows * cols;

        List<int> ids = new List<int>();

        for (int i = 0; i < total / 2; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        Shuffle(ids);

        for (int i = 0; i < total; i++)
        {
            CardController card = Instantiate(cardPrefab, boardParent);

            int id = ids[i];
            Sprite sprite = cardSprites[id];

            card.board = this;
            card.Setup(id, sprite);
        }
    }

    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[r];
            list[r] = temp;
        }
    }

    public void OnCardRevealed(CardController card)
    {
        if (isChecking)
            return;

        revealedCards.Add(card);

        if (revealedCards.Count >= 2)
        {
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        isChecking = true;

        yield return new WaitForSeconds(0.5f);

        CardController a = revealedCards[0];
        CardController b = revealedCards[1];

        if (a.CardId != b.CardId)
        {
            a.Hide();
            b.Hide();
        }

        revealedCards.RemoveRange(0, 2);

        isChecking = false;
    }

    private void ClearBoard()
    {
        foreach (Transform child in boardParent)
        {
            Destroy(child.gameObject);
        }

        revealedCards.Clear();
    }
}
