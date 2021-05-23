using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static GameStatus Instance { get; private set; }

    [SerializeField] private Light _light;
    [SerializeField] private LayerMask _groundCorrect;
    [SerializeField] private LayerMask _groundIncorrect;

    private Card _currentCard;
    private Mouse _mouse;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _mouse = GetComponent<Mouse>();
    }

    private void OnEnable()
    {
        CardActions.OnCardSelected += CardSelected;
    }

    private void OnDisable()
    {
        CardActions.OnCardSelected -= CardSelected;
    }

    private void Update()
    {
        if (_currentCard != null)
        {
            Vector3 mousePoint;

            if (!_light.enabled)
                _light.enabled = true;

            if (_mouse.Aiming(out mousePoint, _groundCorrect))
            {
                _light.color = Color.green;
                _light.transform.position = mousePoint + new Vector3(0f, 0.01f, 0f);
            }

            if(_mouse.Aiming(out mousePoint, _groundIncorrect))
            {
                _light.color = Color.red;
                _light.transform.position = mousePoint + new Vector3(0f, 0.01f, 0f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (_mouse.Aiming(out mousePoint, _groundCorrect))
                {
                    Instantiate(_currentCard.prefab, mousePoint, Quaternion.identity);
                    _currentCard = null;
                }
            }
        }
        else
        {
            if (_light.enabled)
                _light.enabled = false;
        }
    }

    private void CardSelected(object sender, CardActions.EventArgs_OnCardSelected e)
    {
        _currentCard = null;
        _currentCard = e.card;
    }
}
