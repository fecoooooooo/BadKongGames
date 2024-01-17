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
            UnlockAchievement(Achievement.Shimai1);
            SetProgressStat(Stat.SHIMAI, pieceCount, true);
        }
        else if (id == "812")
        {
            UnlockAchievement(Achievement.Shimai2);
            SetProgressStat(Stat.SHIMAI, pieceCount, false);
        }
        else if (id == "813")
        {
            UnlockAchievement(Achievement.Emi1);
            SetProgressStat(Stat.EMI, pieceCount, true);
        }
        else if (id == "814")
        {
            UnlockAchievement(Achievement.Emi2);
            SetProgressStat(Stat.EMI, pieceCount, false);
        }
        else if (id == "815")
        {
            UnlockAchievement(Achievement.Kaya1);
            SetProgressStat(Stat.KAYA, pieceCount, true);
        }
        else if (id == "816")
        {
            UnlockAchievement(Achievement.Kaya2);
            SetProgressStat(Stat.KAYA, pieceCount, false);
        }
        else if (id == "817")
        {
            UnlockAchievement(Achievement.Rei1);
            SetProgressStat(Stat.REI, pieceCount, true);
        }
        else if (id == "818")
        {
            UnlockAchievement(Achievement.Rei2);
            SetProgressStat(Stat.REI, pieceCount, false);
        }
        else if (id == "819")
        {
            UnlockAchievement(Achievement.Shiko1);
            SetProgressStat(Stat.SHIKO, pieceCount, true);
        }
        else if (id == "820")
        {
            UnlockAchievement(Achievement.Shiko2);
            SetProgressStat(Stat.SHIKO, pieceCount, false);
        }
        else if (id == "821")
        {
            UnlockAchievement(Achievement.Kiko1);
            SetProgressStat(Stat.KIKO, pieceCount, true);
        }
        else if (id == "822")
        {
            UnlockAchievement(Achievement.Kiko2);
            SetProgressStat(Stat.KIKO, pieceCount, false);
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
        KIKO,
        EMI,
        KAYA,
        REI,
        SHIKO,
        SHIMAI
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
        { Stat.KIKO,   0},
        { Stat.EMI,    0},
        { Stat.KAYA,   0},
        { Stat.REI,    0},
        { Stat.SHIKO,  0},
        { Stat.SHIMAI, 0},
    };

    public enum Achievement
	{
        LetsPlay_20sec,
        Kiko1,
        Kiko2,
        Emi1,
        Emi2,
        Kaya1,
        Kaya2,
        Rei1,
        Rei2,
        Shiko1,
        Shiko2,
        Shimai1,
        Shimai2,
        GameTime_10min,
        PuzzleStar_all9Piece,
        PuzzleChampion_all12Piece,
        PuzzleGrandMaster_all9_12_16Piece,
	}

    static readonly Dictionary<Achievement, string> AchievementsSteamIDs = new Dictionary<Achievement, string>()
    {
        { Achievement.LetsPlay_20sec,                       "NEW_ACHIEVEMENT_3_0"},
        { Achievement.Kiko1,                                "NEW_ACHIEVEMENT_3_1"},
        { Achievement.Kiko2,                                "NEW_ACHIEVEMENT_3_2"},
        { Achievement.Emi1,                                 "NEW_ACHIEVEMENT_3_3"},
        { Achievement.Emi2,                                 "NEW_ACHIEVEMENT_3_4"},
        { Achievement.Kaya1,                                "NEW_ACHIEVEMENT_3_5"},
        { Achievement.Kaya2,                                "NEW_ACHIEVEMENT_3_6"},
        { Achievement.Rei1,                                 "NEW_ACHIEVEMENT_3_7"},
        { Achievement.Rei2,                                 "NEW_ACHIEVEMENT_3_8"},
        { Achievement.Shiko1,                               "NEW_ACHIEVEMENT_3_9"},
        { Achievement.Shiko2,                               "NEW_ACHIEVEMENT_3_10"},
        { Achievement.Shimai1,                              "NEW_ACHIEVEMENT_3_11"},
        { Achievement.Shimai2,                              "NEW_ACHIEVEMENT_3_12"},
        { Achievement.GameTime_10min,                       "NEW_ACHIEVEMENT_3_13"},
        { Achievement.PuzzleStar_all9Piece,                 "NEW_ACHIEVEMENT_3_14"},
        { Achievement.PuzzleChampion_all12Piece,            "NEW_ACHIEVEMENT_3_15"},
        { Achievement.PuzzleGrandMaster_all9_12_16Piece,    "NEW_ACHIEVEMENT_3_16"},
    };

    static readonly Dictionary<Achievement, bool> AchievementsUnlockState = new Dictionary<Achievement, bool>();


    class a
	{
        public Achievement AchievementEnum { get; private set; }
        public string SteamId { get; private set; }
        public string ModelId { get; private set; }
    }
}
