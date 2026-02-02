using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public List<CardController> revealedCards = new List<CardController>();

    private bool isChecking = false;

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
}
