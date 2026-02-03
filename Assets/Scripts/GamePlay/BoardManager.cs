using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Scoring")]
    public int moves = 0;
    public int matchesFound = 0;
    private int totalPairs;
 


    private int rows;
    private int cols;

    public GridLayoutGroup grid;

    private Queue<CardController> revealedQueue = new Queue<CardController>();
    private bool isChecking = false;
    public float previewTime = 1.5f;

    // called by Play button
    public void StartGame()
    {
        ClearBoard();
        ApplyDifficulty();
        UpdateGridSize();
        GenerateBoard();
        StartCoroutine(PreviewCards());
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
        int pairs = total / 2;

        if (cardSprites.Count == 0)
        {
            Debug.LogError("No sprites assigned!");
            return;
        }

        List<int> ids = new List<int>();

        for (int i = 0; i < pairs; i++)
        {
            int spriteIndex = i % cardSprites.Count;

            ids.Add(spriteIndex);
            ids.Add(spriteIndex);
        }

        Shuffle(ids);

        for (int i = 0; i < total; i++)
        {
            CardController card = Instantiate(cardPrefab, boardParent);

            int safeIndex = ids[i] % cardSprites.Count; // ?? protection
            Sprite sprite = cardSprites[safeIndex];

            card.board = this;
            card.Setup(safeIndex, sprite);
        }
        totalPairs = pairs;
        matchesFound = 0;
       
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
        if (revealedQueue.Contains(card))
            return;

        revealedQueue.Enqueue(card);

        if (!isChecking)
            StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        isChecking = true;

        while (revealedQueue.Count >= 2)
        {
            CardController a = revealedQueue.Dequeue();
            CardController b = revealedQueue.Dequeue();

            yield return new WaitForSeconds(0.5f);

            moves++;
            UIManager.Instance.UpdateMoves(moves);

            if (a.CardId == b.CardId)
            {
                matchesFound++;
                UIManager.Instance.UpdateMatches(matchesFound);
                a.GetComponent<CanvasGroup>().alpha = 0;
                b.GetComponent<CanvasGroup>().alpha = 0;

                CheckWin();
            }
            else
            {
                a.Hide();
                b.Hide();
            }
        }

        isChecking = false;
    }

 
    private void CheckWin()
    {
        if (matchesFound >= totalPairs)
        {
            UIManager.Instance.ShowWin();
            Debug.Log("You win!" );
        }
    }

    private void ClearBoard()
    {
        foreach (Transform child in boardParent)
        {
            Destroy(child.gameObject);
        }

        revealedQueue.Clear();
    }

    private void UpdateGridSize()
    {
        int totalCards = rows * cols;

        List<(int columns, int rows)> possibleGrids = new List<(int columns, int rows)>();

        for (int i = 2; i <= totalCards; i++)
        {
            if (totalCards % i == 0)
            {
                int columns = i;
                int r = totalCards / i;

                if (r >= 2 && columns >= r)
                    possibleGrids.Add((columns, r));
            }
        }

        var best = possibleGrids[0];

        foreach (var g in possibleGrids)
            if (g.columns < best.columns)
                best = g;

        int bestColumns = best.columns;
        int bestRows = best.rows;

        RectTransform rt = boardParent.GetComponent<RectTransform>();

        float panelWidth = rt.rect.width;
        float panelHeight = rt.rect.height;

        float padLeft = grid.padding.left;
        float padRight = grid.padding.right;
        float padTop = grid.padding.top;
        float padBottom = grid.padding.bottom;

        float spacingX = grid.spacing.x;
        float spacingY = grid.spacing.y;

        float usableWidth = panelWidth - padLeft - padRight;
        float usableHeight = panelHeight - padTop - padBottom;

        float cellWidth = (usableWidth - spacingX * (bestColumns - 1)) / bestColumns;
        float cellHeight = (usableHeight - spacingY * (bestRows - 1)) / bestRows;

        float size = Mathf.Min(cellWidth, cellHeight);

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = bestColumns;
        grid.cellSize = new Vector2(size, size);
    }

    private IEnumerator PreviewCards()
    {
        yield return new WaitForSeconds(.3f);
        // reveal all
        foreach (Transform child in boardParent)
        {
            CardController c = child.GetComponent<CardController>();
            c.RevealInstant();
        }

        yield return new WaitForSeconds(previewTime);

        // hide all
        foreach (Transform child in boardParent)
        {
            CardController c = child.GetComponent<CardController>();
            c.HideInstant();
        }
    }
    public void ResetGame()
    {
        ClearBoard();

        moves = 0;
        matchesFound = 0;

        revealedQueue.Clear();

        UIManager.Instance.UpdateMoves(0);
        UIManager.Instance.UpdateMatches(0);
    }


}
