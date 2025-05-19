using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPlane : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Transform playerTransform;
    private Vector3 playerPosition;
    void Start()
    {
        playerPosition =new Vector3(27, -1, 35);
     }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("kill");
        playerTransform.position = playerPosition;
        //if (other.gameObject.CompareTag("Player"))
        //{
          //  playerTransform.position = playerPosition;
        //}


    }
}
 