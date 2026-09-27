using System;
using UnityEngine;

namespace Assets.Src.Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Hp;
        private int _heroSwords;

        public bool IsArmed => HeroSwords > 0;

        public int HeroSwords
        {
            get => _heroSwords;
            set
            {
                _heroSwords = value;
                Debug.Log($"Setting HeroSwords: {value}");
            }
        }

        public PlayerData Clone()
        {
            return new PlayerData
            {
                Coins = Coins,
                Hp = Hp,
                HeroSwords = HeroSwords
            };
        }
    }
}