using System;

namespace Dagobah
{
    public class Request
    {
        public double P { get; set; } //Priority: It's abbreviated to keep the grid looking neat.
        public bool Top { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public string Court { get; set; }
        public string Status { get; set; }
        public string Summary { get; set; }
    }//Request

    public class User
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }//User
}//namespace
