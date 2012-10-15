﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShootR
{
    public class ShipLevelManager
    {
        private Ship _me;
        private LevelCalculator _levelCalculator;

        public event LevelUpEventHandler OnLevel;

        public ShipLevelManager(Ship me)
        {
            Level = 1;
            Experience = 0;            
            _me = me;
            _levelCalculator = new LevelCalculator();
            ExperienceToNextLevel = Convert.ToInt32(_levelCalculator.NextLevelExperience(Level));

            _me.OnKill += Killed;
            _me.OnDeath += Died;

            OnLevel += _me.LifeController.LeveledUp;
            OnLevel += _me.GetWeaponController().LeveledUp;
        }

        public int Level { get; private set; }
        public int Experience { get; private set; }
        public int ExperienceToNextLevel { get; private set; }

        public void Killed(object sender, KillEventArgs e)
        {
            int exp = _levelCalculator.CalculateKillExperience(_me, e.Killed as Ship);

            Experience += exp;
            // Need to level
            if (Experience >= ExperienceToNextLevel)
            {
                Level++;
                Experience = Experience - ExperienceToNextLevel;
                ExperienceToNextLevel = Convert.ToInt32(_levelCalculator.NextLevelExperience(Level));

                if (OnLevel != null)
                {
                    OnLevel(_me, new LevelUpEventArgs(Level));
                }
            }
        }

        public void Died(object sender, DeathEventArgs e)
        {
            int exp = _levelCalculator.CalculateKillExperience((e.KilledBy as Bullet).FiredBy, _me);
            Experience = Math.Max(Experience - exp, 0);
        }
    }
}