using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTransition : MonoBehaviour
{
    // TODO: Transition time should be pulled from the animation clip
    private float transitionTime = 3.5f;

    public IEnumerator StartTransition(bool levelWon)
    {
        yield return new WaitForSeconds(transitionTime);
        
        if (levelWon)
        {
            SceneManager.LoadScene("Result_Screen");
        }
        else
        {
            SceneManager.LoadScene("GameOverScene");
        }
        
        yield return null;
        
        gameObject.SetActive(false);
    }
}
