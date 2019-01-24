using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp1.Tests
{
    [TestClass()]
    public class ImproperFractionTests
    {
        [TestMethod()]
        public void IntegerFractionTest()
        {
           
        }

        [TestMethod()]
        public void ToStringTest()
        {

        }

        [TestMethod()]
        public void TestProperFraction()
        {
            Tests.TestProperFraction.runTest();
        }

        [TestMethod()]
        public void SpecificFailedTestCase()
        {
            Int64 n = 742, d = 371;
            Console.WriteLine("The numerator and the denominator are:" + n + "," + d);
            ProperFraction pf1 = new ProperFraction(n, d);
            Console.WriteLine("= " + pf1);
        }
    }

    [TestClass()]
    public class TestProperFraction : ProperFraction
    {
        private TestProperFraction(long n, long d) : base(n, d)
        {

        }



        private static TestProperFraction getRandomTestNumber(ref Random rd)
        {
            
            Int64 n = rd.Next(1000) - 500;
            Int64 d = rd.Next(1000) - 500;
            if (d == 0||n==0) return getRandomTestNumber(ref rd);
            return new TestProperFraction(n, d);
        }

        [TestMethod()]
        public static void runTest()
        {
            {
                TestProperFraction subject = new TestProperFraction(300, 100);
                Console.WriteLine("The float value of decimal is: " + subject.getDoubleValue());
                Console.WriteLine("The integer part gives: " + subject.integerpart);
                Console.WriteLine("The toString is: " + subject.ToString());
                Assert.AreEqual(3.0, subject.getDoubleValue() );
            }

            {
                TestProperFraction subject = new TestProperFraction(200, 600);
                Console.WriteLine("The float value of decimal is: " + subject.getDoubleValue());
                Console.WriteLine("The integer part gives: " + subject.integerpart);
                Console.WriteLine("The toString is: " + subject.ToString());
               
                Console.WriteLine("The float value is: "+subject.getDoubleValue());
                 Assert.AreEqual(1/3.0, subject.getDoubleValue());
             }

            {
                TestProperFraction subject = new TestProperFraction(887, 100);
                Console.WriteLine("The float value of decimal is: " + subject.decimalpart);
                Console.WriteLine("The integer part gives: " + subject.integerpart);
                Console.WriteLine("The toString is: " + subject.ToString());

                Console.WriteLine("The float value is: " + subject.getDoubleValue());
                Assert.AreEqual(887 / 100.0, subject.getDoubleValue());
            }

            {
                TestProperFraction subject = new TestProperFraction(-887, 100);
                Console.WriteLine("The float value of decimal is: " + subject.decimalpart);
                Console.WriteLine("The integer part gives: " + subject.integerpart);
                Console.WriteLine("The toString is: " + subject.ToString());

                Console.WriteLine("The float value is: " + subject.getDoubleValue());
                Assert.AreEqual(-887 / 100.0, subject.getDoubleValue());
            }

            whilelooptest:
            {
                Random r = new Random();
                while (r.Next(1000) != 0)
                {
                    Int64 n = r.Next(2000)-1000, d = r.Next(2000) - 1000;
                    Int64 n2 = r.Next(2000) - 1000, d2 = r.Next(2000) - 1000;


                    if (d == 0 || d2 == 0) continue;
                    Console.WriteLine("The numerator and the denominator are:" + n + "," + d);
                    ProperFraction pf1 = new ProperFraction(n,d);
                    Console.WriteLine("= "+pf1);

                    Console.WriteLine("The numerator and the denominator are:" + n2 + "," + d2);
                    ProperFraction pf2 = new ProperFraction(n2, d2);
                    Console.WriteLine("= " + pf2);
                    Console.WriteLine("Their sum: "+(pf1+pf2));
                    Console.WriteLine("Their difference: " + (pf1 - pf2));

                    if (Math.Abs
                        (
                        ((double)(pf1 + pf2) - (double)pf1 - (double)pf2) 
                        )
                        >=
                        Math.Pow(10, -5)
                      )
                    {
                        Console.WriteLine("The above result is INCORRECT.");
                        Thread.Sleep(300);
                        Assert.IsTrue(false);
                    }

                }

            }

            testmultiplication:
            {

                // The product of a number and its reciprecal. 
                Random rd = new Random();
                while (rd.Next(1000) != 0)
                {
                    TestProperFraction subject = TestProperFraction.getRandomTestNumber(ref rd);
                    Console.WriteLine("Subject: "+subject);
                    Console.WriteLine("Reciprecal: " + subject.getReciprecal());
                    Console.WriteLine("-> " + subject * subject.getReciprecal());
                    Console.WriteLine("Double value: " + (subject * subject.getReciprecal()).getDoubleValue());

                    bool iswrong = (
                                    Math.Abs((subject * subject.getReciprecal()).getDoubleValue() - 1.0)
                                    >= 1e-9
                                    );
                    if (iswrong)
                    {
                        Thread.Sleep(10); Assert.IsTrue(false);
                    }
                }

                //Two different number multiply together. 
                while (rd.Next(1000) != 0)
                {

                    TestProperFraction subject1 = TestProperFraction.getRandomTestNumber(ref rd);
                    TestProperFraction subject2 = TestProperFraction.getRandomTestNumber(ref rd);
                    Console.WriteLine("Subject1: " + subject1 + "~= " + subject1.getDoubleValue());
                    Console.WriteLine("Subject2: " + subject2 + "~= " + subject2.getDoubleValue());
                    Console.WriteLine("-> " + subject1 * subject2);
                    Console.WriteLine("Double value: " + (subject1 * subject2).getDoubleValue());

                    bool iswrong = (
                                        Math.Abs
                                        (
                                            subject1*subject2
                                            -
                                            (subject2.getDoubleValue())
                                            *(subject1.getDoubleValue())
                                        )
                                    >= 1e-9
                                    );
                    if (iswrong)
                    {
                        Assert.IsTrue(false);
                    }
                }


            }
        }


    }
}