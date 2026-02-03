using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CardController : MonoBehaviour, IPointerClickHandler
{
    [Header("Visuals")]
    [SerializeField] private Image frontImage;
    [SerializeField] private Image backImage;

    public BoardManager board;

    public int CardId { get; private set; }

    private bool isRevealed = false;
    private bool isAnimating = false;

    public void Setup(int id, Sprite sprite)
    {
        CardId = id;
        frontImage.sprite = sprite;
        HideInstant();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isRevealed || isAnimating)
            return;

        Reveal();
    }

    public void Reveal()
    {
        if (isRevealed) return;
        StartCoroutine(Flip(true));
    }

    public void Hide()
    {
        if (!isRevealed) return;
        StartCoroutine(Flip(false));
    }

    private System.Collections.IEnumerator Flip(bool reveal)
    {
        isAnimating = true;

        float duration = 0.2f;
        float time = 0f;

        Vector3 start = transform.localScale;
        Vector3 mid = new Vector3(0f, start.y, start.z);

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, mid, time / duration);
            yield return null;
        }

        frontImage.gameObject.SetActive(reveal);
        backImage.gameObject.SetActive(!reveal);

        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(mid, start, time / duration);
            yield return null;
        }

        transform.localScale = start;

        isRevealed = reveal;
        isAnimating = false;
        SoundManager.instance.PlayFlipSound();
        board.OnCardRevealed(this);
    }

    public void HideInstant()
    {
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
        isRevealed = false;
    }
    public void RevealInstant()
    {
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
        isRevealed = true;
    }
    public IEnumerator FlipPreview(bool reveal)
    {
        yield return Flip(reveal);
    }



}
