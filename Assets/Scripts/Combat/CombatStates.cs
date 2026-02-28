public partial class CombatManager
{
    public enum CombatStates
    {
        None,
        Start,
        ChangeTurn,
        EnemyTurn,
        PlayerSelectAction,
        PlayerSelectAttack,
        PlayerSelectDefend,
        PlayerSelectFireball,
        PlayerSelectMagicShield,
        PlayerSelectAbsorb,
        PlayerSelectHeal,
        PlayerSelectRun,
        Won,
        Lost,
        Escaped,
        Infected
    }
}