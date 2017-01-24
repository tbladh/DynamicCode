using System;
using System.Diagnostics;
using DynamicCode.Extensions;
using NUnit.Framework;

namespace DynamicCode.Tests
{
    [TestFixture]
    public class DynamicAssemblyTests
    {
       

        [Test]
        public void BaseLineTest()
        {
            dynamic d = new DynamicAssembly(Code.HelloWorld);
            dynamic test = d.CodeRunner.Generated.Test.New();
            var reverse = test.Reverse("Hello World!");
            Assert.That(reverse, Is.EqualTo("!dlroW olleH"));
        }

        [Test]
        public void FluentTests()
        {
            var sum = Code.RegularClass.Build().Calc.New().Add(1, 1);

            Assert.That(sum, Is.EqualTo(2));

            sum = Code.StaticClass.Build().Calc.Add(1, 1);

            Assert.That(sum, Is.EqualTo(2));

        }

        [Test]
        public void FluentTestWithErrors()
        {
            try
            {
                var sum = Code.SyntaxErrors.Build().Calc.Add(1, 1);
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.InstanceOf(typeof (DeferredBuildException)));
                var buildException = (DeferredBuildException) ex;
                Assert.That(buildException.BuildMessages, Is.Not.Null);
                Assert.That(buildException.BuildMessages.Count, Is.Not.EqualTo(0));
            }

        }

        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 1)]
        [TestCase(3, ExpectedResult = 2)]
        [TestCase(4, ExpectedResult = 3)]
        [TestCase(5, ExpectedResult = 5)]
        [TestCase(6, ExpectedResult = 8)]
        [TestCase(48, ExpectedResult = 4807526976L)]
        [TestCase(92, ExpectedResult = 7540113804746346429L)]
        public long FibonacciTest(int n)
        {
            var calc = Code.Fibonacci.Build().Calc.New();
            return calc.Fibonacci(n);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void PerformanceTest(bool assert)
        {

            // Regular static method.
            var watch = new Stopwatch();
            watch.Start();
            for (var i = 0; i < 100000; i++)
            {
                var sum = Code.FibonacciCompiled(92);
                if (assert) Assert.That(sum, Is.EqualTo(7540113804746346429L));
            }
            watch.Stop();
            Debug.WriteLine($"Regular:                 {watch.ElapsedMilliseconds}");

            // Dynamically created instance.
            var calc = Code.Fibonacci.Build().Calc.New();
            watch = new Stopwatch();
            watch.Start();
            for (var i = 0; i < 100000; i++)
            {
                var sum = calc.Fibonacci(92);
                if (assert) Assert.That(sum, Is.EqualTo(7540113804746346429L));
            }
            watch.Stop();
            Debug.WriteLine($"Dynamic Instance:        {watch.ElapsedMilliseconds}");

            // Method.Invoke on static method.
            watch = new Stopwatch();
            watch.Start();
            var calcClass = Code.FibonacciStatic.Build().Calc;
            for (var i = 0; i < 100000; i++)
            {
                var sum = calcClass.Fibonacci(92);
                if (assert) Assert.That(sum, Is.EqualTo(7540113804746346429L));
            }
            watch.Stop();
            Debug.WriteLine($"Dynamic Static (Invoke): {watch.ElapsedMilliseconds}");

        }

        [Test]
        public void OverloadsTest()
        {
            var calc = Code.Overloads.Build().Calc;
            var sum = calc.Add(1, 1);

            Assert.That(sum, Is.EqualTo(2));

            sum = calc.Add(1, 1, 1);


            Assert.That(sum, Is.EqualTo(3));

        }
    }
}
