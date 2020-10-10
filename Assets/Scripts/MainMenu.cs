using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clickSound;
    
    public void PlayGame()
    {
        StartCoroutine(DelayedPlayGame());
    }

    private IEnumerator DelayedPlayGame()
    {
        yield return new WaitForSeconds(clickSound.length + 0.25f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        StartCoroutine(DelayedQuitGame());
    }

    private IEnumerator DelayedQuitGame()
    {
        yield return new WaitForSeconds(clickSound.length);
        Debug.Log("Game has been quit");
        Application.Quit();
    }
    
    public void OnClick()
    {
        source.PlayOneShot(clickSound);
    }
}
