using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
public class DeckManager : MonoBehaviour
{   
    private List<BuildingCard> _drawnCards = new List<BuildingCard>();
    
    public List<BuildingCard> deck = new List<BuildingCard>();
    public Transform[] cardSlots;
    public bool[] availableCardSlots;
    public Transform deckTransform;
    private void Start()
    {
        ReturnDrawnCardsToDeck();
    }
    
    public void ReturnDrawnCardsToDeck()
    {
        ReturnDrawnCardsToDeckAnim();
        DrawCards();
    }
    private void DrawCard()
    {
        if (deck.Count > 0)
        {
            BuildingCard randCard = deck[Random.Range(0, deck.Count)];
  
            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i])
                {
                    randCard.gameObject.SetActive(true);
                    randCard.transform.position = cardSlots[i].position;
                    randCard.gameObject.transform.DOScale(Vector3.zero, .5f).From();
                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    _drawnCards.Add(randCard);
                    return;
                }
            }
        }
    }
    private void DrawCards()
    {
        int cardsToDraw = Mathf.Min(availableCardSlots.Length, deck.Count);
        
        for (int i = 0; i < cardsToDraw; i++)
        {
            availableCardSlots[i] = true;
            DrawCard();
        }        
    }
    private void ReturnDrawnCardsToDeckAnim()
    {
        foreach (var drawnCard in _drawnCards)
        {
            drawnCard.transform.DOMove(deckTransform.transform.position, 0.5f)
                .OnComplete(() =>
                {
                    drawnCard.gameObject.SetActive(false);
                    deck.Add(drawnCard);
                });
        }
        _drawnCards.Clear();
    }
}
