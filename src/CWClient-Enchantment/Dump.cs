using cygwin_x32.ObscuredTypes;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CWClient_Enchantment
{
    class Dump : MonoBehaviour
    {
        string path_to_dump_file = @"D:\CWClient.txt";

        void Update()
        {
            if (Peer.ClientGame)
            {
                File.WriteAllText(path_to_dump_file, GetLobbyInformation());
            }
        }

        void OnGUI()
        {
            foreach(EntityNetPlayer player in Peer.ClientGame.AlivePlayers)
            {
                if (IsLocalPlayer(player))
                    continue;
                if (IsFakePlayer(player))
                    continue;

                Vector3 screenPos = WorldToScreenPoint(player.Position);
                Vector3 fwd = player.transform.rotation * player.PlayerTransform.forward;
                Vector3 fwd2 = player.transform.rotation * Vector3.forward;
                Vector3 fwd3 = player.PlayerTransform.forward;
                Vector3 lookAt = WorldToScreenPoint(player.Position + fwd);
                Vector3 lookAt2 = WorldToScreenPoint(player.Position + fwd2);
                Vector3 lookAt3 = WorldToScreenPoint(player.Position + fwd3);
                Vector3 lookAt4 = WorldToScreenPoint(player.Forward);
                if (screenPos.z > 0)
                {
                    Draw.DrawLine(new Vector2(screenPos.x, screenPos.y), new Vector2(lookAt.x, lookAt.y), Color.red);
                    Draw.DrawLine(new Vector2(screenPos.x, screenPos.y), new Vector2(lookAt2.x, lookAt2.y), Color.red);
                    Draw.DrawLine(new Vector2(screenPos.x, screenPos.y), new Vector2(lookAt3.x, lookAt3.y), Color.red);
                    Draw.DrawLine(new Vector2(screenPos.x, screenPos.y), new Vector2(lookAt4.x, lookAt4.y), Color.red);
                    int width = 15;
                    int height = 15;
                    if (IsTeammate(player))
                    {
                        Draw.DrawBoxOutlines(new Vector2(screenPos.x - (width / 2), screenPos.y - (height / 2)), new Vector2(width, height), 1, Color.green);
                    }
                    else
                    {
                        Draw.DrawBoxOutlines(new Vector2(screenPos.x - (width / 2), screenPos.y - (height / 2)), new Vector2(width, height), 1, Color.cyan);
                    }
                    Draw.DrawString(new Vector2(screenPos.x, screenPos.y + height + 5), Color.white, Draw.TextFlags.TEXT_FLAG_CENTERED, $"Name: {player.Nick}");
                    // Draw.DrawString(new Vector2(screenPos.x, screenPos.y + height + 15), Color.white, Draw.TextFlags.TEXT_FLAG_CENTERED, $"USERID: {player.UserID} / ID: {player.ID}");
                    // Draw.DrawString(new Vector2(screenPos.x, screenPos.y + height + 25), Color.white, Draw.TextFlags.TEXT_FLAG_CENTERED, $"HP: {player.Health}");
                    // Draw.DrawString(new Vector2(screenPos.x, screenPos.y + height + 35), Color.white, Draw.TextFlags.TEXT_FLAG_CENTERED, $"LVL: {player.Level}");
                    // Draw.DrawString(new Vector2(screenPos.x, screenPos.y + height + 45), Color.white, Draw.TextFlags.TEXT_FLAG_CENTERED, $"Class: {player.PlayerClass}");
                }
                Draw.DrawLine(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(screenPos.x, screenPos.y), Color.gray);
            }
            foreach (ClientEntity entity in Peer.ClientGame.AllEntities)
            {
                Vector3 screenPos = WorldToScreenPoint(entity.transform.position);
                if (screenPos.z > 0)
                {
                    int width = 15;
                    int height = 15;
                    Draw.DrawBoxOutlines(new Vector2(screenPos.x - (width / 2), screenPos.y - (height / 2)), new Vector2(width, height), 1, Color.red);
                    Draw.DrawLine(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(screenPos.x, screenPos.y), Color.white);
                    Draw.DrawString(new Vector2(screenPos.x, screenPos.y + height + 5), Color.white, Draw.TextFlags.TEXT_FLAG_CENTERED, entity.name);
                }
            }
        }

        void DrawPlayerAim()
        {
            foreach (EntityNetPlayer player in Peer.ClientGame.AlivePlayers)
            {
                RaycastHit hit;
                Vector3 pos = player.camera.transform.position;
                Physics.Linecast(player.camera.transform.position, player.camera.transform.position - new Vector3(pos.x, pos.y * 1000f, pos.z), out hit);
                Vector3 endPosition = WorldToScreenPoint(hit.point);
                Vector3 startPosition = WorldToScreenPoint(player.camera.transform.position);
                Draw.DrawLine(new Vector2(startPosition.x, startPosition.y), new Vector2(endPosition.x, endPosition.y), Color.red);
            }
        }

        void DrawEntities()
        {
            foreach (ClientEntity entity in Peer.ClientGame.AllEntities)
            {
                Vector3 screenPos = WorldToScreenPoint(entity.transform.position);
                if (screenPos.z > 0)
                {
                    int width = 15;
                    int height = 15;
                    Draw.DrawBoxOutlines(new Vector2(screenPos.x - (width / 2), screenPos.y - (height / 2)), new Vector2(width, height), 1, Color.red);
                    Draw.DrawLine(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(screenPos.x, screenPos.y), Color.white);
                    Draw.DrawString(new Vector2(screenPos.x, screenPos.y + height + 5), Color.white, Draw.TextFlags.TEXT_FLAG_CENTERED, entity.name);
                }
            }
        }
        bool IsFakePlayer(EntityNetPlayer player)
        {
            return player.ID <= 0 ? true : false;
        }

        bool IsTeammate(EntityNetPlayer player)
        {
            return player.IsBear == Peer.ClientGame.LocalPlayer.IsBear ? true : false;
        }
        bool IsLocalPlayer(EntityNetPlayer player)
        {
            return player == Peer.ClientGame.LocalPlayer ? true : false;
        }

        void DrawPlayers()
        {
            foreach (EntityNetPlayer player in Peer.ClientGame.AlivePlayers)
            {
                if (IsLocalPlayer(player))
                    continue;
                if (IsFakePlayer(player))
                    continue;

                Vector3 screenPos = WorldToScreenPoint(player.Position);
                if (screenPos.z > 0)
                {
                    int width = 15;
                    int height = 15;
                    if (IsTeammate(player))
                    {
                        Draw.DrawBoxOutlines(new Vector2(screenPos.x - (width / 2), screenPos.y - (height / 2)), new Vector2(width, height), 1, Color.green);
                    }
                    else
                    {
                        Draw.DrawBoxOutlines(new Vector2(screenPos.x - (width / 2), screenPos.y - (height / 2)), new Vector2(width, height), 1, Color.cyan);
                    }
                    Draw.DrawLine(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(screenPos.x, screenPos.y), Color.gray);
                    Draw.DrawString(new Vector2(screenPos.x, screenPos.y + height + 5), Color.white, Draw.TextFlags.TEXT_FLAG_CENTERED, $"Name: {player.Nick}");
                    Draw.DrawString(new Vector2(screenPos.x, screenPos.y + height + 15), Color.white, Draw.TextFlags.TEXT_FLAG_CENTERED, $"USERID: {player.UserID} / ID: {player.ID}");
                    Draw.DrawString(new Vector2(screenPos.x, screenPos.y + height + 25), Color.white, Draw.TextFlags.TEXT_FLAG_CENTERED, $"HP: {player.Health}");
                    Draw.DrawString(new Vector2(screenPos.x, screenPos.y + height + 35), Color.white, Draw.TextFlags.TEXT_FLAG_CENTERED, $"LVL: {player.Level}");
                    Draw.DrawString(new Vector2(screenPos.x, screenPos.y + height + 45), Color.white, Draw.TextFlags.TEXT_FLAG_CENTERED, $"Class: {player.PlayerClass}");
                }
            }
        }

        Vector3 WorldToScreenPoint(Vector3 worldPos)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            screenPos.y = Screen.height - screenPos.y;
            return screenPos;
        }

        string GetLobbyInformation()
        {
            string value = "";
            List<EntityNetPlayer> players = Peer.ClientGame.AlivePlayers;

            foreach (EntityNetPlayer player in players)
            {
                value = value +
                    $"{player.UserID}|" +
                    $"{player.Nick}|" +
                    $"{player.Level}|" +
                    $"{player.Health}|" +
                    $"{Camera.main.WorldToScreenPoint(player.Position).x}|{Camera.main.WorldToScreenPoint(player.Position).y}|{Camera.main.WorldToScreenPoint(player.Position).z}|" +
                    $"{Camera.main.WorldToScreenPoint(player.NeckPosition).x}|{Camera.main.WorldToScreenPoint(player.NeckPosition).y}|{Camera.main.WorldToScreenPoint(player.NeckPosition).z}|" +
                    $"{player.IsBear}|{Peer.ClientGame.LocalPlayer.IsBear}\n";
            }

            return value;
        }
    }
}
