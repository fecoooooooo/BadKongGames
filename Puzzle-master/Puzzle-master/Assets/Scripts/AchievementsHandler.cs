using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System;

public class AchievementsHandler : MonoBehaviour
{
    protected Callback<UserStatsReceived_t> userStatsReceivedCallback;
    protected Callback<UserStatsStored_t> userStatsStoredCallback;
    protected Callback<UserAchievementStored_t> userAchievementStoredCallback;
    

    static bool AchievementsInitialized;
    static bool StatsInitialized;
    
    const float TWENTY_SECONDS = 20;
    const float TEN_MINUTES = 60*10;

    void Start()
    {
        //DeleteStatsAndAchievements();
        userStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        userStatsStoredCallback = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
        userAchievementStoredCallback = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
    }

    public static void UnlockAchievement(Achievement achievement)
	{
        if (!SteamManager.Initialized || !AchievementsInitialized || AchievementsUnlockState[achievement] || !StatsInitialized)
            return;

        string achievementId = AchievementsSteamIDs[achievement];
        SteamUserStats.SetAchievement(achievementId);
        SteamUserStats.StoreStats();
    }

    public static void SetStat(Stat stat, int newValue)
	{
        if (!SteamManager.Initialized || !AchievementsInitialized || !StatsInitialized)
            return;

        SteamUserStats.SetStat(stat.ToString(), newValue);
        SteamUserStats.StoreStats();
    }

    public static void SetStat(Stat stat, float newValue)
    {
        if (!SteamManager.Initialized || !AchievementsInitialized || !StatsInitialized)
            return;

        SteamUserStats.SetStat(stat.ToString(), newValue);
        SteamUserStats.StoreStats();
    }

    private void Update()
	{
        SecondsInGame += Time.deltaTime;

        if (!SteamManager.Initialized)
            return;

        SteamAPI.RunCallbacks();

        if (!StatsInitialized)
        {
			if (SteamUserStats.RequestCurrentStats()) 
                InitStats();
            return;
        }


        if (!AchievementsInitialized)
        {
            InitAchievements();
            return;
        }

        if (SecondsInGame > TWENTY_SECONDS && !AchievementsUnlockState[Achievement.LetsPlay_20sec])
            UnlockAchievement(Achievement.LetsPlay_20sec);
        if (SecondsInGame > TEN_MINUTES && !AchievementsUnlockState[Achievement.GameTime_10min])
            UnlockAchievement(Achievement.GameTime_10min);
    }

	void OnAchievementStored(UserAchievementStored_t pCallback){}

    void OnUserStatsStored(UserStatsStored_t pCallback){}

    private void OnUserStatsReceived(UserStatsReceived_t pCallback) { }

    void DeleteStatsAndAchievements()
	{
        Debug.LogWarning("All stats and achievements have been reset");
        SteamUserStats.ResetAllStats(true);
        SteamUserStats.StoreStats();
    }

    private void InitStats()
    {
        CSteamID userId = SteamUser.GetSteamID();

        Stat[] keys = new Stat[StatProgress.Keys.Count];
        StatProgress.Keys.CopyTo(keys, 0);

        for (int i = 0; i < keys.Length; ++i)
		{
            Stat stat = keys[i];
            int statValue;
            if (!SteamUserStats.GetUserStat(userId, stat.ToString(), out statValue))
                return;

            Debug.Log("Stat - " + stat.ToString() + ": " + statValue);
            StatProgress[stat] = statValue;
        }

        int secondsIngameAsInt;
        if (!SteamUserStats.GetUserStat(userId, Stat.SECONDS_IN_GAME.ToString(), out secondsIngameAsInt))
            return;

        SecondsInGame = secondsIngameAsInt;

        StatsInitialized = true;
    }

    void InitAchievements()
	{
        foreach(Achievement a in AchievementsSteamIDs.Keys)
		{
            bool isEarned;
            if(!SteamUserStats.GetAchievement(AchievementsSteamIDs[a], out isEarned))
			{
                AchievementsUnlockState.Clear();
                return;
            }

            AchievementsUnlockState.Add(a, isEarned);
        }

        AchievementsInitialized = true;
    }


    private void OnDestroy()
    {
        foreach (Stat s in StatProgress.Keys)
            SteamUserStats.SetStat(s.ToString(), StatProgress[s]);

        SteamUserStats.SetStat(Stat.SECONDS_IN_GAME.ToString(), (int)SecondsInGame);
        SteamUserStats.StoreStats();
    }

