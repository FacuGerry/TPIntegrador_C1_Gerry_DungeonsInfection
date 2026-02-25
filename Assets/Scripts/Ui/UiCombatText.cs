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

        CombatAction.OnPlayerKillEnemy += OnPlayerKillEnemy_ChangeText;

        CombatAction.OnPlayerUsedSpell += OnPlayerUsedSpell_ChangeText;

        CombatAction.OnPlayerHealedWithAbsorb += OnPlayerHealedWithAbsorb_ChangeText;

        EnemyTurnManager.OnEnemyTurnStart += OnEnemyTurnStart_ChangeText;

        EnemyTurnManager.OnShowEnemyAttacked += OnShowEnemyAttacked_ChangeText;

        UiCombatButtons.OnNotHavingSpells += OnNotHavingSpells_ChangeText;

        Character.OnInfected += OnInfected_ChangeText;

        CombatAction.OnFailedToCast += OnFailedToCast_ChangeText;
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

        CombatAction.OnPlayerKillEnemy -= OnPlayerKillEnemy_ChangeText;

        CombatAction.OnPlayerUsedSpell -= OnPlayerUsedSpell_ChangeText;

        CombatAction.OnPlayerHealedWithAbsorb -= OnPlayerHealedWithAbsorb_ChangeText;

        EnemyTurnManager.OnEnemyTurnStart -= OnEnemyTurnStart_ChangeText;

        EnemyTurnManager.OnShowEnemyAttacked -= OnShowEnemyAttacked_ChangeText;

        UiCombatButtons.OnNotHavingSpells -= OnNotHavingSpells_ChangeText;

        Character.OnInfected -= OnInfected_ChangeText;

        CombatAction.OnFailedToCast -= OnFailedToCast_ChangeText;
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
        _text.text = "You win!!!\nGoing back to the dungeon...";
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

    public void OnPlayerKillEnemy_ChangeText(Character enemy)
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

            case CombatAction.Spells.MagicShield:
                _text.text = player.data.name + " uses a Magic Shield to protect itself.";
                break;

            case CombatAction.Spells.Absorb:
                _text.text = player.data.name + " prepares itself to absorb damage.";
                break;

            case CombatAction.Spells.Heal:
                _text.text = player.data.name + " heals its life.";
                break;
        }
    }

    public void OnPlayerHealedWithAbsorb_ChangeText(Character player)
    {
        _text.text = player.data.name + " absorbed the damage and healed.";
    }

    public void OnNotHavingSpells_ChangeText()
    {
        _text.text = "You don't have spells to use.";
    }

    public void OnInfected_ChangeText(Character player)
    {
        _text.text = player.name + " is damaged by infection.";
    }

    public void OnFailedToCast_ChangeText(Character player)
    {
        _text.text = player.name + " failed casting the spell";
    }
}
