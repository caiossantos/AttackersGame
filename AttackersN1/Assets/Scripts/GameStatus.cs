using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStatus : MonoBehaviour
{
    public static GameStatus Instance { get; private set; }

    [SerializeField] private Light _light;
    [SerializeField] private float gameTime;
    [SerializeField] private TMP_Text _txtTimer;
    
    [Header("Mana")]
    [SerializeField] private int _totalMana;
    [SerializeField] private int _inicialMana;
    [SerializeField] private float _regenerationTime;
    [SerializeField] private TMP_Text _txtCurrentMana;
    private int _currentMana;

    [Header("Enemy Spawn")]
    [SerializeField] private float timeStep;
    [SerializeField] private int enemiesToSpawn;

    [Header("UI Geral")]
    [SerializeField] private GameObject uiSuccessState;
    [SerializeField] private GameObject uiVictory;
    [SerializeField] private GameObject uiDefeat;
    [SerializeField] private GameObject uiPause;

    private const string GROUND_CORRECT = "GroundAlly";
    private const string GROUND_INCORRET = "GroundEnemy";
    public const string MENU_SCENE_NAME = "MainMenu";
    private Card _currentCard;
    private Mouse _mouse;
    private Spawner[] spawners;
    private float timer;

    private void Awake()
    {
        if (Time.timeScale != 1f)
            Time.timeScale = 1f;

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable() => CardActions.OnCardSelected += CardSelected;

    private void Start()
    {
        _mouse = GetComponent<Mouse>();
        
        _currentMana = _inicialMana;
        UpdateManaText();
        StartCoroutine(ManaRegeneration());

        spawners = FindObjectsOfType<Spawner>();
        foreach (Spawner spawner in spawners)
        {
            spawner.InitSpawn(enemiesToSpawn, timeStep);
        }

        timer += Time.deltaTime + gameTime;
    }

    private void Update()
    {
        Timer();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        CharacterPlacement();
    }


    private void OnDisable() => CardActions.OnCardSelected -= CardSelected;
    private void Timer()
    {
        if (timer <= 0f)
        {
            SetSuccessState(false);
        }
        else
        {
            timer -= Time.deltaTime;
            _txtTimer.SetText("{0:00}:{1:00}", (int)(timer / 60), (int)(timer % 60));
        }
    }

    public void Pause()
    {
        if (Time.timeScale != 0f)
        {
            Time.timeScale = 0f;
            uiPause.SetActive(true);
        }
        else
        {
            uiPause.SetActive(false);
            Time.timeScale = 1f;
        }
    }

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


    private void CharacterPlacement()
    {
        if (_currentCard != null)
        {
            Vector3 mousePoint = new Vector3(1000f, 0f, 0f);
            
            PlacementFeedback(mousePoint);

            if (Input.GetMouseButtonDown(0))
            {
                if (_currentMana < _currentCard.manaCost) return;

                if (_mouse.Aiming(GROUND_CORRECT, out mousePoint))
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

        if (_mouse.Aiming(GROUND_CORRECT, out mousePoint))
        {
            _light.enabled = true;
            _light.color = Color.green;
            _light.transform.position = mousePoint + new Vector3(0f, 0.01f, 0f);
        }

        if (_mouse.Aiming(GROUND_INCORRET, out mousePoint))
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

    #region UI
    public void SetSuccessState(bool state)
    {
        Time.timeScale = 0f;

        foreach (Spawner spawner in spawners)
        {
            spawner.Pause(true);
        }
        uiSuccessState.SetActive(true);

        if (state)
            uiVictory.SetActive(true);
        else
            uiDefeat.SetActive(true);
    }
    public void BtnPlayAgain() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void BtnBackToMenu() => SceneManager.LoadScene(MENU_SCENE_NAME);
    public void BtnQuitGame() => Application.Quit();
    private void UpdateManaText() => _txtCurrentMana.SetText($"<b>{_currentMana}</b>");
    #endregion
}
