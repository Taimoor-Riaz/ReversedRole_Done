public static class GameRuleState
{
    public enum ContactMode { None, DamagePlayer, HealPlayer }

    public static ContactMode ZombieContact { get; private set; } = ContactMode.DamagePlayer;
    public static ContactMode PrincessContact { get; private set; } = ContactMode.None;

    public static void SetZombieContactMode(ContactMode mode) => ZombieContact = mode;
    public static void SetPrincessContactMode(ContactMode mode) => PrincessContact = mode;

    // Optional: later you can add more:
    // public static ContactMode PotionContact { get; private set; }
}
