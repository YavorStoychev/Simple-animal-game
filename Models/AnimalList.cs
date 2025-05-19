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


        public static readonly Animal Tiger = new Animal(1,EmojiList.Tiger, 0, 80, 5, false, AnimalType.Carnivore, 90, 2, LandType.Terrestrial);

        public static readonly Animal Leopard = new Animal(2, EmojiList.Leopard, 0, 75, 4, false, AnimalType.Carnivore, 85, 3, LandType.Terrestrial);

        public static readonly Animal Crocodile = new Animal(3, EmojiList.Crocodile, 0, 100, 8, true, AnimalType.Carnivore, 70, 4, LandType.Terrestrial);

        public static readonly Animal Gorilla = new Animal(4, EmojiList.Gorilla, 0, 95, 8, false, AnimalType.Herbivore, 80, 5, LandType.Terrestrial);

        public static readonly Animal Rhino = new Animal(5, EmojiList.Rhino, 0, 120, 10, false, AnimalType.Herbivore, 60, 4, LandType.Terrestrial);

        public static readonly Animal Shark = new Animal(6, EmojiList.Shark, 0, 110, 10, true, AnimalType.Carnivore, 85, 3, LandType.Aquatic);

        public static readonly Animal Squid = new Animal(7, EmojiList.Squid, 0, 60, 2, true, AnimalType.Carnivore, 95, 2, LandType.Aquatic);

        public static readonly Animal Fish = new Animal(8, EmojiList.Fish, 0, 30, 1, true, AnimalType.Herbivore, 50, 1, LandType.Aquatic);

        public static readonly Animal Ox = new Animal(9, EmojiList.Ox, 0, 110, 8, false, AnimalType.Herbivore, 100, 6, LandType.Terrestrial);

        public static readonly Animal Penguin = new Animal(10, EmojiList.Penguin, 0, 55, 2, true, AnimalType.Carnivore, 70, 1, LandType.Terrestrial);

        public static readonly Animal Elephant = new Animal(11, EmojiList.Elephant, 0, 150, 15, false, AnimalType.Herbivore, 150, 8, LandType.Terrestrial);

        public static readonly Animal Monkey = new Animal(12, EmojiList.Monkey, 0, 70, 2, false, AnimalType.Herbivore, 95, 2, LandType.Terrestrial);

        static List<Animal> AddingAllAnimalsToList()
        {
            List<Animal> animals = new List<Animal>();

            animals.Add(Tiger);
            animals.Add(Leopard);
            animals.Add(Crocodile);
            animals.Add(Gorilla);
            animals.Add(Rhino);
            animals.Add(Shark);
            animals.Add(Squid);
            animals.Add(Fish);
            animals.Add(Ox);
            animals.Add(Penguin);
            animals.Add(Elephant);
            animals.Add(Monkey);

            foreach (var animal in animals)
            {
               animal.RecoverAfterBattle();
            }

            return animals;
        }

        public Animal GetClonedAnimalByEmoji(string emoji)
        {
            return AddedAnimalList.FirstOrDefault(a => a.Emoji == emoji)?.Clone();
        }

        public Animal GetClonedAnimalById(int id)
        {
            return AddedAnimalList.FirstOrDefault(a => a.Id == id)?.Clone();
        }
    }
}
