using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching
{
    class Program
    {
        static List<Job> jobList {
            get {
                return new List<Job>
                {
                    new Job {Date = DateTime.Now, Region = Regions.R1 , Name = "Jb1", Semester = Semesters.First_Semester, Status = Status.On},
                    new Job {Date = DateTime.Now, Region = Regions.R3 , Name = "Jb2", Semester = Semesters.Second_Semester, Status = Status.On},
                    new Job {Date = DateTime.Now, Region = Regions.R2 , Name = "Jb3", Semester = Semesters.First_Semester, Status = Status.On},
                    new Job {Date = DateTime.Now, Region = Regions.R4 , Name = "Jb4", Semester = Semesters.Second_Semester, Status = Status.On},
                    new Job {Date = DateTime.Now, Region = Regions.R5 , Name = "Jb5", Semester = Semesters.First_Semester, Status = Status.On},
                    new Job {Date = DateTime.Now, Region = Regions.R3 , Name = "Jb6", Semester = Semesters.Second_Semester, Status = Status.On},
                    new Job {Date = DateTime.Now, Region = Regions.R2 , Name = "Jb7", Semester = Semesters.First_Semester, Status = Status.On},
                    new Job {Date = DateTime.Now, Region = Regions.R1 , Name = "Jb8", Semester = Semesters.Second_Semester, Status = Status.On},
                    new Job {Date = DateTime.Now, Region = Regions.R5 , Name = "Jb9", Semester = Semesters.First_Semester, Status = Status.On},
                    new Job {Date = DateTime.Now, Region = Regions.R4 , Name = "Jb10", Semester = Semesters.Second_Semester, Status = Status.On}
                };
            }
        }

        static void Main(string[] args)
        {
            var data = new Dictionary<Regions, List<Job>>();

            Run(new List<int> { 1, 2, 3 }, new List<int> { });
            Run(new List<int> { 1, 2, 3, 4, 5 }, new List<int> { });
            Run(new List<int> {}, new List<int> {1});
            Run(new List<int> {}, new List<int> {2});
            Run(new List<int> { }, new List<int> { 1,2 });

            Run(new List<int> {1,2,3,4,5}, new List<int> { 1});
            Run(new List<int> {1,2,3,4,5}, new List<int> { 2 });


        }

        private static void Run(List<int> regionsValues, List<int> semestersValues)
        {
           

            if ((regionsValues != null && regionsValues.Count > 0) && (semestersValues != null && semestersValues.Count > 0))
            {

                var data = GetByRegions(regionsValues);
                var filteredBySemester = data.Where(jb => semestersValues.Contains((int)jb.Semester));
                var result = filteredBySemester.GroupBy(gr => (int)gr.Region).ToDictionary(jb => jb.Key, jb => jb.ToList());

            }
            else if ((regionsValues != null && regionsValues.Count > 0) && (semestersValues == null || semestersValues.Count == 0))
            {

                var data = GetByRegions(regionsValues);
                var result =  data.GroupBy(gr => (int)gr.Region).ToDictionary(jb => jb.Key, jb => jb.ToList());

            }
            else
            {

                var data = GetBySemesters(semestersValues);
                var result = data.GroupBy(gr => (int)gr.Semester).ToDictionary(jb => jb.Key, jb => jb.ToList());

            }
        }

        private static List<Job> GetByRegions(List<int> regionsValues)
        {
            var data = new List<Job>();
            var notInCache = new List<int>();
            var cacheNamePrefix = "RegionJob_";
            var cacheTime = 15;

            CheckJobExpoFromCache(regionsValues, cacheNamePrefix, out notInCache, out data);

            if (notInCache != null && notInCache.Count > 0)
            {
                var notInCachedData = jobList.Where(jb => notInCache.Contains((int)jb.Region));
                var groupData = notInCachedData.GroupBy(rg => (int)rg.Region);

                foreach (var dt in groupData)
                {
                    var cacheName = cacheNamePrefix + dt.Key;
                    MyCache.SetItemCache(cacheName, dt.ToList(), cacheTime);
                }

                data.AddRange(notInCachedData);
            }

            return data;
        }

        private static List<Job> GetBySemesters(List<int> semestersValues)
        {
            var data = new List<Job>();
            var notInCache = new List<int>();
            var cacheNamePrefix = "SemesterJob_";
            var cacheTime = 15;

            CheckJobExpoFromCache(semestersValues, cacheNamePrefix, out notInCache, out data);

            if (notInCache != null && notInCache.Count > 0)
            {
                var notInCachedData = jobList.Where(jb => notInCache.Contains((int)jb.Semester));
                var groupData = notInCachedData.GroupBy(rg => (int)rg.Semester);

                foreach (var dt in groupData)
                {
                    var cacheName = cacheNamePrefix + dt.Key;
                    MyCache.SetItemCache(cacheName, dt.ToList(), cacheTime);
                }

                data.AddRange(notInCachedData);
            }

            return data;
        }

        public static void CheckJobExpoFromCache(List<int> filterValues, string cacheNamePrefix,out List<int> notInCache, out List<Job> data) 
        {
            notInCache = new List<int>();
            data = new List<Job>();

            foreach (var filterVal in filterValues)
            {
                var cacheName = cacheNamePrefix + filterVal;
                var cachedItem = MyCache.GetItemCached(cacheName);

                if (cachedItem == null)
                {
                    notInCache.Add(filterVal);
                }
                else
                {
                    data.AddRange((IEnumerable<Job>)cachedItem);
                }
            }
        }
    }
}
