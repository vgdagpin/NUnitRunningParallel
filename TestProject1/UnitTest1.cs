namespace TestProject1
{
    public class Tests
    {
        [Test]
        public async Task Test1()
        {
            await Task.Delay(Random.Shared.Next(3, 5) * 1000);

            var id = TestContext.Parameters.Get("IDFromParameter");

            TestContext.WriteLine($"From TestProject1.Tests.Test1 ID: {id}");
        }
    }
}