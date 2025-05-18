using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Models.Contracts
{
    public interface IAnimal
    {   
        public int Id { get; } //Ид-то на животното
        public string Emoji { get; } // Иконка на животното
        public int KillCount { get; }// Броят на убийства за дадено животно
        public int Hp { get; } // Живот на животното

        public int Attack { get; } //Атака на животното
        public bool CanSwim { get; } // Дали може да плува животното
        public string AnimalType { get; } //Вид на животно (Месоядно или тревопасно)
        public int Energy { get; } // Енергия на животното
        public string LandType { get; } //Вид земя (Вода или сухоземни)
       
    }
}
