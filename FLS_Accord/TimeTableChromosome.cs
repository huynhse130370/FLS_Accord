using Accord.Genetic;
using FLS_Accord.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLS_Accord
{

    public class TimeTableGenerationHandler
    {
        

        private static readonly int MAXCHROMOSOME = 50;

        public TimeTableGenerationHandler()
        {
        }

        public static Population CreatePopulations(GenerateTimetableInput dataContext)
        {
            Console.WriteLine("Create population ...");

            Population population = new Population(MAXCHROMOSOME, new TimeTableChromosome(dataContext),
                           new TimeTableChromosome.FitnessFunction(), new EliteSelection());
            
            return population;
        }

        public static Population FindBestPopulation(Population population)
        {
            Console.WriteLine("Begin creating ...");
            int i = 0;
            while (true)
            {

                population.RunEpoch();
                i++;
                if (population.FitnessMax >= 0.99 || i >= MAXCHROMOSOME)
                {
                    break;
                }
            }
            return population;
        }
        public static IChromosome FindBestPopulation(Population population, IChromosome selectedChromosome)
        {
            Console.WriteLine("Begin creating ...");

            int i = 0;
            while (true)
            {

                population.RunEpoch();
                i++;
                if (population.FitnessMax >= 0.99 || i >= MAXCHROMOSOME)
                {
                    break;
                }
            }

            IChromosome bestChromosome = population.BestChromosome;
            bestChromosome.Evaluate(new TimeTableChromosome.FitnessFunction());
            
            Console.WriteLine(selectedChromosome.Fitness + "  vs  " + bestChromosome.Fitness + "= " + (selectedChromosome.Fitness.CompareTo(bestChromosome.Fitness) > 0));


            if (selectedChromosome.Fitness.CompareTo(bestChromosome.Fitness) > 0)
            {
                Console.WriteLine("Select:" + selectedChromosome.Fitness);
                return selectedChromosome;
            }
            Console.WriteLine("Select:" + bestChromosome.Fitness);

            return bestChromosome;
        }


        public class SubjectValue
        {
            public String Name { get; set; }
            public int LecturerId { get; set; }

            public List<CourseInput> Courses { get; set; }

            public SubjectValue(string name, int lecturerId, List<CourseInput> courses)
            {
                Name = name;
                LecturerId = lecturerId;
                Courses = courses;
            }
        }

        class TimeTableChromosome : ChromosomeBase
        {
            private readonly GenerateTimetableInput _dataContext;
            static Random Random = new Random();

            //static TimeSpan RandomStartTime()
            //{
            //    return TimeSpan.FromMilliseconds(Random.Next((int)TimeSpan.FromHours(7).TotalMilliseconds,
            //        (int)TimeSpan.FromHours(17).TotalMilliseconds));
            //}

            public List<LecturerCourse> Value;

            public TimeTableChromosome(GenerateTimetableInput dataContext)
            {
                _dataContext = dataContext;
                Generate();
            }
            public TimeTableChromosome(List<LecturerCourse> subjectChromosomes, GenerateTimetableInput dataContext)
            {
                _dataContext = dataContext;
                Value = subjectChromosomes.ToList();
            }


            private List<LecturerCourse> CreateLecturerWithCourses(List<LecturerInput> lecturerList, List<CourseInput> courseList)
            {

                Random random = new Random();
                LecturerInput randomLecturer;
                List<LecturerCourse> listLecturerCourse = new List<LecturerCourse>();

                foreach(var course in courseList)
                {
                    randomLecturer = lecturerList[random.Next(0, lecturerList.Count)];


                    LecturerCourse lecturerCourse = new LecturerCourse();
                    lecturerCourse.Lecturer = randomLecturer;
                    lecturerCourse.Course = course;

                    listLecturerCourse.Add(lecturerCourse);
                }
                
                return listLecturerCourse;
            }


            private LecturerInput RandomLecturers(List<LecturerInput> Lecturers, SubjectInput subjectValue)
            {
                var count = 0;
                LecturerInput teacherRandom;
                Random Random = new Random();
                bool canChoose;
                var registers = _dataContext.SubjectRegisterInputs;
                if (Lecturers.Count != 0)
                {
                    do
                    {
                        canChoose = true;
                        teacherRandom = Lecturers[Random.Next(0, Lecturers.Count)];

                        //Condition for create Lecturer
                        foreach (var course in teacherRandom.OccupiedCourse)
                        {
                            foreach (var currentCourse in subjectValue.Courses)
                            {
                                foreach (var registerCourse in registers)
                                {
                                    if (teacherRandom.Id == registerCourse.LecturerId && currentCourse.SubjectId != registerCourse.SubjectId)
                                    {
                                        canChoose = false;
                                        count++;
                                    }
                                }

                            }

                        }

                    } while (!canChoose && count <= 100);
                    return teacherRandom;
                }
                return null;
            }

            public override void Generate()
            {
                Random random = new Random();

                var courseList = _dataContext.Courses;

                var lecturerList = _dataContext.Lecturers;


                LecturerInput randomLecturer;
                List<LecturerCourse> listLecturerCourse = new List<LecturerCourse>();

                foreach (var course in courseList)
                {
                    randomLecturer = lecturerList[random.Next(0, lecturerList.Count)];
                    
                    LecturerCourse lecturerCourse = new LecturerCourse();
                    lecturerCourse.Lecturer = randomLecturer;
                    lecturerCourse.Course = course;
                    listLecturerCourse.Add(lecturerCourse);
                }
                

                Value = listLecturerCourse;

            }

            public override IChromosome CreateNew()
            {
                var timeTableChromosome = new TimeTableChromosome(_dataContext);
                timeTableChromosome.Generate();
                return timeTableChromosome;
            }

            public override IChromosome Clone()
            {
                return new TimeTableChromosome(Value, _dataContext);
            }

            public override void Mutate()
            {
                Random Random = new Random();
                
                int index = Random.Next(0, Value.Count - 1);

                LecturerCourse indexLecturerCourse = Value[index];

                var listLecturer = _dataContext.Lecturers;
                LecturerInput lecturer;
                do
                {
                    lecturer = listLecturer[Random.Next(0, listLecturer.Count - 1)];
                } while (lecturer.Equals(indexLecturerCourse.Lecturer));

                Value[index].Lecturer = lecturer;
            }

            public override void Crossover(IChromosome pair)
            {
                var randomVal = Random.Next(0, Value.Count - 2);
                var otherChromsome = pair as TimeTableChromosome;
                for (int index = randomVal; index < otherChromsome.Value.Count; index++)
                {
                    Value[index] = otherChromsome.Value[index];
                }
            }

            public class FitnessFunction : IFitnessFunction
            {

                private readonly GenerateTimetableInput _dataContext = new GenerateTimetableInput();


                public double Evaluate(IChromosome chromosome)
                {
                    double score = 1;
                    FitNessCalculation fit = new FitNessCalculation();

                var values = (chromosome as TimeTableChromosome).Value;
                    //var timetableType = (chromosome as TimeTableChromosome)._dataContext.TimetableType;
                    score += fit.calMinMaxCouseOfLecturer(_dataContext.Lecturers, values);
                    score += fit.checkLecturerTeachableSubject(values, _dataContext);
                    score += fit.checkNotRegister(values, _dataContext);
                    //foreach (var value in values)
                    //{
                    //    //var overLaps = CheckOverLapConstaint(value, values);
                    //    //score += CheckOverLapConstaint(value, values);

                    //    s    
                    //}
                    //score += CheckTotalHoursConstaint(values, timetableType);


                    return Math.Pow(Math.Abs(score), -1);
                }

                //private bool isBetweenMinMaxCourse(LecturerInput lecturer, List<CourseInput> Courses)
                //{
                //    var count = 0;
                //    foreach (var course in Courses)
                //    {
                //        if (lecturer.Id == course.Lecturer.Id)
                //        {
                //            count++;
                //        }
                //    }
                //    if (count > lecturer.MinCourse && count < lecturer.MaxCourse)
                //    {
                //        return true;
                //    }
                //    return false;
                //}

                //private bool isFitSubject(LecturerInput lecturer, List<LecturerInput> lecturers)
                //{
                //    return false;
                //}
            }
        }
    }

    
}
