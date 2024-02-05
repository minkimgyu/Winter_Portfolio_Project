using System.Collections.Generic;
using System;

namespace WPP.ClientInfo.Card
{
    [Serializable]
    public class CardsData
    {
        public List<CardData> cards;
        public CardsData()
        {
            cards = new List<CardData>();
        }

        public void AddCard(CardData card)
        {
            cards.Add(card);
        }

        public CardData FindCard(int card_id)
        {
            foreach (CardData card in cards)
            {
                if (card.id == card_id)
                    return card;
            }
            return null;
        }
    }
}