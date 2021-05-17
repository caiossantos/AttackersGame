using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static GameStatus Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        CardActions.OnCardSelected += CardMessage;
    }

    private void OnDisable()
    {
        CardActions.OnCardSelected -= CardMessage;
    }

    private void CardMessage(object sender, CardActions.EventArgs_OnCardSelected e)
    {
        Debug.Log("Debug do evento " + e.card.name);
    }
}
