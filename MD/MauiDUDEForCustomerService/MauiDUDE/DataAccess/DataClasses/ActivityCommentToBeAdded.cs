using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class ActivityCommentToBeAdded
    {
        public string ARC { get; set; }
        public string Comment { get; set; }

        public ActivityCommentToBeAdded(string arc, string comment)
        {
            ARC = arc;
            Comment = comment;
        }
    }
}
