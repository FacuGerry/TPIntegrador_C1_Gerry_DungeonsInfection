using UnityEngine;

public class EnemyLifeBar : MonoBehaviour
{
    [SerializeField] private GameObject _lifeBar;

    private Character _enemy;
    private float _initalSize;

    private void Awake()
    {
        _enemy = GetComponent<Character>();
    }

    private void Start()
    {
        _initalSize = _lifeBar.transform.localScale.x;
    }

    private void OnEnable()
    {
        CombatAction.OnUpdateEnemyLife += UpdateLifeBar;
    }

    private void OnDisable()
    {
        CombatAction.OnUpdateEnemyLife -= UpdateLifeBar;
    }

    public void UpdateLifeBar(Character enemy)
    {
        if (enemy == _enemy)
        {
            float life = (float)enemy.life / (float)enemy.maxLife;
            if (life < 0f)
                life = 0f;
            _lifeBar.transform.localScale = new Vector3(life * _initalSize, _lifeBar.transform.localScale.y, _lifeBar.transform.localScale.z);
        }
    }
}
