using System;
using System.Threading;

public class NumberRangeThreads
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Задание 1: Базовый поток ===");
        Thread basicThread = new Thread(DisplayNumbersBasic);
        basicThread.Start();
        basicThread.Join();

        Console.WriteLine("\n=== Задание 2: Пользовательский диапазон ===");

        Console.Write("Введите начало диапазона: ");
        int start = int.Parse(Console.ReadLine());
        Console.Write("Введите конец диапазона: ");
        int end = int.Parse(Console.ReadLine());

        Thread customThread = new Thread(() => DisplayNumbersCustom(start, end));
        customThread.Start();
        customThread.Join();

        Console.WriteLine("\n=== Задание 3: Многопоточность ===");
        Console.Write("Введите количество потоков: ");
        int threadCount = int.Parse(Console.ReadLine());
        Console.Write("Введите начало диапазона: ");
        start = int.Parse(Console.ReadLine());
        Console.Write("Введите конец диапазона: ");
        end = int.Parse(Console.ReadLine());

        StartMultipleThreads(threadCount, start, end);
    }

    public static void DisplayNumbersBasic()
    {
        for (int i = 0; i <= 50; i++)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: {i}");
            Thread.Sleep(50);
        }
    }

    public static void DisplayNumbersCustom(int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: {i}");
            Thread.Sleep(50);
        }
    }

    public static void StartMultipleThreads(int threadCount, int start, int end)
    {
        Thread[] threads = new Thread[threadCount];
        int rangeSize = (end - start + 1) / threadCount;

        for (int i = 0; i < threadCount; i++)
        {
            int threadStart = start + i * rangeSize;
            int threadEnd = (i == threadCount - 1) ? end : threadStart + rangeSize - 1;
            int threadIndex = i;

            threads[i] = new Thread(() =>
            {
                Console.WriteLine($"Поток {threadIndex + 1} начал работу: {threadStart}-{threadEnd}");
                for (int num = threadStart; num <= threadEnd; num++)
                {
                    Console.WriteLine($"Поток {threadIndex + 1}: {num}");
                    Thread.Sleep(30);
                }
                Console.WriteLine($"Поток {threadIndex + 1} завершил работу");
            });

            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
    }
}