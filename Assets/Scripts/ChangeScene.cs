using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private UnityEngine.SceneManagement.Scene sceneLoaded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int indexScene;
    public GameObject endCanva;
    void Start()
    {
        print(indexScene);
        UnityEngine.SceneManagement.Scene sceneLoaded = SceneManager.GetActiveScene();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (indexScene == 1)
            {
                SceneManager.LoadScene(2);
            }
            else if (indexScene == 2)
            {
                endCanva.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
