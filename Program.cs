using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("1/3 is :");

            IFraction ff = new ProperFraction(1L, 3L);
            Console.WriteLine(ff);

            Console.WriteLine("Now let's try -3/4");
            ff = new ProperFraction(-3L, 4L);
            Console.WriteLine(ff);

            Console.WriteLine("Now we try 4/6");

            ff = new ProperFraction(4L, 6L);
            Console.WriteLine(ff);
            Console.WriteLine("The reciprecal of that is: " + ((ProperFraction)(ff)).getReciprecal());

            Console.ReadLine();
        }
    }
}
