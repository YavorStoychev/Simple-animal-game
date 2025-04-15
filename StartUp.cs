using System.Linq;
using System.Text;
using OOPProject.Core;
using OOPProject.Core.Contracts;
using OOPProject.Models;
using OOPProject.Models.Contracts;
using OOPProject.Utilities.EmojiImages;

namespace OOPProject
{
    public class SetUp
    {
        static void Main(string[] args)
        {
            //Тази програма се пуска или на windows 11
            //или на windows 10 с изтеглен и сложем по подразбиране windows terminal от microsoft store.
            IEngine engine = new Engine();
            engine.Run();
        }
    }
}