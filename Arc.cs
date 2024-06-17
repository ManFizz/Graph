using System;

namespace Lab_4
{
    internal class Arc
    {
        public string Start;
        public string End;
        public int Weight;

        /*
         * string format: "[Name vertex 1 :string] [Name vertex 2 :string] [weight :int]"
         * example "pes dos 3"
         */
        public Arc(string parse)
        {
            var parseResult = parse.Split(' ');
            if (parseResult.Length != 3)
                throw new Exception("Broken string " + parse);
            Start = parseResult[0];
            End = parseResult[1];
            if(!int.TryParse(parseResult[2], out Weight))
                throw new Exception("Broken weight " + parse);
        }
    }
}