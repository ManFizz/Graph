using System;
using System.Collections.Generic;
using System.IO;

/*
 *  Граф: 
 *  ориентированный
 *  взвешенный
 *
 *  Внутреннее представление:
 *  вектор векторов
 *
 *  Обход:
 *  в глубину (нерекурсивный) (ребер/дуг)
 *
 *  Алгоритм:
 *  КП (из заданной в заданную - выводить путь) (есть дуги отрицательного веса)
 */
namespace Lab_4
{
    internal class Program
    {
        private static int RandomMatrixToFile(string path)
        {
            const int minSize = 4;
            const int maxSize = 25;

            const int bufSize = maxSize - minSize;
            var rnd = new Random();
            var count = rnd.Next(bufSize) + minSize;
            var sOut =  count.ToString() + '\n';
            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < count; j++)
                    sOut += (((rnd.Next() % 2) == 0 ||  i == j) ? 0 : rnd.Next() % 1000 - 5) + " ";
                sOut += '\n';
            }
            var sw = new StreamWriter(path);
            sw.Write(sOut);
            sw.Close();
            return count;
        }

        private static void Main()
        {
            const string pathFile = "input.txt";
            var graph = new Graph(pathFile);
            const int a = 2;
            const int b = 3;
            //Default test
            Console.WriteLine("Shortest path {0} to {1}\n", a, b);
            Console.WriteLine("Weight: {0}", graph.ShortestPath(a, b));
            graph.Print();
            graph.PrintMatrix();

            //Crash test

            var rnd = new Random();
            while (true)
            {
                var count = RandomMatrixToFile(pathFile);
                var c = rnd.Next() % count;
                var d = rnd.Next() % count;
                graph.FillFromFile(pathFile);
                var result1 = 0;
                var except = false;
                try
                {
                    result1 = graph.ShortestPath(c, d);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    except = true;
                }
                var result2 = graph.FloidAlgorithm(c, d);
                if (result2 != result1 &&  !except)
                {
                    Console.WriteLine("Shortest path {0} to {1} - ERROR", c, d);
                    throw new Exception("NO NO NO NOOOOOO");
                }
                
                graph.PrintMatrix();
                graph.Print();

                Console.Read();
            }
        }
    }
}