using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Tests
{
    [TestClass()]
    public class InterpretorTests
    {
        [TestMethod()]
        public void evaluateSimpleTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void testComputeMethod()
        {
            Random rd = new Random();
            while (rd.Next(1000) != 0)
            {

               ProperFraction n1= getRandomFraction(rd);
               ProperFraction n2 =getRandomFraction(rd);
               char opt =Interpretor.OPERATORS[rd.Next(3)];
               Console.WriteLine(n1+" "+opt+" ("+n2+")");
                ProperFraction res = Interpretor.compute(n1, n2, opt);
               Console.WriteLine("Compute: " + res + " = "+res.getDoubleValue());
            }
        }

        public static ProperFraction getRandomFraction(Random rd)
        {
            Int64 d = rd.Next(2000) - 1000;
            if (d == 0) return getRandomFraction(rd);
            return new ProperFraction(rd.Next(2000)-1000,d);
        }


        [TestMethod()]
        public void testComputeStack()
        {
            Int64[] nums = {3,5,9,7};
            char[] operators = { '-','+','-','*' };

            Stack<ProperFraction> nstack = new Stack<ProperFraction>();
            Stack<char> optstack = new Stack<char>();
            foreach (Int64 ele in nums)
            {
                nstack.Push(new ProperFraction(ele)); 
            }
            foreach (char ele in operators)
            {
                optstack.Push(ele);
            }
            Console.WriteLine("This is the number stack: " + nstack.Count);
            Console.WriteLine("Operator stack: " + optstack.Count);
            Console.WriteLine(Interpretor.evalueteStacks(nstack,optstack).ToString());

        }

        [TestMethod()]
        public void testExpressionDiegest()
        {
            new Interpretor.ExpressionDiegest("1+9/(-9+3)");
            try
            {
               Interpretor.ExpressionDiegest ed = new Interpretor.ExpressionDiegest("1+9/(-93)");
               Console.WriteLine(ed.ToString());
            }
            catch (Exception)
            {

            }

        }

        [TestMethod()]
        public void TestinfixtToPostFix()
        {
            {

                String[] strlist = { "9/(0-9+3)", "-(-3+8/7)", "4-3/2", "-(-3*5/6+(9-54)-(4*3+4*(3+2)))"};
                foreach (String exp in strlist)
                {
                    IList<Object> expression = new Interpretor.ExpressionDiegest(exp).getCastSplittedExpression();
                    Queue<Object> Postfix = Interpretor.infixtToPostFix(expression);
                    Console.WriteLine(Postfix != null);

                    printICollection<Object>(Postfix.ToList());

                    Console.WriteLine("Evaluate the postfix. ");

                    ProperFraction pf = Interpretor.evaluatePostFix(Postfix);
                    Console.WriteLine(pf.ToString());
                }
            }
           

        }



        [TestMethod()]
        public void testInterpretor()
        {
            String[] strlist = { "9/(0-9+3)", "-(-3+8/7)", "4-3/2", "-(-3*5/6+(9-54)-(4*3+4*(3+2)))" ,"3.9","99999999999999999999"};

            foreach(String str in strlist)
            {
                Console.WriteLine("The input is: "+ str);
                new Interpretor(str);
            }

        }
        private static  void printICollection<T>(ICollection<T> arg)
        {
            Console.Write("{ ");
            foreach (T element in arg)
            {
                Console.Write( " "+element.ToString() + " ");
            }
            Console.Write("}\n");
        }


    }
}