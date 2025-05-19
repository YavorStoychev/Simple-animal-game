using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPProject.Utilities.Messages
{
    public struct OutputMessages
    {
        public const string IncorrectAnimalSelection 
            = "Incorrect animal selection!";

        public const string CannotPickCurrAnimal 
            = "You cannot choose that animal! You are not in {0} land! Choose another animal!";

        public const string TooMuchAnimalInput 
            = "Too much animals! The max capacity animals is {0}!";

        public const string PlayerKillCount 
            = "{0}'s kill count is {1}";

        public const string CannotKillAnimal 
            = "You cannot kill that animal!";

        public const string CannotInterfere 
            = "You cannot interfere with this {0}!";

        public const string CannotGoInsideTheWater 
            = "You cannot go inside the water!";

        public const string CannotGoOutsideTheWater 
            = "You cannot go outside the water!";

        public const string SuccessfullyKilledAnAnimal
            = "You successfully killed {0} ! Your kill counter now for {1}  is {2}.";

        public const string DeadFromExhaustion 
            = "{0} died from exhaustion!";

        public const string YouGotKilled 
            = "{0} killed your {1}!";

        public const string AlreadyDead 
            = "{0} is already dead! Chose different animal!";

    }
}
