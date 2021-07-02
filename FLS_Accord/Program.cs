using Accord.Genetic;
using FLS_Accord.Models;
using System;
using System.Linq;

namespace FLS_Accord
{
    class Program
    {
        static void Main(string[] args)
        {
            

            
            //using (var dataContext = new FLSContext())
            //{
            //    Console.WriteLine("Create population ...");
            //    Population population = new Population(50, new TimeTableChromosome(dataContext),
            //        new TimeTableChromosome.FitnessFunction(), new EliteSelection());

            //    int i = 0;

            //    Console.WriteLine("Begin creating ...");
            //    while (true)
            //    {
            //        population.RunEpoch();
            //        i++;
            //        if (population.FitnessMax >= 0.99 || i >= 1000)
            //        {
            //            break;
            //        }
            //    }

            //    var timetable = (population.BestChromosome as TimeTableChromosome).Value.Select(chromosome =>
            //        new Schedule()
            //        {
            //            LecturerCode = chromosome.LecturerId,
            //            SubjectCode = chromosome.SubjectId
            //        }
            //    ).ToList();
            //    foreach(var x in timetable)
            //    {
            //        Console.WriteLine(x.LecturerCode);
            //        Console.WriteLine(x.SubjectCode);
            //    }
                
            //    //dataContext.TimeSlots.AddRange(timetable);
            //    //dataContext.SaveChanges();
            //}
        }
        private readonly FLSContext _context;

        public Program(FLSContext context)
        {
            _context = context;
        }

        public string AddDataToSchedule()
        {
            var courses = _context.Course.ToList();
            var subjects = _context.Subject.ToList();
            var lecturers = _context.Lecturer.Include.ToList();
            var subjectRegisters = _context.SubjectRegister.ToList();
            var teachableSubject = _context.TeachableSubject.ToList();

            foreach(var lecturer in lecturers)
            {
                LecturerInput lecturerInput = new LecturerInput
                {
                    Id = lecturer.Id,
                    Department = lecturer.Department;
                }
            }


            GenerateTimetableInput generate = new GenerateTimetableInput()
        }
    }
}
