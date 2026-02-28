using UnityEngine;
using UnityEngine.SceneManagement;

public class PressToContinue : MonoBehaviour
{
    [SerializeField] private KeyBindingsSO _data;
    [SerializeField] private PositionInOverworldSO _position;
    [SerializeField] private Vector3 _positionToSpawn;
    [SerializeField] private string _sceneToLoad;

    private void Update()
    {
        if (Input.GetKey(_data.interact))
        {
            _position.position = _positionToSpawn;
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
