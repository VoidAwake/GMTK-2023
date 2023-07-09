using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTransition : MonoBehaviour
{
    private float transitionTime = 3.5f;
    private float currentTransitionTime = 3.5f;

    private bool levelWon = false;

    public void StartTransition(bool _levelWon)
    {
        levelWon = _levelWon;
        currentTransitionTime = transitionTime;
    }

    private void Update()
    {
        currentTransitionTime -= Time.deltaTime;

        if (currentTransitionTime <= 0)
        {
            if (levelWon)
            {
                SceneManager.LoadScene("Result_Screen");
            }
            else
            {
                SceneManager.LoadScene("GameOverScene");
            }
            
            StartCoroutine(HideObject());
        }
    }

    private IEnumerator HideObject()
    {
        yield return null;
        gameObject.SetActive(false);
    }
}
