using System;

namespace Uheaa.Common 
{
    public class Order : Attribute
    {
        public int PropertyOrder {get;set;}
        public Order(int order)
        {
            PropertyOrder = order;
        }
    }
}
