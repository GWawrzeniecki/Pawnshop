using System;
using System.Linq;

namespace Pawnshop.TestingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TEST");
            using DAL.Data.UnitOfWork unitOfWork = new DAL.Data.UnitOfWork();
            Console.WriteLine(unitOfWork.ClientRepository.Get().Count());      
            


        }
    }
}
