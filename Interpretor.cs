using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    /// <summary>
    /// 
    /// This calss should contain all the things we need to interpret algebric 
    /// solution we need. 
    /// </summary>
    public class Interpretor
    {

        public static readonly char[] OPERATORS={'+','-','*','/'};
        public static readonly IReadOnlyCollection<char> OPERATORSlist = new List<char>(OPERATORS);

    

        /// <summary>
        /// This method should be able to catch possible invalide expreession and sub expression and throw an exception.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [Obsolete("Please don't use this method. ")]
        public static ProperFraction evaluateSimple(IList<Object> expression)
        {
            Stack<ProperFraction> numstack = new Stack<ProperFraction>();
            Stack<char> opstack = new Stack<char>();
            //Queue q = new Queue();
           

            //iterate through a queue of number, operator,or a sub expression.
            foreach(Object ele in expression)
            {
                //if is number
                if(ele is ProperFraction)
                {
                    //put into numberstack.
                    numstack.Push(ele as ProperFraction);
                }
                // elseif operator
                else if(ele is char)
                {
                    
                    if(opstack.Count==0)// if operator stack is empty, put it in. 
                    {
                        opstack.Push((char)ele);
                    }
                    else // else that stack is not empty. 
                    {
                        char opt = (char)ele;

                        if (compare(opt, opstack.Peek()) == 1)//if it's bigger to the most recent opt in stack
                        {
                            // put it in op stack. 
                            opstack.Push(opt);
                        }

                        else //else: it's leq to the top opt.
                        {
                            while (compare(opt, opstack.Peek()) < 1 || opstack.Count != 0)//while current op leq than top one on stack || operator stack is not empty.
                            {
                                // pop top 
                                char topoperator = opstack.Pop();
                                // pop two numbers if there is no more number on top, use zero
                                ProperFraction secondnumber = numstack.Pop();
                                ProperFraction firstnumber = null;
                                if (numstack.Count != 0) firstnumber = numstack.Pop();
                                //compute the top two number using the operator, the order matters. 
                                ProperFraction res = compute(firstnumber, secondnumber, opt);
                                //put the number back to the number stack. 
                                numstack.Push(res);
                            }
                            //put that operator in the opt stack? 
                            opstack.Push(opt);
                        }
                    }
                }
                else//else it must be sub expression
                {
                    if (!(ele is IList<Object>))// This shit if not a list
                    {
                        throw new Exception("Bad element: "+ele);
                    }

                    //Turn the sub expression into a number. 
                    ProperFraction subexpressionnumber = evaluateSimple((IList<Object>)ele);
                    numstack.Push(subexpressionnumber);
                }  
            }
            // Evaluate the remaining expression empty the stack;
            // return result. 

            ProperFraction subexpressionresult = evalueteStacks(numstack,opstack);
            if (subexpressionresult == null) throw new Exception("Expression or sub expression cannot be evaluated. "); 
            return subexpressionresult;

        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="infix">
        /// List of type In64 and type char, the element should be integers, operators, parenthesis. 
        /// </param>
        /// <returns>
        /// null if error occured. 
        /// </returns>
        public static Queue<Object> infixtToPostFix(IList<Object> infix)
        {


            Queue<Object> result = new Queue<Object>();
            Stack<char> optstack = new Stack<char>();
            foreach (Object obj in infix)
            {
                if(obj is Int64)// If the object is number, add to output queue. 
                {
                    result.Enqueue(obj);
                }
                else if(obj is char && (char)obj=='(')//else if the object is '(', put it into the operator stack. 
                {
                    optstack.Push('(');
                }
                else if(obj is char && (char)obj == ')')//else if the object is ')'
                {
                    while
                        (
                        optstack.Count!=0
                        &&!optstack.Peek().Equals((Object)'(')
                        )//while stack is not empty and the most recent element in the stack is not '('
                    {
                        result.Enqueue(optstack.Pop());
                        //pop it out and add it to output queue. 
                    }
                    //The stack is empty or the most recent element is '('
                    //discard '(' from the stack, or if stack is empty, return null; 
                    if (optstack.Count == 0)  throw new Exception();
                    optstack.Pop();
                }
                else//Else the object must be an operator. 
                {
                    if (!(obj is char)||!OPERATORSlist.Contains((char)obj) ) throw new Exception();
                    char currentoperator = (char)obj;
                    // while stack is not empty and the operator is less than or equal to the most recent operator 
                    //on the stack
                    while(
                        optstack.Count!=0
                        && 
                        compare(currentoperator,optstack.Peek())<=0
                        )
                    {
                        result.Enqueue(optstack.Pop());
                        //pop the operator and add it to the output queue. 
                    }
                    optstack.Push(currentoperator);

                }

            }

            //pop all remaining element from the stack to the queue. 
            while (optstack.Count != 0)
            {
                result.Enqueue(optstack.Pop());
            }
            return result; 
        }


        public static ProperFraction evaluatePostFix(Queue<Object> postFix)
        {


        }


        public static bool isUnaryOpt(char arg)
        {
            return arg == '/';
        }
        /// <summary>
        /// If -1 is returned, the operator is invalid. 
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public static int getOptRank(char opt)
        {
            switch (opt)
            {
                case '+':
                    return 1;
                case '-':
                    return 1;
                case '*':
                    return 2;
                case '/':
                    return 2;
            }

            //Parenthesis has the lowest rank. 
            return -1; 
        }

        /// <summary>
        /// If first bigger than second, return 1, return 0 iff equal, smaller iff -1 returned. 
        /// </summary>
        /// <returns></returns>
        public static int compare(char arg1, char arg2)
        {
            return getOptRank(arg1) - getOptRank(arg2);
        }

        /// <summary>
        /// method is tested. 
        /// <para>
        /// This method takes in two number and an operator and return the result. 
        /// </para>
        /// If there is only one number is not null, this method will assume the operator is '-'. 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// Null is the computation cannot be done or error has occured. 
        /// </returns>
        public static ProperFraction compute(ProperFraction a, ProperFraction b, char opt)
        {
            // the operator is valide: 
            if (getOptRank(opt) == -1) return null;
            // If there is one number at least: 
            if (a == null && b == null) return null;

            //At this point, the operator will be valide and there will be at least one numebr. 

            if (a != null && b != null)//Both bumber is not null
            {
                switch (opt)
                {
                    case '+':
                        return a + b;
                    case '-':
                        return a - b;
                    case '*':
                        return a * b;
                    case '/':
                        return a / b;
                }
                throw new Exception("Something wrong, please check consistency on logic.");
            }
            //One of the number is null and the operator must be unary: '-'.
            return a == null ? -b:-a;
        }


        /// <summary>
        /// 
        /// This method is an extension of evaluate simple. 
        /// </summary>
        /// <param name="numberstack"></param>
        /// <param name="optstack"></param>
        /// <returns>
        /// null if the input cannot evalueted. 
        /// </returns>
        [Obsolete("Outside the scope.")]
        public static ProperFraction evalueteStacks(Stack<ProperFraction> numberstack, Stack<char> optstack)
        {
            //make sure the input is both not null: 
            if (numberstack == null || optstack == null) return null;

            
             //If there is no more operator in the stack, that means there is only one number in the number stack, and it's 
            if(optstack.Count==0)// it's the base case:
            {
                //check if there is really one number in the stack: 
                if (numberstack.Count != 1) throw new Exception("Something shouldn't happen hapepened.");
                return numberstack.Pop();
            }

            //Comfirm that there is a Operator in the stack. 
            char opt = optstack.Pop();
            ProperFraction secondnumber = numberstack.Pop();
            ProperFraction firstnumber = null;
            if(numberstack.Count!=0) firstnumber = numberstack.Pop();
            ProperFraction computedresult = compute(firstnumber,secondnumber,opt);

            if (computedresult == null) return null; //This is hit if the number and the operator cannot be computed. 
            numberstack.Push(computedresult);
            return evalueteStacks(numberstack, optstack);
        }


        /// <summary>
        /// <para>
        /// This class take in a string and validate whether it's a 
        /// algebaric expression or not, if it is, it will parse into a list containing 
        /// integers or operator. </para>
        /// It also checks if the parenthesises are balanced. 
        /// </summary>
        public class ExpressionDiegest
        {
            protected String sourceexpression;
            protected String diagestedexpression;
            protected String[] splitedexpression ;

            protected IList<Object> castplistedexpression;

            public ExpressionDiegest(String expression)
            {
                sourceexpression = expression;
                if (!checkChar()) throw new ExpressionDiegestError("Character Issue.");
                if (!checkParenthesis()) throw new ExpressionDiegestError("Parentesis Issue.");
                addSpace();
                parseExpression();
                castElements();
            }

            public IList<Object> getCastSplittedExpression()
            {
                return castplistedexpression;
            }

            /// <summary>
            /// <para>
            /// Only integers, parenthesis and elementary operators are allowed.
            /// </para>
            /// <para>
            /// The prenthesis must be matched. 
            /// </para>
            /// </summary>
            /// <returns></returns>
            protected bool checkChar()
            {
                foreach (char c in sourceexpression)
                {
                    if (c < 0 && c > 9)//not a number
                    {
                        if (!OPERATORSlist.Contains(c))//not a operator. 
                        {
                            return c == ')' || c == '(';
                        }
                    }
                }
                return true;
            }

            protected bool checkParenthesis()
            {
                Int32 sum = 0;
                foreach (char c in this.sourceexpression)//foreach char in the string
                {
                    if (c != ')' || c != '(') //Not '(' or ')' skip
                        continue;
                    if (c == '(') // if it's left (, plus one
                        sum++;
                    else sum--;// if it's right ), minus one
                    if (sum < 0)// if -1, return false; 
                        return false;
                }
                return true;
            }


            /// <summary>
            /// This method is totally wrong. 
            /// </summary>
            /// <returns></returns>
            protected bool checkNumberAndOperator()
            {
                Int32 sum = 1;
                foreach (char c in this.sourceexpression)
                {
                    if (c <= '9' || c >= '0') sum++;//add one if this is a number.
                    else if (OPERATORSlist.Contains(c)) sum--;//else minus one if this is a operator.
                    else sum = 1;//else this must be parenthesis, it will clear the sum to 1. 
                    if (sum < 0) return false; //if it's smaller than zero, there is something wrong with the expression. 
                }
                return true;
            }


            /// <summary>
            /// 
            /// This method modify the current expression by adding space to left side and right side of the 
            /// operators or parenthesis making it easier to parse using split. 
            /// </summary>
            protected void addSpace()
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in sourceexpression)
                {
                    if (!OPERATORSlist.Contains(c) && c != ')' && c != '(')//if it's not an operator or parenthesis
                    {
                        sb.Append(c);
                    }
                    else//It is a operator or prenthesis or something unexpected. 
                    {
                        sb.Append(' ');
                        sb.Append(c);
                        sb.Append(' ');
                    }
                }
                this.diagestedexpression = sb.ToString();
            }
            /// <summary>
            /// Parse the expression into units of operator, integers, or parenthesis. 
            /// </summary>
            protected void parseExpression()
            {
                String[] splitedexpression = Regex.Split(diagestedexpression, @"\s+");
                this.splitedexpression = splitedexpression;
            }


            /// <summary>
            /// 
            /// Cast the splitted string into a list of integers and operators. 
            /// </summary>
            protected void castElements()
            {
                IList<Object> expression = new List<Object>();
                foreach (String str in splitedexpression)
                {

                    if (str.Length == 0) continue; // Skip empty string. 

                    bool isinteger = Regex.IsMatch(str, @"\d{1,9}");
                    bool OptOrParen = str.Length == 1
                                      &&
                                        ( 
                                        OPERATORSlist.Contains((char)str.ElementAt(0))
                                        || (char)str.ElementAt(0) == '(' || (char)str.ElementAt(0) == ')'
                                         );
                    if (isinteger) expression.Add(Int64.Parse(str));
                    else if (OptOrParen) expression.Add(str.ElementAt(0));
                    else return; // something is wrong is this line is executed. 
                }
                this.castplistedexpression = expression;
            }
                                             
            public override string ToString()
            {
                String s = this.sourceexpression;
                s += '\n';
                s += this.diagestedexpression;
                s += "\nCast Splitted Expression: ";
                s += String.Join(" , ",this.castplistedexpression);
                return s;
            }
        }
                       
    }

    [Serializable]
    internal class ExpressionDiegestError : Exception
    {
        public ExpressionDiegestError()
        {
        }

        public ExpressionDiegestError(string message) : base(message)
        {
        }

        public ExpressionDiegestError(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExpressionDiegestError(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
