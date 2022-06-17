using System;

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
        private static void Main()
        {
            var graph = new Graph();
            graph.FillFromFile("C:\\Users\\Mak\\OneDrive\\Рабочий стол\\Учеба\\Фундаменталка\\Lab_4\\input.txt");
            Console.WriteLine("Shortest path E->C  {0}",graph.ShortestPath("E", "C"));
            graph.Print();
        }
    }
}