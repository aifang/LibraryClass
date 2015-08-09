using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach(var number in Iterator.OneTwoThree())
            {
                Console.WriteLine(number);
            }
            Console.ReadKey();
        }
        static void DisplayProcesses(Func<Process,Boolean> match)
        {
            var processes = new List<ProcessData>();
            foreach(var process in Process.GetProcesses())
            {
                if(match(process))
                {
                    processes.Add(new ProcessData
                    {
                        Id=process.Id,
                        Name = process.ProcessName,
                        Memory = process.WorkingSet64
                    });
                }
            }
            Console.WriteLine("Total memroy:{0} MB", processes.TotalMemory()/1024/1024);
            var top2Memory =
                processes
                .OrderByDescending(process => process.Memory)
                .Take(2)
                .Sum(process => process.Memory) / 1024 / 1024;
            Console.WriteLine("Memory consumed by the two most hungry processes: {0} MB", top2Memory);
            
            //匿名类
            var result = new
            {
                TotalMemory = processes.TotalMemory() / 1024 / 1024,
                Top2Menory = top2Memory,
                Processes = processes
            };
        }
       
    }
    class ProcessData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Int64 Memory { get; set; }
    }
    /// <summary>
    /// 扩展方法
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// 计算进程所消耗的内存总和
        /// </summary>
        /// <param name="processes"></param>
        /// <returns></returns>
        public static Int64 TotalMemory(this IEnumerable<ProcessData> processes)
        {
            Int64 result = 0;
            foreach (var process in processes)
                result += process.Memory;
            return result;
        }
    }

    /// <summary>
    /// 迭代器
    /// </summary>
    static class Iterator
    {
        public static IEnumerable<int> OneTwoThree()
        {
            Console.WriteLine("Returning 1");
            yield return 1;
            Console.WriteLine("Returning 2");
            yield return 2;
            Console.WriteLine("Returning 3");
            yield return 3;
        }

    }
}
