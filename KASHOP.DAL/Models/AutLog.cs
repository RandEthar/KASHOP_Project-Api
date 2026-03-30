using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    internal class AutLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public String Action { get; set; }
        //اسم الجدول اللي صار عليه التعديل
        public String TableName { get; set; }
        public String OldValue { get; set; }
        public String NewValue { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
