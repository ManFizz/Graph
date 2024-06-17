using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab_4
{
    internal class Graph
    {
        //Parametrs
        private int?[][] _arr;
        private int _count;
        //End parametrs

        //Constructors
        public Graph(string str)
        {
            FillFromFile(str);
        }

        public Graph(int l)
        {
            _count = l;
            _arr = InitArray(l);
        }

        public Graph(Graph oldGraph)
        {
            _arr = oldGraph._arr;
            _count = oldGraph._count;
        }
        //End constructors


        //Procedure reading a matrix from a file
        //Warn - auto clear matrix
        //https://github.com/MadVlad1715/GraphAlgorithms/blob/main/GraphAlgorithms/Graph.cs
        public void FillFromFile(string path)
        {
            var fileReader = new StreamReader(path);
            _count = int.Parse(fileReader.ReadLine() ?? string.Empty);
            _arr = InitArray(_count);
            for (var i = 0; i < _count; i++)
            {
                var stringBuff = fileReader.ReadLine();
                if (stringBuff == null)
                    throw new Exception("file broken");
                stringBuff = stringBuff.Trim();
                var parseResult = stringBuff.Split(' ');
                if (parseResult.Length != _count)
                    throw new Exception("Broken string " + stringBuff);

                //int c = _arr[0].Count;
                for (var j = 0; j < _count; j++)
                    _arr[i][j] = int.Parse(parseResult[j]);
            }

            //PrintMatrix(_arr, _count);
            fileReader.Close();
        }

        // Основная функция, которая находит кратчайший расстояние от src до всех остальных вершин
        // с использованием алгоритма Беллмана-Форда. Функция также обнаруживает цикл отрицательного веса
        public int ShortestPath(int src, int end)
        {
            // Инициализируйте расстояние между всеми вершинами как бесконечное.
            var arr = InitArray(_count);
            _arr.CopyTo(arr, 0);
            var dis = new int?[_count];
            for (var i = 0; i < _count; i++)
                dis[i] = int.MaxValue;

            for (var i = 0; i < _count; i++)
                for (var u = 0; u < _count; u++)
                    if (arr[i][u] == 0)
                        arr[i][u] = null;

            dis[src] = 0;
            
            for (var i = 0; i < _count - 1; i++)
                for (var u = 0; u < _count; u++)
                    for (var v = 0; v < _count; v++)
                        if (arr[u][v] != null && dis[u] != int.MaxValue && dis[u] + arr[u][v] < dis[v])
                            dis[v] = dis[u] + arr[u][v];

            for (var u = 0; u < _count; u++)
                for (var v = 0; v < _count; v++)
                    if (arr[u][v] != null && dis[u] + arr[u][v] < dis[v])
                        throw new Exception("Graph contains negative weight cycle");

            for (var i = 0; i < _count; i++)
                if (dis[i] == int.MaxValue)
                    dis[i] = 0;

            return dis[end] ?? int.MinValue;
        }

        //Floid algorithm
        public int FloidAlgorithm(int a, int b)
        {
            var matrix = _arr.ToList();
            var sz = _count;
            for(var i = 0; i < sz; i++)
                for(var j = 0; j < sz; j++)
                    if (i != j && matrix[i][j] == null)
                        matrix[i][j] = int.MaxValue;

            for (var idx = 0; idx < sz; ++idx)
            {
                for (var iTop = 0; iTop < sz; ++iTop)
                {
                    if (matrix[iTop][idx] == int.MaxValue)
                        continue;

                    for (var iLeft = 0; iLeft < sz; ++iLeft)
                    {
                        if (matrix[idx][iLeft] == int.MaxValue)
                            continue;

                        var distance = matrix[iTop][idx] + matrix[idx][iLeft];
                        if (matrix[iTop][iLeft] > distance) 
                            matrix[iTop][iLeft] = distance; 
                    }
                }
            }
            
            return matrix[a][b] == int.MaxValue ? -1 : matrix[a][b] ?? 0;
        }
        //End floid algorithm

        private class Point
        {
            public int name;
            public int from;
            public Point(int name, int from)
            {
                this.name = name;
                this.from = from;
            }
        }
        ////Traversing the graph in depth
        public void PrintTest(int s)
        {
            // Initially mark all vertices as not visited
            var visited = new bool[_count];

            // Create a stack for DFS
            var stack = new Stack<int>();

            // Push the current source node
            stack.Push(s);

            while (stack.Count > 0)
            {
                // Pop a vertex from stack and print it
                s = stack.Peek();
                stack.Pop();

                // Stack may contain same vertex twice. So
                // we need to print the popped item only
                // if it is not visited.
                if (visited[s] == false)
                {
                    Console.Write(s + " ");
                    visited[s] = true;
                }

                // Get all adjacent vertices of the popped vertex s
                // If a adjacent has not been visited, then push it
                // to the stack.
                for(var i = 0; i < _count; ++i)
                {
                    if (_arr[s][i] != 0 && _arr[s][i] != null && !visited[i])
                        stack.Push(i);
                }

            }
            Console.WriteLine();
        }
        public void Print()
        {
            var vizited = new List<int>(); //Список посещенных
            var buffArr = new List<Point>(); //Буффер для deep обхода
            string sOut1 = "   ";
            string sOut2 = "";
            while (vizited.Count != _count) //Пока не посетим все вершины
            {
                if (buffArr.Count == 0) //Если буфер пуст
                {
                    //Берем первый не  посещенный
                    for (var j = 0; j < _count; j++)
                    {
                        if (vizited.Contains(j))
                            continue;

                        buffArr.Add(new Point(j, -1));
                        break;
                    }
                }

                while (buffArr.Count != 0)
                {
                    var work = buffArr.Last(); //Берем последний
                    var arcList = new List<int>(); //GetArcList(work); //Берем все его дуги
                    buffArr.RemoveAt(buffArr.Count - 1); //Убираем из буфера
                    vizited.Add(work.name); //Отмечаем посещенным
                    if (work.from != -1)
                        sOut1 += (_arr[work.from][work.name] + " ").PadLeft(4);
                    sOut2 += (work.name + " ").PadLeft(4);

                    for (var j = 0; j < _count; j++)
                        if (_arr[work.name][j] != null && !vizited.Contains(j))
                        {
                            for (var k = 0; k < buffArr.Count; k++)
                            {
                                if (buffArr[k].name != j)
                                    continue;

                                buffArr.RemoveAt(k);
                                break;
                            }

                            buffArr.Add(new Point(j, work.name));
                        }
                }
            }
            Console.WriteLine("{0}\n{1}", sOut1, sOut2);
        }

        //Print matrix
        public void PrintMatrix()
        {
            PrintMatrix(_arr, _count);
        }

        //Private zone
        private static void PrintMatrix(int?[][] matrix, int sz)
        {
            for (var i = 0; i < sz; i++)
            {
                for (var j = 0; j < sz; j++)
                {
                    var iOut = matrix[i][j];
                    iOut = (iOut == int.MaxValue) ? 0 : iOut;
                    Console.Write(iOut + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private bool TestNegativeArc()
        {
            return _arr.Any(c => c.Any(m => m < 0));
        }
        //End private zone

        //Utils
        private static int?[][] InitArray(int size)
        {
            var arr = new int?[size][];
            for (var i = 0; i < size; i++)
                arr[i] = new int?[size];

            return arr;
        }

        public static string Reverse(string s)
        {
            var charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        //End Utils
    }
}