using OOPProject.Models.Contracts;
using OOPProject.Utilities;
using OOPProject.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Models
{
    public class Animal : IAnimal
    {
        private readonly int maxHp;
        private readonly int maxEnergy;

        public Animal(int id, string emoji, int killCount, int hp, int attack ,bool canSwim, string animalType, int energy, int energyConsumption ,string landType )
        {
            Id = id;
            Emoji = emoji;
            KillCount = killCount;
            Hp = hp;
            Attack = attack;
            CanSwim = canSwim;
            AnimalType = animalType;
            Energy = energy;
            EnergyConsumption = energyConsumption;
            LandType = landType;


            maxHp = hp;
            maxEnergy = energy;
        }
        private int id;

        public int Id
        {
            get { return id; }
            protected set { id = value; }
        }

        private int killCount;

        public int KillCount
        {
            get { return killCount; }
            set { killCount = value; }
        }


        private string emoji;

        public string Emoji
        {
            get { return emoji; }
            protected set { emoji = value; }
        }
          
        private int hp;

        public int Hp
        {
            get { return hp; }
            set { hp = value; }
        }

        private int attack;

        public int Attack
        {
            get { return attack; }
            protected set { attack = value; }
        }


        private bool canSwim;

        public bool CanSwim
        {
            get { return canSwim; }
            protected set { canSwim = value; }
        }

        private string animalType;

        public string AnimalType
        {
            get { return animalType; }
            protected set { animalType = value; }
        }

        private int energy;

        public int Energy
        {
            get { return energy; }
            set { energy = value; }
        }

        private int energyConsumption;

        public int EnergyConsumption
        {
            get { return energyConsumption; }
            set { energyConsumption = value; }
        }

        private string landType;

        public string LandType
        {
            get { return landType; }
            protected set { landType = value; }
        }



        public Animal Clone()
        {
            return new Animal(
                this.Id,
                this.Emoji,
                this.KillCount,
                this.Hp,
                this.Attack,
                this.CanSwim,
                this.AnimalType,
                this.Energy,
                this.EnergyConsumption,
                this.LandType
            );
        }


        public void RecoverAfterBattle()
        {
            Hp = Math.Min(Hp + 50, maxHp);

            Energy = maxEnergy;
        }

        public void ConsumeEnergy()
        {
            this.Energy -= this.energyConsumption;

            if (Energy <= 0)
            {
                Console.WriteLine(string.Format(OutputMessages.DeadFromExhaustion, Emoji));
                Thread.Sleep(3000);
                return;
            }
        }
    }
}
