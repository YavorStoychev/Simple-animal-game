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
      
        public Animal(int id, string emoji, int killCount, int hp, int defence, bool canSwim, string animalType, int energy, int speed, string landType )
        {
            Id = id;
            Emoji = emoji;
            KillCount = killCount;
            Hp = hp;
            Defence = defence;
            CanSwim = canSwim;
            AnimalType = animalType;
            Energy = energy;
            Speed = speed;
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
            protected set { hp = value; }
        }

        private int defence;

        public int Defence
        {
            get { return defence; }
            protected set { defence = value; }
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
            protected set { energy = value; }
        }

        private int speed;

        public int Speed
        {
            get { return speed; }
            protected set { speed = value; }
        }

        private string landType;

        public string LandType
        {
            get { return landType; }
            protected set { landType = value; }
        }

       
    }
}
