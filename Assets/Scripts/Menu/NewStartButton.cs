using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NewStartButton : MonoBehaviour
{
    [SerializeField] private SceneReference sceneReference;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => OnClick());
    }

    private void OnClick()
    {
        if (PlayerPrefs.HasKey("levelsCompleted"))
        {
            PlayerPrefs.SetInt("levelsCompleted", 0);
            
            if(DaddyManager.instance)
                DaddyManager.instance.levelsCompleted = 0;
        }
        
        SceneManager.LoadScene(sceneReference.BuildIndex);
    }
}