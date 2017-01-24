namespace DynamicCode.Tests
{
    public static class Code
    {
        public const string HelloWorld =
            @"using System;
            using System.Text;
            namespace CodeRunner.Generated
            {
              public class Test
              {
                public string Reverse(string message)
                {
                   var sb = new StringBuilder();
                   for (var i = message.Length-1; i >= 0; i--)
                   {
                     sb.Append(message[i]);
                   }
                   return sb.ToString();
                }
              }
            }";

        public const string RegularClass =
            @"using System;
            namespace CodeRunner.Generated
            {
                public class Calc
                {
                    public int Add(int a, int b)
                    {
                        return a + b;
                    }
                }
            }";

        public const string StaticClass =
            @"using System;
            namespace CodeRunner.Generated
            {
                public static class Calc
                {
                    public static int Add(int a, int b)
                    {
                        return a + b;
                    }
                }
            }";

        public const string SyntaxErrors =
            @"using System
            namespace CodeRunner.Generated
            {
                public static class Calc
                {
                    public int Add(int a = 0, int b)
                    {
                        return a + b;
                    }
                }

            }";

        public const string Fibonacci =
            @"using System;
            namespace CodeRunner.Generated
            {
                public class Calc
                {
                    public long Fibonacci(int n)
                    {
                        long a = 0;
                        long b = 1;
                        if (n == a) return a;
                        if (n == b) return b;
                        for (var i = 0; i < (n-1); i++)
                        {
                            var t = b;
                            b = a + b;
                            a = t;
                        }
                        return b;
                    }
                }
            }";

        public const string FibonacciStatic =
            @"using System;
            namespace CodeRunner.Generated
            {
                public static class Calc
                {
                    public static long Fibonacci(int n)
                    {
                        long a = 0;
                        long b = 1;
                        if (n == a) return a;
                        if (n == b) return b;
                        for (var i = 0; i < (n-1); i++)
                        {
                            var t = b;
                            b = a + b;
                            a = t;
                        }
                        return b;
                    }
                }
            }";

        public const string Overloads =
            @"using System;
            namespace CodeRunner.Generated
            {
                public static class Calc
                {
                    public static int Add(int a, int b)
                    {
                        return a + b;
                    }
                    public static int Add(int a, int b, int c)
                    {
                        return a + b + c;
                    }
                }
            }";

        public static long FibonacciCompiled(int n)
        {
            long a = 0;
            long b = 1;
            if (n == a) return a;
            if (n == b) return b;
            for (var i = 0; i < (n - 1); i++)
            {
                var t = b;
                b = a + b;
                a = t;
            }
            return b;
        }

    }
}
