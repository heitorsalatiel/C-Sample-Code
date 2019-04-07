using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching
{
    public class Job
    {
        public string Name { get; set; }
        public DateTimeOffset? Date { get; set; }
        public Regions Region { get; set; }
        public Semesters Semester { get; set; }
        public Status Status { get; set; }
    }

    public enum Semesters
    {
        First_Semester = 1,
        Second_Semester = 2
    } 

    public enum Status
    {
        On = 1,
        Off = 2
    }

    public enum Regions
    {
        R1 = 1,
        R2 = 2,
        R3 = 3,
        R4 = 4,
        R5 = 5
    }
}
