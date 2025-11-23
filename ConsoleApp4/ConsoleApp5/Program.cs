using System;
using System.Threading;
using System.IO;
using System.Linq;

public class NumberAnalysis
{
    private static int[] numbers = new int[10000];
    private static int max, min;
    private static double average;
    private static object lockObject = new object();

    public static void Main(string[] args)
    {
        Random rand = new Random();
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = rand.Next(-1000, 1001);
        }

        Console.WriteLine("=== Задание 4: Анализ чисел ===");

        Thread maxThread = new Thread(FindMax);
        Thread minThread = new Thread(FindMin);
        Thread avgThread = new Thread(CalculateAverage);

        maxThread.Start();
        minThread.Start();
        avgThread.Start();

        maxThread.Join();
        minThread.Join();
        avgThread.Join();

        Console.WriteLine($"Максимум: {max}");
        Console.WriteLine($"Минимум: {min}");
        Console.WriteLine($"Среднее: {average:F2}");

        Console.WriteLine("\n=== Задание 5: Запись в файл ===");
        Thread fileThread = new Thread(SaveToFile);
        fileThread.Start();
        fileThread.Join();

        Console.WriteLine("Данные сохранены в файл 'results.txt'");
    }

    public static void FindMax()
    {
        int localMax = numbers[0];
        for (int i = 1; i < numbers.Length; i++)
        {
            if (numbers[i] > localMax)
                localMax = numbers[i];
        }
        lock (lockObject)
        {
            max = localMax;
        }
        Console.WriteLine($"Поток максимума завершил работу: {localMax}");
    }

    public static void FindMin()
    {
        int localMin = numbers[0];
        for (int i = 1; i < numbers.Length; i++)
        {
            if (numbers[i] < localMin)
                localMin = numbers[i];
        }
        lock (lockObject)
        {
            min = localMin;
        }
        Console.WriteLine($"Поток минимума завершил работу: {localMin}");
    }

    public static void CalculateAverage()
    {
        long sum = 0;
        foreach (int num in numbers)
        {
            sum += num;
        }
        double localAvg = (double)sum / numbers.Length;
        lock (lockObject)
        {
            average = localAvg;
        }
        Console.WriteLine($"Поток среднего завершил работу: {localAvg:F2}");
    }

    public static void SaveToFile()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter("results.txt"))
            {
                writer.WriteLine("Набор чисел:");
                for (int i = 0; i < numbers.Length; i++)
                {
                    writer.Write(numbers[i] + " ");
                    if ((i + 1) % 20 == 0) 
                        writer.WriteLine();
                }
                writer.WriteLine("\n\nРезультаты анализа:");
                writer.WriteLine($"Максимум: {max}");
                writer.WriteLine($"Минимум: {min}");
                writer.WriteLine($"Среднее: {average:F2}");
                writer.WriteLine($"Количество элементов: {numbers.Length}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи в файл: {ex.Message}");
        }
    }
}