using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatEndChangeScene : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad;
    private IEnumerator _waitingCoroutine;

    private void OnEnable()
    {
        CombatManager.OnPlayerWin += OnPlayerWin_StartCoroutine;
        CombatManager.OnPlayerLose += OnPlayerLose_StartCoroutine;
        CombatManager.OnPlayerEscaped += OnPlayerEscaped_StartCoroutine;
    }

    private void OnDisable()
    {
        CombatManager.OnPlayerWin -= OnPlayerWin_StartCoroutine;
        CombatManager.OnPlayerLose -= OnPlayerLose_StartCoroutine;
        CombatManager.OnPlayerEscaped -= OnPlayerEscaped_StartCoroutine;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public IEnumerator WaitingForChange()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(_sceneToLoad);
    }

    public void OnPlayerWin_StartCoroutine()
    {
        if (_waitingCoroutine != null)
            StopCoroutine(_waitingCoroutine);

        _waitingCoroutine = WaitingForChange();
        StartCoroutine(_waitingCoroutine);
    }

    public void OnPlayerLose_StartCoroutine()
    {
        if (_waitingCoroutine != null)
            StopCoroutine(_waitingCoroutine);

        _waitingCoroutine = WaitingForChange();
        StartCoroutine(_waitingCoroutine);
    }

    public void OnPlayerEscaped_StartCoroutine()
    {
        if (_waitingCoroutine != null)
            StopCoroutine(_waitingCoroutine);

        _waitingCoroutine = WaitingForChange();
        StartCoroutine(_waitingCoroutine);
    }
}
