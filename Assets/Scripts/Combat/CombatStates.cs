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
        PlayerSelectIceWall,
        PlayerSelectDarkShield,
        PlayerSelectHealingRoot,
        PlayerSelectRun,
        Won,
        Lost,
        Escaped,
    }
}