using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab_4
{
    internal class Graph
    {
        private readonly List<Arc> _arr = new List<Arc>();

        public Graph() {}

        public Graph(Graph oldGraph)
        {
            _arr = oldGraph._arr;
        }

        public void Add(string strArc)
        {
            _arr.Add(new Arc(strArc));
        }

        public void FillFromFile(string path)
        {
            var fileReader = new StreamReader(path);
            string stringBuff;
            while ((stringBuff = fileReader.ReadLine()) != null) 
                Add(stringBuff); 
        }

        public List<Arc> GetArcList(string vertex)
        {
            return _arr.Where(i => i.start == vertex).ToList();
        }

        public List<string> GetVertexList()
        {
            var arr = new List<string>();
            foreach (var i in _arr)
            {
                if(!arr.Contains(i.start))
                    arr.Add(i.start);
                if (!arr.Contains(i.end))
                    arr.Add(i.end);
            }
            return arr;
        }

        public int[] BuildMatrix(ref int sz)
        {
            var arr = GetVertexList();
            var matrix = new int[arr.Count * arr.Count];
            for (var i = 0; i < arr.Count * arr.Count; i++)
                matrix[i] = int.MaxValue;

            for (var i = 0; i < arr.Count; i++) //Цикл по всем вершинам
            {
                var mas = GetArcList(arr[i]); //Берем список всех дуг
                foreach (var c in mas)//Берем 1 дугу
                {
                    var m = arr.FindIndex(x => x.Equals(c.end)); //Ищем на каком месте в arr находится c.start

                    //Console.WriteLine("{0} to {1} -> {2}", c.end, c.start, c.weight);
                    matrix[i * arr.Count + m] = c.weight;
                }
            }

            sz = arr.Count;
            return matrix;
        }

        class Point
        {
            public string name;
            public int metka;
            public Point fromMetka;
            public bool Fixed;

            public Point(string name)
            {
                this.name = name;
                this.metka = int.MaxValue;
                fromMetka = null;
                Fixed = false;
            }
        }

        public bool TestNegativeArc()
        {
            return _arr.Any(c => c.weight < 0);
        }

        public int ShortestPath(string a, string b)
        {
            if (TestNegativeArc())
                return -1;

            var path = new Graph(this);
            if (path._arr.Count == 0)
                return -1;

            var startPoint = new Point(a)
            {
                Fixed = true,
                metka = 0
            };

            var link = startPoint;
            while (link.name != b)
            {
                var mas = new List<Point>();
                foreach (var arc in _arr)
                {
                    if (arc.start != link.name)
                        continue;

                    //Ищу точку - конец дуги
                    var point = mas.Where(p => p.name == arc.end).GetEnumerator().Current;
                    if (point == null)
                    {
                        point = new Point(arc.end);
                        mas.Add(point);
                    }
                    else if (point.Fixed) continue; //если она уже закреплена, то скип

                    //Обновление метки
                    if ((link.metka + arc.weight < point.metka))
                    {
                        point.metka = arc.weight + link.metka;
                        point.fromMetka = link;
                        Console.WriteLine("Update {0} from {1}, set {2}", point.name, point.fromMetka.name, point.metka);
                    }
                }

                // Find min metka for nex iteration
                var k = new Point("s")
                {
                    metka = int.MaxValue
                };
                foreach (var p in mas.Where(p => p.metka < k.metka))
                    k = p;
                link = k;
                link.Fixed = true;
                Console.WriteLine("Ste linked point {0}", link.name);
            }

            var iOut = link.metka;
            var sOut = "";
            while (link != startPoint)
            {
                sOut += link.name + " >- ";
                link = link.fromMetka;
            }

            sOut += a;
            sOut = Reverse(sOut);
            Console.WriteLine(sOut);
            return iOut;
        }
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public int ShortestPathFloid(string A, string B)
        {
            var sz = 0;
            var matrix = BuildMatrix(ref sz);
            FloidAlgorithm(ref matrix, sz);
            var arr = GetVertexList();
            return matrix[arr.FindIndex(x => x.Equals(A)) * arr.Count + arr.FindIndex(x => x.Equals(B))];
        }

        private static void FloidAlgorithm(ref int[] matrix, int sz)
        {
            for (var idx = 0; idx < sz; ++idx)
            {
                for (var iTop = 0; iTop < sz; ++iTop)
                {
                    if (matrix[iTop * sz + idx] == int.MaxValue)
                        continue;
                        
                    for (var iLeft = 0; iLeft < sz; ++iLeft)
                    {
                        if (matrix[idx * sz + iLeft] == int.MaxValue)
                            continue;

                        var distance = matrix[iTop * sz + idx] + matrix[idx * sz + iLeft];
                        if (matrix[iTop * sz + iLeft] > distance)
                        {
                            matrix[iTop * sz + iLeft] = distance;
                        }
                    }
                }
            }
        }

        //Go to deep
        public void Print()
        {
            var vertexList = GetVertexList(); //Все вершины
            //vertexList.Reverse(); // Разверну для теста BUG
            var vizited = new List<string>(); //Список посещенных
            var buffArr = new List<string>(); //Буффер для deep обхода

            while (vizited.Count != vertexList.Count) //Пока не посетим все вершины
            {
                if(buffArr.Count == 0) //Если буфер пуст
                    buffArr.Add(vertexList.Find(str => !vizited.Contains(str)));
                //Берем первый не  посещенный

                while (buffArr.Count != 0)
                {
                    var work = buffArr.Last(); //Берем последний
                    var arcList = GetArcList(work); //Берем все его дуги
                    buffArr.RemoveAt(buffArr.Count - 1); //Убираем из буфера
                    vizited.Add(work); //Отмечаем посещенным
                    Console.Write(work + " ");
                    
                    foreach(var a in arcList) //Добавляем все не посещенные
                        if(!vizited.Contains(a.end))
                            buffArr.Add(a.end);
                }
            }
            Console.WriteLine();
        }

        private void PrintArray(IReadOnlyList<int> matrix, int sz)
        {
            var arr = GetVertexList();
            Console.Write("  ");
            for (var i = 0; i < sz; i++)
                Console.Write(arr[i] + " ");
            Console.WriteLine();

            for (var i = 0; i < sz; i++)
            {
                Console.Write(arr[i] + " ");
                for (var j = 0; j < sz; j++)
                {
                    var iOut = matrix[i * sz + j];
                    iOut = (iOut == int.MaxValue) ? 0 : iOut;
                    Console.Write(iOut + " ");
                }

                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}