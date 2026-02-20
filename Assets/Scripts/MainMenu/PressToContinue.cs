using UnityEngine;
using UnityEngine.SceneManagement;

public class PressToContinue : MonoBehaviour
{
    [SerializeField] private KeyBindingsSO _data;
    [SerializeField] private string _sceneToLoad;

    private void Update()
    {
        if (Input.GetKey(_data.interact))
        {
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
