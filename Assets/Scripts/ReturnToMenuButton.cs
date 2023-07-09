using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenuButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
