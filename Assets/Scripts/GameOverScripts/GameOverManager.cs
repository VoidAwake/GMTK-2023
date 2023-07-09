using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI reasonText;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateGameOverText();
        scoreText.text = "Final Score: " + DaddyManager.instance.highscore;
        if(PlayerPrefs.HasKey("HighScore") == false)
        {
            PlayerPrefs.SetFloat("HighScore", DaddyManager.instance.highscore);
        }
        else if(PlayerPrefs.GetFloat("HighScore") < DaddyManager.instance.highscore)
        {
            PlayerPrefs.SetFloat("HighScore", DaddyManager.instance.highscore);
        }
    }

    private void UpdateGameOverText()
    {
        switch (DaddyManager.instance.GetGameOverType())
        {
            case GAME_OVER_TYPE.NONE:
                reasonText.text = "Ran out of time";
                break;
            case GAME_OVER_TYPE.HEART_RATE_TOO_HIGH:
                reasonText.text = "Too much stress!";
                break;
            case GAME_OVER_TYPE.BARISTA_LOST_PATIENCE:
                reasonText.text = "Barista grew impatient";
                break;
            default:
                Debug.LogWarning("Unknown game over reason");
                break;
        }
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(1);
    }
}
