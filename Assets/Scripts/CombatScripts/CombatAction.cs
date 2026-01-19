using System;
using System.Collections;
using UnityEngine;

public class CombatAction : MonoBehaviour
{
    public static event Action OnPlayerEndTurn;

    private IEnumerator _corroutineSelectEnemy;

    private void OnDestroy()
    {
        StopAllCoroutines();
        _corroutineSelectEnemy = null;
    }

    private IEnumerator WatingForInput(CharacterDataSO data)
    {
        bool isWaiting = true;
        while (isWaiting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 direction = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D ray = Physics2D.Raycast(mousePos, direction);

                if (ray.collider != null && ray.collider.TryGetComponent(out Character character))
                {
                    character.life -= data.attack;
                    Debug.Log(character.name + " life is " + character.life, character.gameObject);
                    isWaiting = false;
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        OnPlayerEndTurn?.Invoke();
    }

    public void PlayerSelectEnemy(CharacterDataSO data)
    {
        if (_corroutineSelectEnemy != null)
            StartCoroutine(_corroutineSelectEnemy);

        _corroutineSelectEnemy = WatingForInput(data);
        StartCoroutine(_corroutineSelectEnemy);
    }
}
