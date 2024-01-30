using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace WPP.Deck.UI
{
    public class DeckUIController : MonoBehaviour
    {
        [SerializeField] private DeckEditor _deckEditor;
        [Header("Deck UI")]
        [SerializeField] private GameObject _deckCardGrid;
        [SerializeField] private GameObject _deckCardPopupGrid;
        [SerializeField] private int _defaultDeckCardCount = 8;

        [Header("Collection UI")]
        [SerializeField] private GameObject _collectionCardGrid;
        [SerializeField] private GameObject _collectionCardPopupGrid;

        [Header("Card Prefabs")]
        [SerializeField] private GameObject _cardHolder;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private GameObject _cardPopupPrefab;

        private List<Card> _cardCollection;

        private List<CardUI> _deckCardUIs;
        private List<CardUI> _collectionCardUIs;

        private List<GameObject> _deckPopups;
        private List<GameObject> _collectionPopups;

        private void Awake()
        {
            LoadCards();
        }
        private void Start()
        {
            _deckEditor.OnDeckChanged += SetCards;
            InitializeGrid();
            _deckEditor.LoadDeck();
            _deckEditor.SelectDeck(0);
        }

        public void LoadCards()
        {
            _cardCollection = new List<Card>();
            for (int i = 0; i < 10; i++)
            {
                Card card = new Card();
                card.id = "card_" + i;
                _cardCollection.Add(card);
            }
        }

        public void InitializeGrid()
        {
            foreach(Transform child in _deckCardGrid.transform)
                Destroy(child.gameObject);

            foreach (Transform child in _deckCardPopupGrid.transform)
                Destroy(child.gameObject);

            foreach (Transform child in _collectionCardGrid.transform)
                Destroy(child.gameObject);

            foreach (Transform child in _collectionCardPopupGrid.transform)
                Destroy(child.gameObject);

            // 1. Instantiate card prefabs

            _deckCardUIs = new();
            _deckPopups = new();
            InstatiateCard(_deckCardGrid.transform, _deckCardPopupGrid.transform,
                _deckCardUIs, _deckPopups, _defaultDeckCardCount, true);

            _collectionCardUIs = new();
            _collectionPopups = new();
            InstatiateCard(_collectionCardGrid.transform, _collectionCardPopupGrid.transform,
                _collectionCardUIs, _collectionPopups, _cardCollection.Count, false);

            void InstatiateCard(Transform cardGrid, Transform popupGrid, List<CardUI> cardUIs, List<GameObject> popups, int count, bool isDeckCard)
            {
                for (int i = 0; i < count; i++) {
                    GameObject cardHolder = Instantiate(_cardHolder, cardGrid);
                    GameObject card = Instantiate(_cardPrefab, cardHolder.transform);

                    GameObject cardPopupHolder = Instantiate(_cardHolder, popupGrid);
                    GameObject cardPopup = Instantiate(_cardPopupPrefab, cardPopupHolder.transform);

                    cardUIs.Add(card.GetComponentInChildren<CardUI>());
                    popups.Add(cardPopup);

                    card.GetComponentInChildren<CardUI>().Initialize(this, cardPopup, i);
                    cardPopup.GetComponentInChildren<CardUI>().Initialize(this, cardPopup, i);

                    cardPopup.GetComponentInChildren<CardPopupUI>().Initialize(this, i, isDeckCard);

                    cardPopup.SetActive(false);
                }
            }
        }

        public void TurnAllPopupsOff()
        {
            foreach (GameObject popup in _deckPopups)
                popup.SetActive(false);

            foreach (GameObject popup in _collectionPopups)
                popup.SetActive(false);
        }

        public void AddCardToDeck(int index)
        {
            _deckEditor.AddCard(_cardCollection[index]);
            TurnAllPopupsOff();
        }

        public void RemoveCardFromDeck(int index)
        {
            _deckEditor.RemoveCard(index);
            TurnAllPopupsOff();
        }
    
        public void SetCards(Deck deck)
        {
            for (int i = 0; i < deck.CardId.Count; i++)
            {
                _deckCardUIs[i].SetCard(deck.CardId[i]);
                _deckPopups[i].GetComponentInChildren<CardUI>().SetCard(deck.CardId[i]);

            }

            for (int i = 0; i < _cardCollection.Count; i++)
            {
                _collectionCardUIs[i].SetCard(_cardCollection[i].id);
                _collectionPopups[i].GetComponentInChildren<CardUI>().SetCard(_cardCollection[i].id);
            }

            StringBuilder sb = new StringBuilder();
            foreach (var id in deck.CardId)
            {
                sb.Append(id);
                sb.Append(" ");
            }
            Debug.Log(sb.ToString());
        }
    
    }
}
