using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private UnityEngine.SceneManagement.Scene sceneLoaded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.SceneManagement.Scene sceneLoaded = SceneManager.GetActiveScene();
    }

private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (sceneLoaded.buildIndex == 1)
            {
                SceneManager.LoadScene(2);

            }
            else if(sceneLoaded.buildIndex==2)
            {
                SceneManager.LoadScene(0);
            }
        }
       

    }
}
