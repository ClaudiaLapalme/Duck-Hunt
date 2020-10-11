using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void OnClickButton(string option)
    {
        SceneManager.LoadScene(option == "yes" ? 1 : 0);
    }
}