    internal static void UnlockByModelId(string id, int pieceCount)
    {
        if (id == "811")
        {
            UnlockAchievement(Achievement.Blades1);
            SetProgressStat(Stat.BLADES, pieceCount, true);
        }
        else if (id == "812")
        {
            UnlockAchievement(Achievement.Blades2);
            SetProgressStat(Stat.BLADES, pieceCount, false);
        }
        else if (id == "813")
        {
            UnlockAchievement(Achievement.Lilu1);
            SetProgressStat(Stat.LILU, pieceCount, true);
        }
        else if (id == "814")
        {
            UnlockAchievement(Achievement.Lilu2);
            SetProgressStat(Stat.LILU, pieceCount, false);
        }
        else if (id == "815")
        {
            UnlockAchievement(Achievement.Nikita1);
            SetProgressStat(Stat.NIKITA, pieceCount, true);
        }
        else if (id == "816")
        {
            UnlockAchievement(Achievement.Nikita2);
            SetProgressStat(Stat.NIKITA, pieceCount, false);
        }
        else if (id == "817")
        {
            UnlockAchievement(Achievement.Zoe1);
            SetProgressStat(Stat.ZOE, pieceCount, true);
        }
        else if (id == "818")
        {
            UnlockAchievement(Achievement.Zoe2);
            SetProgressStat(Stat.ZOE, pieceCount, false);
        }
        else if (id == "819")
        {
            UnlockAchievement(Achievement.Kibi1);
            SetProgressStat(Stat.KIBI, pieceCount, true);
        }
        else if (id == "820")
        {
            UnlockAchievement(Achievement.Kibi2);
            SetProgressStat(Stat.KIBI, pieceCount, false);
        }
        else if (id == "821")
        {
            UnlockAchievement(Achievement.Avril1);
            SetProgressStat(Stat.AVRIL, pieceCount, true);
        }
        else if (id == "822")
        {
            UnlockAchievement(Achievement.Avril2);
            SetProgressStat(Stat.AVRIL, pieceCount, false);
        }

        TryUnlockAllPieceAchievements();
    }

	private static void TryUnlockAllPieceAchievements()
	{
        bool all9Completed = true;
        bool all12Completed = true;
        bool all16Completed = true;

		if (!AchievementsUnlockState[Achievement.PuzzleStar_all9Piece])
		{
            foreach(Stat s in StatProgress.Keys)
			{
                if ((StatProgress[s] & (int)StatProgressMask.PIECE_1_9) == 0 || (StatProgress[s] & (int)StatProgressMask.PIECE_2_9) == 0)
                    all9Completed = false;
			}
            if (all9Completed)
                UnlockAchievement(Achievement.PuzzleStar_all9Piece);
		}

        if (!AchievementsUnlockState[Achievement.PuzzleChampion_all12Piece])
        {
            foreach (Stat s in StatProgress.Keys)
            {
                if ((StatProgress[s] & (int)StatProgressMask.PIECE_1_12) == 0 || (StatProgress[s] & (int)StatProgressMask.PIECE_2_12) == 0)
                    all12Completed = false;
            }
            if (all12Completed)
                UnlockAchievement(Achievement.PuzzleChampion_all12Piece);
        }

        if (!AchievementsUnlockState[Achievement.PuzzleGrandMaster_all9_12_16Piece])
        {
            foreach (Stat s in StatProgress.Keys)
            {
                if ((StatProgress[s] & (int)StatProgressMask.PIECE_1_16) == 0 || (StatProgress[s] & (int)StatProgressMask.PIECE_2_16) == 0)
                    all16Completed = false;
            }

            if (all9Completed && all12Completed && all16Completed)
                UnlockAchievement(Achievement.PuzzleGrandMaster_all9_12_16Piece);
        }

        

    }

