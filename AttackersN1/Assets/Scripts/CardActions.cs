using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardActions : MonoBehaviour
{
    public static event EventHandler<EventArgs_OnCardSelected> OnCardSelected;

    public class EventArgs_OnCardSelected : EventArgs
    {
        public Card card;
    }

    [SerializeField] private Card _card;
    [SerializeField] private TMP_Text _manaCostText;

    private Button button;

    private void Awake() => button = GetComponent<Button>();

    private void Start() => _manaCostText.SetText(_card.manaCost.ToString());

    private void OnEnable() => button.onClick.AddListener(CardSelected);

    private void OnDisable() => button.onClick.RemoveListener(CardSelected);

    private void CardSelected() => OnCardSelected?.Invoke(this, new EventArgs_OnCardSelected { card = _card });
}