using UnityEngine;
using WebSocketSharp;
using System;
using TMPro;

namespace Nomlas.ToN_Overlay
{
    public class WebSocketParser : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI terrorText;
        [SerializeField] private TextMeshProUGUI messageText;
        private WebSocket ws;
        private string terrorName;
        private string roundName;
        private bool hasUpdatedTerrorName;
        private bool isConnected;

        private void Start()
        {
            // WebSocket接続設定
            ws = new WebSocket("ws://localhost:11398");

            ws.OnOpen += (sender, e) =>
            {
                Debug.Log("WebSocket Open");
                isConnected = true;
            };

            ws.OnMessage += (sender, e) =>
            {
                OnMessageReceived(sender, e);
            };

            ws.OnError += (sender, e) =>
            {
                Debug.LogError("WebSocket Error Message: " + e.Message);
                Debug.LogException(e.Exception);
            };

            ws.OnClose += (sender, e) =>
            {
                Debug.LogError("WebSocket Close Reason: " + e.Reason);
                isConnected = false;
            };

            ws.Connect();
        }

        private void Update()
        {
            if (!hasUpdatedTerrorName)
            {
                terrorText.text = roundName + terrorName;
                hasUpdatedTerrorName = true;
            }
            messageText.text = "WebSocket: " + (isConnected ? "<color=green>接続済み</color>" : "<color=red>未接続</color>");
        }

        private void OnDestroy()
        {
            // WebSocket切断
            if (ws != null)
            {
                ws.Close();
                ws = null;
            }
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            ParseEventData(e.Data);
        }

        private void ParseEventData(string json)
        {
            var jsonObject = JsonUtility.FromJson<EventData>(json);
            EventType type = Enum.Parse<EventType>(jsonObject.Type, true);
            if (type == EventType.ROUND_TYPE)
            {
                roundName = jsonObject.Command == 1 ? (jsonObject.DisplayName.Contains("霧") ? $"{jsonObject.DisplayName}\n" : string.Empty) : string.Empty;
            }
            else if (type == EventType.STATS)
            {
                if (jsonObject.Name == "TerrorName")
                {
                    hasUpdatedTerrorName = false;
                    terrorName = jsonObject.Value.Replace(" & ", "\n").Replace("Mona\nThe Mountain", "Mona & The Mountain");
                }
            }
        }

        /*
        private string ParseEventData(string json)
        {
            var jsonObject = JsonUtility.FromJson<EventData>(json);
            EventType type = (EventType)Enum.Parse(typeof(EventType), jsonObject.Type, true);
            switch (type)
            {
                case EventType.CONNECTED:
                    return $"Hello, {jsonObject.DisplayName}さん！";
                case EventType.TERRORS:
                    return !string.IsNullOrEmpty(jsonObject.DisplayName) ? $"<color=#{jsonObject.DisplayColor:X}>{string.Join(", ", jsonObject.Names)}</color>" : "";
                case EventType.ROUND_TYPE:
                    return $"ラウンド{(jsonObject.Command == 1 ? $"開始: {jsonObject.DisplayName}" : "終了")}";
                case EventType.LOCATION:
                    return $"Location: {jsonObject.Name} by {jsonObject.Creator} {(jsonObject.Origin != null ? $"from {jsonObject.Origin}" : "")}";
                case EventType.ITEM:
                    return jsonObject.Command == 1 ? $"Grabbed Item: {jsonObject.Name}" : "";
                case EventType.PAGE_COUNT:
                    return $"{jsonObject.Value}/8ページ";
                case EventType.DAMAGED:
                    return $"{jsonObject.Value}ダメージ";
                case EventType.PLAYER_JOIN:
                    return $"{jsonObject.Value}が合流";
                case EventType.PLAYER_LEAVE:
                    return $"{jsonObject.Value}が退出";
                case EventType.MASTER_CHANGE:
                    return "マスター変更";
                case EventType.STATS:
                    ParseStats(jsonObject.Name, jsonObject.Value);
                    return "";
                default:
                    return "";
            }
        }
        

        private void ParseStats(string name, string value)
        {
            if (name == "TerrorName") stats.TerrorName = value.Replace(" & ", "\n").Replace("Mona\nThe Mountain", "Mona & The Mountain");
            else if (name == "RoundType") stats.RoundType = value;
            else if (name == "MapName") stats.MapName = value;
            else if (name == "MapCreator") stats.MapCreator = value;
            else if (name == "MapOrigin") stats.MapOrigin = value;
            else if (name == "ItemName") stats.ItemName = value;
            else if (name == "IsAlive") stats.IsAlive = ToBool(value);
            else if (name == "IsReborn") stats.IsReborn = ToBool(value);
            else if (name == "IsKiller") stats.IsKiller = ToBool(value);
            else if (name == "IsStarted") stats.IsStarted = ToBool(value);
            else if (name == "RoundInt") stats.RoundInt = int.Parse(value);
            else if (name == "MapInt") stats.MapInt = int.Parse(value);
            else if (name == "PageCount") stats.PageCount = int.Parse(value);
        }
        
        private static bool ToBool(string value)
        {
            if (value == "true" || value == "True" || value == "TRUE" || value == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        */
    }
}