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

            public List<SubjectValue> Value;

            public TimeTableChromosome(GenerateTimetableInput dataContext)
            {
                _dataContext = dataContext;
                Generate();
            }
            public TimeTableChromosome(List<SubjectValue> subjectChromosomes, GenerateTimetableInput dataContext)
            {
                _dataContext = dataContext;
                Value = subjectChromosomes.ToList();
            }


            private LecturerInput RandomLecturers(List<LecturerInput> Lecturers, SubjectInput subjectValue)
            {
                var count = 0;
                LecturerInput teacherRandom;
                Random Random = new Random();
                bool canChoose;
                var registers = _dataContext.SubjectRegisters;
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
                IEnumerable<SubjectValue> generateRandomLecturer()
                {
                    var subjects = _dataContext.Subjects;


                    foreach (var subject in subjects)
                    {
                        LecturerInput lecturerRandom = RandomLecturers(_dataContext.Lecturers, subject);

                        List<CourseInput> newCourse = new List<CourseInput>();

                        foreach (var course in subject.Courses)
                        {
                            course.Lecturer = lecturerRandom;
                            newCourse.Add(course);
                        }
                        if (newCourse.ElementAt(0).Lecturer != null)
                        {
                            yield return new SubjectValue(newCourse.ElementAt(0).Name, newCourse.ElementAt(0).Lecturer.Id, newCourse);

                        }
                        else
                        {
                            yield return new SubjectValue(newCourse.ElementAt(0).Name, 0, newCourse);
                        }

                    }
                }

                Value = generateRandomLecturer().ToList();

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
                LecturerInput lecturerFirst;
                LecturerInput lecturerSecond;
                SubjectValue subjectChoosedFirst;
                SubjectValue subjectChoosedSecond;
                int indexFirst;
                int indexSecond;
                bool canChoose;
                var count = 0;
                var registers = _dataContext.SubjectRegisters;

                do
                {
                    canChoose = true;
                    var randomCount = 0;
                    do
                    {
                        do
                        {
                            indexFirst = Random.Next(0, Value.Count - 1);
                            indexSecond = Random.Next(0, Value.Count - 1);
                        }
                        while (indexSecond == indexFirst);

                        subjectChoosedFirst = Value.ElementAt(indexFirst);
                        subjectChoosedSecond = Value.ElementAt(indexSecond);

                        lecturerFirst = subjectChoosedFirst.Courses.ElementAt(0).Lecturer;
                        lecturerSecond = subjectChoosedSecond.Courses.ElementAt(0).Lecturer;
                        if (lecturerFirst == null || lecturerSecond == null)
                        {
                            break;
                        }
                        randomCount++;
                    } while (lecturerFirst.Equals(lecturerSecond) && randomCount <= Value.Count);
                    if (lecturerFirst != null && lecturerSecond != null)
                    {
                        foreach (var course in lecturerFirst.OccupiedCourse)
                        {
                            foreach (var currentCourse in subjectChoosedFirst.Courses)
                            {
                                foreach (var registerCourse in registers)
                                {
                                    if (lecturerFirst.Id == registerCourse.LecturerId && currentCourse.SubjectId != registerCourse.SubjectId)
                                    {
                                        canChoose = false;
                                        count++;
                                    }
                                }

                            }

                        }
                        foreach (var course in lecturerFirst.OccupiedCourse)
                        {
                            foreach (var currentCourse in subjectChoosedSecond.Courses)
                            {
                                foreach (var registerCourse in registers)
                                {
                                    if (lecturerFirst.Id == registerCourse.LecturerId && currentCourse.SubjectId != registerCourse.SubjectId)
                                    {
                                        canChoose = false;
                                        count++;
                                    }
                                }

                            }

                        }
                    }
                } while (!canChoose && count <= 100);

                foreach (var session in subjectChoosedFirst.Courses)
                {
                    session.Lecturer = lecturerSecond;
                }

                foreach (var session in subjectChoosedSecond.Courses)
                {
                    session.Lecturer = lecturerFirst;
                }

                Value[indexFirst] = subjectChoosedFirst;
                Value[indexSecond] = subjectChoosedSecond;
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
                public double Evaluate(IChromosome chromosome)
                {
                    double score = 1;
                    //var values = (chromosome as TimeTableChromosome).Value;
                    //var timetableType = (chromosome as TimeTableChromosome)._dataContext.TimetableType;
                    //foreach (var value in values)
                    //{
                    //    //var overLaps = CheckOverLapConstaint(value, values);
                    //    //score += CheckOverLapConstaint(value, values);
                    //}
                    //score += CheckTotalHoursConstaint(values, timetableType);


                    return Math.Pow(Math.Abs(score), -1);
                }

                private bool isBetweenMinMaxCourse(LecturerInput lecturer)
                {
                    List<CourseInput> Courses = new List<CourseInput>();
                    var count = 0;
                    foreach (var course in Courses)
                    {
                        if (lecturer.Id == course.Lecturer.Id)
                        {
                            count++;
                        }
                    }
                    if (count > lecturer.MinCourse && count < lecturer.MaxCourse)
                    {
                        return true;
                    }
                    return false;
                }

                private bool isFitSubject(LecturerInput lecturer, List<LecturerInput> lecturers)
                {

                    return false;
                }
            }
        }
    }

    
}
