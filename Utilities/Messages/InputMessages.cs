using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Utilities.Messages
{
    public struct InputMessages
    {
        public const string FieldSize 
            = "Insert field size (NxN)";

        public const string AnimalCountInput 
            = "Insert npc animals count:";

        public const string EnterACommand 
            = "Tap 'Enter' to enter a command (Type 'Help' to see the available commands.)";

        public const string PlayerInfo 
            = "Player's HP: {0} {1} Player's Attack {2} {3}  Players's Energy: {4} {5}";

        public const string AnimalInfo
            = "Animal's HP: {0} {1} Animal's Attack {2} {3}";

        public const string AnimalPick 
            = "Which animal do you want to pick?";

        public const string AreYouSureAboutTheFight
            = "Are you sure you want to fight this  {0} ?";

        public const string BeginOrCancelFight
            = "Press 'Enter' to begin the battle or 'Esc' to cancel it.";
    }
}
