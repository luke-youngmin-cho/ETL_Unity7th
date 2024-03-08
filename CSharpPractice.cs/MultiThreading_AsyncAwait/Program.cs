using System.Collections;

namespace MultiThreading_AsyncAwait
{
    internal class Program
    {
        public static int BeverageCount;
        public static object countLock = new object();

        static void Main(string[] args)
        {
            List<Task<string>> tasks = new List<Task<string>>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(MakeBaristaToWork());
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(BeverageCount);
            return;

            Task<string> task = MakeBaristaToWork();
            task.Wait();

            Console.WriteLine(task.Result);

            List<Task<Beverage>> baristaTasks = new List<Task<Beverage>>();
            Barista barista = HireBarista("Super barista");
            for (int i = 0; i < 10; i++)
            {
                baristaTasks.Add(barista.MakeCoffee());
            }

            Task.WaitAll(baristaTasks.ToArray());

            Console.WriteLine(BeverageCount);

            //for (int i = 0; i < baristaTasks.Count; i++)
            //{
            //    Console.WriteLine(baristaTasks[i].Result);
            //}
        }

        static async Task<string> MakeBaristaToWork()
        {
            Beverage result = await HireBarista("Smart-Barista")
                                        .GoToCafe("Luke's Coffee")
                                        .MakeCoffee();

            lock (countLock)
            {
                for (int i = 0; i < 100000; i++)
                {
                    BeverageCount++;
                }
            }
            return result.ToString();
        }

        static Barista HireBarista(string nickname)
        {
            Barista barista = new Barista(nickname);
            return barista;
        }
    }

    public enum Beverage
    {
        Aspresso,
        Latte,
        Lemonade,
    }


    public class Barista
    {
        public Barista(string name)
        {
            this.name = name;
        }

        public string name { get; private set; }
        private Random random = new Random();

        public Barista GoToCafe(string cafeName)
        {
            Console.WriteLine($"바리스타 {name} 은(는) {cafeName} 카페로 출근합니다...");
            return this;
        }

        public async Task<Beverage> MakeCoffee()
        {
            Console.WriteLine($"바리스타 {name} 은(는) 커피 추출을 시작합니다...");
            await Task.Delay(3000);
            Console.WriteLine($"바리스타 {name} 은(는) 커피 추출을 완료했습니다...");
            return (Beverage)random.Next(0, Enum.GetValues(typeof(Beverage)).Length);
        }
    }
}