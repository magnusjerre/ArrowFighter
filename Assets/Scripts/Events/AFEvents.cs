﻿using UnityEngine;

namespace Jerre.Events
{
    public class AFEvents
    {
        public static AFEvent Kill(int killerPlayerNumber, int killedPlayerNumber)
        {
            return new AFEvent(AFEventType.KILLED, new KilledEventPayload(killerPlayerNumber, killedPlayerNumber));
        }

        public static AFEvent GameOver(int winnerPlayerNumber)
        {
            return new AFEvent(AFEventType.GAME_OVER, new GameOverPayload(winnerPlayerNumber));
        }

        public static AFEvent PlayerJoin(int playerNumber, Color playerColor)
        {
            return new AFEvent(AFEventType.PLAYER_JOIN, new PlayerJoinPayload(playerNumber, playerColor));
        }

        public static AFEvent PlayerLeave(int playerNumber)
        {
            return new AFEvent(AFEventType.PLAYER_LEAVE, new PlayerLeavePayload(playerNumber));
        }

        public static AFEvent Score(int playerNumber, int playerScore, int maxScore)
        {
            return new AFEvent(AFEventType.SCORE, new ScorePayload(playerNumber, playerScore, maxScore));
        }

        public static AFEvent BombTrigger(int ownerPlayerNumber, bool triggeredByPlayer)
        {
            return new AFEvent(AFEventType.BOMB_TRIGGER, new BombTriggerPayload(ownerPlayerNumber, triggeredByPlayer));
        }

        public static AFEvent PlayerMenuBarUICreated(int playerNumber)
        {
            return new AFEvent(AFEventType.PLAYER_MENU_BAR_UI_CREATED, new PlayerMenuBarUICreatedPayload(playerNumber));
        }

        public static AFEvent HealthDamage(int damagedPlayerNumber, int damage, int healthLeft)
        {
            return new AFEvent(AFEventType.HEALTH_DAMAGE, new HealthDamagePayload(damagedPlayerNumber, damage, healthLeft));
        }

        public static AFEvent Respawn(int playerNumber, int health)
        {
            return new AFEvent(AFEventType.RESPAWN, new RespawnPayload(playerNumber, health));
        }
    }
}
