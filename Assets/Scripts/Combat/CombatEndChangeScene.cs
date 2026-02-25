using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatEndChangeScene : MonoBehaviour
{
    [SerializeField] private string _sceneOverworld;
    [SerializeField] private string _sceneLose;
    private IEnumerator _waitingCoroutine;

    private void OnEnable()
    {
        CombatManager.OnPlayerWin += OnPlayerWin_StartCoroutine;
        CombatManager.OnPlayerLose += OnPlayerLose_StartCoroutine;
        CombatManager.OnPlayerEscaped += OnPlayerEscaped_StartCoroutine;

        CheckAllWinsAndRestart.OnWinGame += OnWinGame_StopCoroutines;
    }

    private void OnDisable()
    {
        CombatManager.OnPlayerWin -= OnPlayerWin_StartCoroutine;
        CombatManager.OnPlayerLose -= OnPlayerLose_StartCoroutine;
        CombatManager.OnPlayerEscaped -= OnPlayerEscaped_StartCoroutine;

        CheckAllWinsAndRestart.OnWinGame -= OnWinGame_StopCoroutines;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public IEnumerator WaitingForSceneChange(bool hasLost)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(hasLost ? _sceneOverworld : _sceneLose);
    }

    public void OnPlayerWin_StartCoroutine()
    {
        if (_waitingCoroutine != null)
            StopCoroutine(_waitingCoroutine);

        _waitingCoroutine = WaitingForSceneChange(true);
        StartCoroutine(_waitingCoroutine);
    }

    public void OnPlayerLose_StartCoroutine()
    {
        if (_waitingCoroutine != null)
            StopCoroutine(_waitingCoroutine);

        _waitingCoroutine = WaitingForSceneChange(false);
        StartCoroutine(_waitingCoroutine);
    }

    public void OnPlayerEscaped_StartCoroutine()
    {
        if (_waitingCoroutine != null)
            StopCoroutine(_waitingCoroutine);

        _waitingCoroutine = WaitingForSceneChange(true);
        StartCoroutine(_waitingCoroutine);
    }

    public void OnPlayerWinFinalBattle_StartCoroutine()
    {
        if (_waitingCoroutine != null)
            StopCoroutine(_waitingCoroutine);

        _waitingCoroutine = WaitingForSceneChange(true);
        StartCoroutine(_waitingCoroutine);
    }

    public void OnWinGame_StopCoroutines()
    {
        if (_waitingCoroutine != null)
            StopCoroutine(_waitingCoroutine);
    }
}
