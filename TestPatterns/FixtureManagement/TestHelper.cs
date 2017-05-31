namespace TestPatterns.FixtureManagement
{
    public static class TestHelper
    {
        public static C CreateValidC(int value = 12)
        {
            var a = new A(value);
            var b = new B(a);
            var c = new C(b);
            return c;
        }
    }
}