using System.Diagnostics;
using System.Threading;

namespace MultiThreading
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            Thread baristaThread1 = new Thread(() =>
                                               {
                                                   HireBarista("Carl")
                                                      .GoToCafe("Luke's coffee")
                                                      .MakeCoffee();
                                               },
                                               1024 * 1024 / 2);

            baristaThread1.Name = "Barista Thread 1";
            baristaThread1.IsBackground = true;
            baristaThread1.Start();
            baristaThread1.Join();

            Thread.Sleep(1000);

            ThreadPool.SetMinThreads(1, 0);
            ThreadPool.SetMaxThreads(4, 8);

            // Task : ThreadPool 에서 Thread 를 빌려다가 작업을 할당하는 클래스
            Task task1 = new Task(() =>
                                  {
                                      HireBarista("Carl")
                                         .GoToCafe("Luke's coffee")
                                         .MakeCoffee();
                                  });

            task1.Start();
            task1.Wait();
            */
            List<Task> tasks = new List<Task>();
            
            for (int i = 0; i < 10; i++)
            {
                int tmpID = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    HireBarista($"Barista{tmpID}")
                       .GoToCafe("Luke's coffee")
                       .MakeCoffee();
                }));
            }

            Task.WaitAll(tasks.ToArray());

            // Generic Task : 작업의 결과물(결과값)을 반환받고싶을 때 사용
            Task<string> taskWithResult = new Task<string>(() =>
            {
                return HireBarista($"Smart-Barista")
                          .GoToCafe("Luke's coffee")
                          .MakeCoffee()
                          .ToString();
            });
            taskWithResult.Start();
            taskWithResult.Wait();
            Console.WriteLine(taskWithResult.Result);

            List<int> list = new List<int>();
            using (IEnumerator<int> e = list.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    Console.WriteLine(e.Current);
                }
                e.Reset();
            }
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

        public Beverage MakeCoffee()
        {
            Console.WriteLine($"바리스타 {name} 은(는) 커피 추출을 시작합니다...");
            Thread.Sleep(3000);
            Console.WriteLine($"바리스타 {name} 은(는) 커피 추출을 완료했습니다...");
            return (Beverage)random.Next(0, Enum.GetValues(typeof(Beverage)).Length);
        }
    }

}