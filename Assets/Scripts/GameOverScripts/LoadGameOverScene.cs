using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameOverScene : MonoBehaviour
{
    [SerializeField] private float transitionTime = 2.5f;
    private float currentTransitionTime = 2.5f;

    private void Start()
    {
        currentTransitionTime = transitionTime;
    }

    private void Update()
    {
        currentTransitionTime -= Time.deltaTime;

        if (currentTransitionTime <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
            Destroy(this.gameObject);
        }
    }
}
