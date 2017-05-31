using Xunit;

namespace TestPatterns.FixtureManagement
{
    public class TransientFreshFixtureTests
    {
        [Fact]
        public void InlineSetup()
        {
            const int expected = 42;
            var a = new A(expected);
            var b = new B(a);
            var c = new C(b);

            int result = c.Execute();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void DelegatedSetup()
        {
            const int expected = 42;
            var c = CreateValidC(expected);

            int result = c.Execute();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void OneBadAttribute()
        {
            var c = CreateValidC();
            c.Name = "invalid";

            bool result = c.IsValid();

            Assert.False(result);
        }

        [Fact]
        public void NamedStateReachingMethod()
        {
            var c = CreateCWithInvalidName();

            bool result = c.IsValid();

            Assert.False(result);
        }

        [Fact]
        public void TestHelperCreationMethod()
        {
            var c = TestHelper.CreateValidC();

            bool result = c.IsValid();

            Assert.True(result);
        }

        private static C CreateCWithInvalidName()
        {
            var c = CreateValidC();
            c.Name = "invalid";
            return c;
        }

        private static C CreateValidC(int value = 12)
        {
            var a = new A(value);
            var b = new B(a);
            var c = new C(b);
            return c;
        }
    }

    public class C
    {
        private readonly B _b;

        public C(B b)
        {
            _b = b;
        }

        public string Name { get; set; } = "";

        public int Execute()
        {
            return _b.Do();
        }

        public bool IsValid()
        {
            return !Name.ToLowerInvariant().Contains("invalid");
        }
    }

    public class B
    {
        private readonly A _a;

        public B(A a)
        {
            _a = a;
        }

        public int Do()
        {
            return _a.Value;
        }
    }

    public class A
    {
        public A(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}
