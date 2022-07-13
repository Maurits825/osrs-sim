using TMPro;
using UnityEngine;

public class GameStateUI : MonoBehaviour
{
    [SerializeField] private GameStates gameState;
    private TextMeshProUGUI gameStateText;

    private void Start()
    {
        gameStateText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        gameStateText.text = gameState.currentState.ToString();
    }
}
