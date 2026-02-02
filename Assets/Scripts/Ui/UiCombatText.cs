using TMPro;
using UnityEngine;

public class UiCombatText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        CombatManager.OnBattleStart += OnBattleStart_ChangeText;

        CombatManager.OnWaitingForNewTurn += OnWaitingForNewTurn_ClearText;

        CombatManager.OnIndicationsChange += OnIndicationsChange_ChangeText;

        CombatManager.OnPlayerWin += OnPlayerWin_ChangeText;

        CombatManager.OnPlayerEscaped += OnPlayerEscaped_ChangeText;

        CombatManager.OnPlayerNotEscaped += OnPlayerNotEscaped_ChangeText;

        CombatAction.OnPlayerSelectEnemy += OnPlayerSelectEnemy_ChangeText;

        CombatAction.OnPlayerAttackedEnemy += OnPlayerAttackedEnemy_ChangeText;

        CombatAction.OnPlayerKillEnemyShowText -= OnPlayerKillEnemyShowText_ChangeText;

        CombatAction.OnPlayerUsedSpell += OnPlayerUsedSpell_ChangeText;

        CombatAction.OnPlayerHealedWithDarkShield += OnPlayerHealedWithDarkShield_ChangeText;

        EnemyTurnManager.OnEnemyTurnStart += OnEnemyTurnStart_ChangeText;

        EnemyTurnManager.OnShowEnemyAttacked += OnShowEnemyAttacked_ChangeText;
    }

    private void OnDisable()
    {
        CombatManager.OnBattleStart -= OnBattleStart_ChangeText;

        CombatManager.OnWaitingForNewTurn -= OnWaitingForNewTurn_ClearText;

        CombatManager.OnIndicationsChange -= OnIndicationsChange_ChangeText;

        CombatManager.OnPlayerWin -= OnPlayerWin_ChangeText;

        CombatManager.OnPlayerEscaped -= OnPlayerEscaped_ChangeText;

        CombatManager.OnPlayerNotEscaped -= OnPlayerNotEscaped_ChangeText;

        CombatAction.OnPlayerSelectEnemy -= OnPlayerSelectEnemy_ChangeText;

        CombatAction.OnPlayerAttackedEnemy -= OnPlayerAttackedEnemy_ChangeText;

        CombatAction.OnPlayerKillEnemyShowText -= OnPlayerKillEnemyShowText_ChangeText;

        CombatAction.OnPlayerUsedSpell -= OnPlayerUsedSpell_ChangeText;

        CombatAction.OnPlayerHealedWithDarkShield -= OnPlayerHealedWithDarkShield_ChangeText;

        EnemyTurnManager.OnEnemyTurnStart -= OnEnemyTurnStart_ChangeText;

        EnemyTurnManager.OnShowEnemyAttacked -= OnShowEnemyAttacked_ChangeText;
    }

    public void OnBattleStart_ChangeText()
    {
        _text.text = "The battle starts!";
    }

    public void OnWaitingForNewTurn_ClearText()
    {
        _text.text = " ";
    }

    public void OnIndicationsChange_ChangeText(Character player)
    {
        if (!player.data.isPlayer)
            return;

        if (player.life <= 0)
            _text.text = player.data.name + " has died. Battle is over...";
        else 
            _text.text = "It's " + player.data.name + "'s turn.";
    }

    public void OnPlayerWin_ChangeText()
    {
        _text.text = "You win!!!";
    }

    public void OnPlayerEscaped_ChangeText()
    {
        _text.text = "Successfully escaped!";
    }

    public void OnPlayerNotEscaped_ChangeText()
    {
        _text.text = "Couldn't run away.";
    }

    public void OnEnemyTurnStart_ChangeText(CharacterDataSO enemy)
    {
        _text.text = "It's " + enemy.name + "'s turn.";
    }

    public void OnShowEnemyAttacked_ChangeText(CharacterDataSO enemy, Character player, string action)
    {
        if (action == "healed")
            _text.text = enemy.name + " " + action + ".";
        else
            _text.text = enemy.name + " " + action + " " + player.data.name + ".";
    }

    public void OnPlayerSelectEnemy_ChangeText()
    {
        _text.text = "Select an enemy to attack";
    }

    public void OnPlayerAttackedEnemy_ChangeText(Character player, Character enemy)
    {
        _text.text = player.data.name + " attacked " + enemy.data.name;
    }

    public void OnPlayerKillEnemyShowText_ChangeText(Character enemy)
    {
        _text.text = enemy.data.name + " was slain.";
    }

    public void OnPlayerUsedSpell_ChangeText(Character player, CombatAction.Spells spells)
    {
        switch (spells)
        {
            case CombatAction.Spells.None:
                Debug.LogError("NO SPELL HAS BEEN USED, WRONG LINE!!!");
                break;

            case CombatAction.Spells.Defend:
                _text.text = player.data.name + " defended.";
                break;

            case CombatAction.Spells.Fireball:
                _text.text = player.data.name + " attacked with a Fireball.";
                break;

            case CombatAction.Spells.IceWall:
                _text.text = player.data.name + " uses an Ice Wall to protect itself.";
                break;

            case CombatAction.Spells.DarkShield:
                _text.text = player.data.name + " uses a Dark Shield to absorb damage.";
                break;

            case CombatAction.Spells.HealingRoot:
                _text.text = player.data.name + " heals with roots founded on the ground.";
                break;
        }
    }

    public void OnPlayerHealedWithDarkShield_ChangeText(Character player)
    {
        _text.text = player.data.name + " absorbed the damage and healed.";
    }
}
