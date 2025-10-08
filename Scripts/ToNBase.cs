
using System;

namespace Nomlas.ToN_Overlay
{
    [Serializable]
    internal class EventData
    {
        public string Type;
        public string DisplayName;
        public string Name;
        public string[] Names;
        public string Creator;
        public string Origin;
        public string Message;
        public byte Command;
        public uint DisplayColor;
        public string Value;
    }
/*
    [Serializable]
    internal class Velocity
    {
        public float X;
        public float Y;
        public float Z;
    }

    internal class StatsRound
    {
        public string TerrorName { get; internal set; } = "???";
        public string RoundType { get; internal set; } = "???";
        public string MapName { get; internal set; } = "???";
        public string MapCreator { get; internal set; } = "???";
        public string MapOrigin { get; internal set; } = "???";
        public string ItemName { get; internal set; } = string.Empty;

        public bool IsAlive { get; internal set; } = true;
        public bool IsReborn { get; internal set; } = false;
        public bool IsKiller { get; internal set; } = false;
        public bool IsStarted { get; internal set; } = false;

        public int RoundInt { get; internal set; } = 0;
        public int MapInt { get; internal set; } = 255;

        public int PageCount { get; internal set; } = 0;
    }
    */

    public enum EventType
    {
        CONNECTED,
        STATS,
        TERRORS,
        ROUND_TYPE,
        LOCATION,
        ITEM,
        INSTANCE,
        ROUND_ACTIVE,
        ALIVE,
        REBORN,
        OPTED_IN,
        IS_SABOTEUR,
        PAGE_COUNT,
        DAMAGED,
        DEATH,
        PLAYER_JOIN,
        PLAYER_LEAVE,
        SAVED,
        MASTER_CHANGE
    }
}
