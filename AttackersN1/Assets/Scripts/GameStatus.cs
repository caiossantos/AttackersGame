using System.Collections;
using UnityEngine;
using TMPro;

public class GameStatus : MonoBehaviour
{
    public static GameStatus Instance { get; private set; }

    [SerializeField] private Light _light;
    private string _groundCorrect = "GroundAlly";
    private string _groundIncorrect = "GroundEnemy";

    private Card _currentCard;
    private Mouse _mouse;

    [Header("Mana")]
    [SerializeField] private int _totalMana;
    [SerializeField] private int _inicialMana;
    [SerializeField] private float _regenerationTime;
    [SerializeField] private TMP_Text _txtCurrentMana;
    [SerializeField] private int _currentMana;

    [Header("Enemy Spawn")]
    [SerializeField] private float timeStep;
    [SerializeField] private int enemiesToSpawn;

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
        
        _currentMana = _inicialMana;
        UpdateManaText();
        StartCoroutine(ManaRegeneration());

        Spawner[] spawners = GameObject.FindObjectsOfType<Spawner>();
        foreach (Spawner spawner in spawners)
        {
            spawner.InitSpawn(enemiesToSpawn, timeStep);
        }
    }

    private void OnEnable()
    {
        CardActions.OnCardSelected += CardSelected;
    }

    private void OnDisable()
    {
        CardActions.OnCardSelected -= CardSelected;
    }

    private void Update() => CharacterPlacement();

    private IEnumerator ManaRegeneration()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(_regenerationTime);

            if (_currentMana < _totalMana)
                _currentMana++;

            UpdateManaText();
        }
    }

    private void UpdateManaText() => _txtCurrentMana.SetText($"<b>{_currentMana}</b> / <b>{_totalMana}</b>");

    private void CharacterPlacement()
    {
        if (_currentCard != null)
        {
            Vector3 mousePoint = new Vector3(1000f, 0f, 0f);
            
            PlacementFeedback(mousePoint);

            if (Input.GetMouseButtonDown(0))
            {
                if (_currentMana < _currentCard.manaCost) return;

                if (_mouse.Aiming(_groundCorrect, out mousePoint))
                {
                    Instantiate(_currentCard.prefab, mousePoint, Quaternion.identity);
                    _currentMana -= _currentCard.manaCost;
                    UpdateManaText();
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

    private void PlacementFeedback(Vector3 mousePoint)
    {
        _light.enabled = false;

        if (_mouse.Aiming(_groundCorrect, out mousePoint))
        {
            _light.enabled = true;
            _light.color = Color.green;
            _light.transform.position = mousePoint + new Vector3(0f, 0.01f, 0f);
        }

        if (_mouse.Aiming(_groundIncorrect, out mousePoint))
        {
            _light.enabled = true;
            _light.color = Color.red;
            _light.transform.position = mousePoint + new Vector3(0f, 0.01f, 0f);
        }
    }

    private void CardSelected(object sender, CardActions.EventArgs_OnCardSelected e)
    {
        _currentCard = null;
        _currentCard = e.card;
    }
}