	private static void SetProgressStat(Stat stat, int pieceCount, bool isFirst)
	{
        if (isFirst)
        {
            if (pieceCount == 9)
                StatProgress[stat] = StatProgress[stat] | (int)StatProgressMask.PIECE_1_9;
            else if (pieceCount == 12)
                StatProgress[stat] = StatProgress[stat] | (int)StatProgressMask.PIECE_1_12;
            else if (pieceCount == 16)
                StatProgress[stat] = StatProgress[stat] | (int)StatProgressMask.PIECE_1_16;
        }
		else
		{
            if (pieceCount == 9)
                StatProgress[stat] = StatProgress[stat] | (int)StatProgressMask.PIECE_2_9;
            else if (pieceCount == 12)
                StatProgress[stat] = StatProgress[stat] | (int)StatProgressMask.PIECE_2_12;
            else if (pieceCount == 16)
                StatProgress[stat] = StatProgress[stat] | (int)StatProgressMask.PIECE_2_16;
        }

        Debug.Log(stat.ToString() + ": " + Convert.ToString(StatProgress[stat], 2).PadLeft(8, '0'));

        /*Debug.Log($"{stat}: " +
            $"1_9:  {(StatProgress[stat] & (int)StatProgressMask.PIECE_1_9)  == (int)StatProgressMask.PIECE_1_9}, " +
            $"1_12: {(StatProgress[stat] & (int)StatProgressMask.PIECE_1_12) == (int)StatProgressMask.PIECE_1_12}, " +
            $"1_16: {(StatProgress[stat] & (int)StatProgressMask.PIECE_1_16) == (int)StatProgressMask.PIECE_1_16}, " +
            $"2_9:  {(StatProgress[stat] & (int)StatProgressMask.PIECE_2_9)  == (int)StatProgressMask.PIECE_2_9}, " +
            $"2_12: {(StatProgress[stat] & (int)StatProgressMask.PIECE_2_12) == (int)StatProgressMask.PIECE_2_12}, " +
            $"2_16: {(StatProgress[stat] & (int)StatProgressMask.PIECE_2_16) == (int)StatProgressMask.PIECE_2_16}, ");*/

        SetStat(stat, StatProgress[stat]);
    }

	public enum Stat
	{
        SECONDS_IN_GAME,
        AVRIL,
        LILU,
        NIKITA,
        ZOE,
        KIBI,
        BLADES
    }

    public enum StatProgressMask
	{
        PIECE_1_9   = 0b0000_0001,
        PIECE_1_12  = 0b0000_0010,
        PIECE_1_16  = 0b0000_0100,
        PIECE_2_9   = 0b0000_1000,
        PIECE_2_12  = 0b0001_0000,
        PIECE_2_16  = 0b0010_0000,
	}

    static float SecondsInGame;

    static readonly Dictionary<Stat, int> StatProgress = new Dictionary<Stat, int>()
    {
        { Stat.AVRIL,   0},
        { Stat.LILU,    0},
        { Stat.NIKITA,  0},
        { Stat.ZOE,     0},
        { Stat.KIBI,    0},
        { Stat.BLADES,  0},
    };

    public enum Achievement
	{
        LetsPlay_20sec,
        Avril1,
        Avril2,
        Lilu1,
        Lilu2,
        Nikita1,
        Nikita2,
        Zoe1,
        Zoe2,
        Kibi1,
        Kibi2,
        Blades1,
        Blades2,
        GameTime_10min,
        PuzzleStar_all9Piece,
        PuzzleChampion_all12Piece,
        PuzzleGrandMaster_all9_12_16Piece,
	}

    static readonly Dictionary<Achievement, string> AchievementsSteamIDs = new Dictionary<Achievement, string>()
    {
        { Achievement.LetsPlay_20sec,                       "NEW_ACHIEVEMENT_1_0"},
        { Achievement.Avril1,                               "NEW_ACHIEVEMENT_1_1"},
        { Achievement.Avril2,                               "NEW_ACHIEVEMENT_1_2"},
        { Achievement.Lilu1,                                "NEW_ACHIEVEMENT_1_3"},
        { Achievement.Lilu2,                                "NEW_ACHIEVEMENT_1_4"},
        { Achievement.Nikita1,                              "NEW_ACHIEVEMENT_1_5"},
        { Achievement.Nikita2,                              "NEW_ACHIEVEMENT_1_6"},
        { Achievement.Zoe1,                                 "NEW_ACHIEVEMENT_1_7"},
        { Achievement.Zoe2,                                 "NEW_ACHIEVEMENT_1_8"},
        { Achievement.Kibi1,                                "NEW_ACHIEVEMENT_1_9"},
        { Achievement.Kibi2,                                "NEW_ACHIEVEMENT_1_10"},
        { Achievement.Blades1,                              "NEW_ACHIEVEMENT_1_11"},
        { Achievement.Blades2,                              "NEW_ACHIEVEMENT_1_12"},
        { Achievement.GameTime_10min,                       "NEW_ACHIEVEMENT_1_13"},
        { Achievement.PuzzleStar_all9Piece,                 "NEW_ACHIEVEMENT_1_14"},
        { Achievement.PuzzleChampion_all12Piece,            "NEW_ACHIEVEMENT_1_15"},
        { Achievement.PuzzleGrandMaster_all9_12_16Piece,    "NEW_ACHIEVEMENT_1_16"},
    };

    static readonly Dictionary<Achievement, bool> AchievementsUnlockState = new Dictionary<Achievement, bool>();


    class a
	{
        public Achievement AchievementEnum { get; private set; }
        public string SteamId { get; private set; }
        public string ModelId { get; private set; }
    }
}
