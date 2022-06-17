using System;

namespace Lab_4
{
    internal class Arc
    {
        public string start;
        public string end;
        public int weight;

        /*
         * string format: "[Name vertex 1 :string] [Name vertex 2 :string] [weight :int]"
         * example "pes dos 3"
         */
        public Arc(string parse)
        {
            var parseResult = parse.Split(' ');
            if (parseResult.Length != 3)
                throw new Exception("Broken string " + parse);
            start = parseResult[0];
            end = parseResult[1];
            if(!int.TryParse(parseResult[2], out weight))
                throw new Exception("Broken weight " + parse);
        }
    }
}