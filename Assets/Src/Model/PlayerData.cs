using System;
using UnityEngine;

namespace Assets.Src.Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Hp;
        public bool IsArmed;

        public PlayerData Clone()
        {
            return new PlayerData
            {
                Coins = Coins,
                Hp = Hp,
                IsArmed = IsArmed
            };
        }
    }
}
