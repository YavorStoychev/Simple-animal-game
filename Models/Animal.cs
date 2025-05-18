using OOPProject.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Models
{
    public class Animal : IAnimal
    {
      
        public Animal(int id, string emoji, int killCount, int hp, int attack ,bool canSwim, string animalType, int energy, string landType )
        {
            Id = id;
            Emoji = emoji;
            KillCount = killCount;
            Hp = hp;
            Attack = attack;
            CanSwim = canSwim;
            AnimalType = animalType;
            Energy = energy;          
            LandType = landType;
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

        private string landType;

        public string LandType
        {
            get { return landType; }
            protected set { landType = value; }
        }

       
    }
}
