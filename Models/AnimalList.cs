using OOPProject.Models.Contracts;
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


        public static readonly Animal Tiger = new Animal(1,EmojiList.Tiger, 0, 80, 40, false, "Carnivore", 90, 85, "Terrestrial");

        public static readonly Animal Leopard = new Animal(2, EmojiList.Leopard, 0, 75, 35, false, "Carnivore", 85, 90, "Terrestrial");

        public static readonly Animal Crocodile = new Animal(3, EmojiList.Crocodile, 0, 100, 80, true, "Carnivore", 70, 30, "Both");

        public static readonly Animal Gorilla = new Animal(4, EmojiList.Gorilla, 0, 95, 50, false, "Herbivore", 80, 50, "Terrestrial");

        public static readonly Animal Rhino = new Animal(5, EmojiList.Rhino, 0, 120, 90, false, "Herbivore", 60, 25, "Terrestrial");

        public static readonly Animal Shark = new Animal(6, EmojiList.Shark, 0, 110, 70, true, "Carnivore", 85, 80, "Aquatic");

        public static readonly Animal Squid = new Animal(7, EmojiList.Squid, 0, 60, 20, true, "Carnivore", 95, 75, "Aquatic");

        public static readonly Animal Fish = new Animal(8, EmojiList.Fish, 0, 30, 10, true, "Herbivore", 50, 60, "Aquatic");

        public static readonly Animal Ox = new Animal(9, EmojiList.Ox, 0, 110, 60, false, "Herbivore", 70, 40, "Terrestrial");

        public static readonly Animal Penguin = new Animal(10, EmojiList.Penguin, 0, 55, 30, true, "Carnivore", 70, 35, "Both");

        public static readonly Animal Elephant = new Animal(11, EmojiList.Elephant, 0, 150, 95, false, "Herbivore", 50, 20, "Terrestrial");

        public static readonly Animal Monkey = new Animal(12, EmojiList.Monkey, 0, 70, 25, false, "Herbivore", 95, 80, "Terrestrial");

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
