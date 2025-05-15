using OOPProject.Models.Contracts;
using OOPProject.Utilities;
using OOPProject.Utilities.EmojiImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Models
{
    public class AnimalList
    {

        public AnimalList()
        {
            AddedAnimalList = AddingAllAnimalsToList();
        }

        private List<Animal> addedAnimalList;

        public List<Animal> AddedAnimalList
        {
            get { return addedAnimalList; }
            private set { addedAnimalList = value; }
        }


        public static readonly Animal Tiger = new Animal(1,EmojiList.Tiger, 0, 80, 40, false, AnimalType.Carnivore, 90, 85, LandType.Terrestrial);

        public static readonly Animal Leopard = new Animal(2, EmojiList.Leopard, 0, 75, 35, false, AnimalType.Carnivore, 85, 90, LandType.Terrestrial);

        public static readonly Animal Crocodile = new Animal(3, EmojiList.Crocodile, 0, 100, 80, true, AnimalType.Carnivore, 70, 30, "Both");

        public static readonly Animal Gorilla = new Animal(4, EmojiList.Gorilla, 0, 95, 50, false, AnimalType.Herbivore, 80, 50, LandType.Terrestrial);

        public static readonly Animal Rhino = new Animal(5, EmojiList.Rhino, 0, 120, 90, false, AnimalType.Herbivore, 60, 25, LandType.Terrestrial);

        public static readonly Animal Shark = new Animal(6, EmojiList.Shark, 0, 110, 70, true, AnimalType.Carnivore, 85, 80, LandType.Aquatic);

        public static readonly Animal Squid = new Animal(7, EmojiList.Squid, 0, 60, 20, true, AnimalType.Carnivore, 95, 75, LandType.Aquatic);

        public static readonly Animal Fish = new Animal(8, EmojiList.Fish, 0, 30, 10, true, AnimalType.Herbivore, 50, 60, LandType.Aquatic);

        public static readonly Animal Ox = new Animal(9, EmojiList.Ox, 0, 110, 60, false, AnimalType.Herbivore, 70, 40, LandType.Terrestrial);

        public static readonly Animal Penguin = new Animal(10, EmojiList.Penguin, 0, 55, 30, true, AnimalType.Carnivore, 70, 35, "Both");

        public static readonly Animal Elephant = new Animal(11, EmojiList.Elephant, 0, 150, 95, false, AnimalType.Herbivore, 50, 20, LandType.Terrestrial);

        public static readonly Animal Monkey = new Animal(12, EmojiList.Monkey, 0, 70, 25, false, AnimalType.Herbivore, 95, 80, LandType.Terrestrial);

        static List<Animal> AddingAllAnimalsToList()
        {
            List<Animal> list = new List<Animal>();

            list.Add(Tiger);
            list.Add(Leopard);
            list.Add(Crocodile);
            list.Add(Gorilla);
            list.Add(Rhino);
            list.Add(Shark);
            list.Add(Squid);
            list.Add(Fish);
            list.Add(Ox);
            list.Add(Penguin);
            list.Add(Elephant);
            list.Add(Monkey);

            return list;
        }
    }
}
