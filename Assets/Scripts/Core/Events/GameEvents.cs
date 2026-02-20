using System;
using UnityEngine;

public static class GameEvents
{
    // Input
    public static Action<Vector2> OnMoveInput;
    public static Action<Vector2> OnAimScreenInput;
    public static Action OnFirePressed;

    // Rules / Modifiers
    public static Action<Vector2> OnMoveMultiplierChanged;
    public static Action<float> OnAimMultiplierChanged;

    // Gameplay events
    public static Action<HitEvent> OnHit;              // projectile hits something
    public static Action<PickupEvent> OnPickup;        // player picks item

    // Stats requests (RuleEngine -> Stats)
    public static System.Action<int, int> OnHealthChanged;
    public static Action<int> OnPlayerHealthDelta;     // + / -
    public static Action<int> OnScoreDelta;            // + / -
    public static Action<DamageRequest> OnDamageTarget;// damage enemy/target

    // Rules Flipped
    public static Action<string> OnRuleBanner;   // show text like "RULES FLIPPED!"
    // Announcements
    public static Action<Announcement> OnAnnouncement;

    // Game Logic Win/Lose
    public static Action<int, string> OnPhaseChanged; // (index, phaseName)
    public static Action OnGameWin;
    public static Action OnGameLose;

    public static System.Action<bool> OnZombieContactDamageEnabled;

}

public enum TargetType
{
    Zombie,
    Princess,    // (or Sorceress in theme)
    Player
}

public enum PickupType
{
    PowerUp,
    Poison
}

[Serializable]
public struct HitEvent
{
    public TargetType targetType;
    public Vector2 hitPoint;
    public DamageableTarget target;
    public int baseDamage; // “normal” damage value (rule may redirect)
}

[Serializable]
public struct PickupEvent
{
    public PickupType pickupType;
    public int baseValue; // amount (heal, boost etc.)
}

[Serializable]
public struct DamageRequest
{
    public TargetType targetType;
    public DamageableTarget target;
    public int damage;
}
public enum AnnouncementType
{
    GameStart,
    RuleStart,
    RuleFlip,
    Warning,
    Info
}

[Serializable]
public struct Announcement
{
    public AnnouncementType type;
    public string title;
    public string body;
    public float duration;

    public Announcement(AnnouncementType type, string title, string body, float duration = 2f)
    {
        this.type = type;
        this.title = title;
        this.body = body;
        this.duration = duration;
    }
}