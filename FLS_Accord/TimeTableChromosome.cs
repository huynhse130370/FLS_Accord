using Accord.Genetic;
using System;
using System.Collections.Generic;
using System.Linq;

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


        public class TimeTableChromosome : ChromosomeBase
        {
            private readonly GenerateTimetableInput _dataContext;
            static Random Random = new Random();

            public List<LecturerCourseSlot> Value;

            public TimeTableChromosome(GenerateTimetableInput dataContext)
            {
                _dataContext = dataContext;
                Generate();
            }
            public TimeTableChromosome(List<LecturerCourseSlot> subjectChromosomes, GenerateTimetableInput dataContext)
            {
                _dataContext = dataContext;
                Value = subjectChromosomes.ToList();
            }


            private TimeSlotInput RandomTimeSlot(Random random, List<TimeSlotInput> timeSlotInputList)
            {
                int index = random.Next(0, timeSlotInputList.Count - 1);

                return timeSlotInputList[index];
            }

            private LecturerInput RandomLecturer(Random random, List<LecturerInput> lecturerInputList)
            {
                int index = random.Next(0, lecturerInputList.Count - 1);

                return lecturerInputList[index];
            }

            static TimeSpan RandomStartTime()
            {
                return TimeSpan.FromHours(Random.Next((int)TimeSpan.FromHours(7).TotalHours,
                    (int)TimeSpan.FromHours(17).TotalHours));
            }

            public override void Generate()
            {
                IEnumerable<LecturerCourseSlot> generateRandomSlots()
                {
                    var courses = _dataContext.Courses;

                    var subjects = _dataContext.Subjects;

                    var lecturers = _dataContext.Lecturers;

                    var timeSlots = _dataContext.TimeSlots;

                    foreach (var course in courses)
                    {
                        string subjectCode = course.Subject.SubjectCode;

                        int departmentId = 1;

                        foreach(var subject in subjects)
                        {
                            if (subject.SubjectCode.Equals(subjectCode)){
                                departmentId = subject.DepartmentId;
                            }
                        }

                        var lecturerList = new List<LecturerInput>();

                        foreach(var lec in lecturers)
                        {
                            if(lec.DepartmentId == departmentId)
                            {
                                lecturerList.Add(lec);
                            }
                        }

                        yield return new LecturerCourseSlot()
                        {
                            CourseName = course.Name,
                            SubjectCode = course.Subject.SubjectCode,
                            LecturerCode = RandomLecturer(Random, lecturerList).LecturerCode,
                            TimeSlot = RandomTimeSlot(Random, timeSlots)
                        };
                    }
                }

                Value = generateRandomSlots().ToList();


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

                int index = Random.Next(0, Value.Count - 1);

                var timeSlotList = _dataContext.TimeSlots;

                LecturerCourseSlot indexLecturerCourseSlot = Value[index];

                indexLecturerCourseSlot.TimeSlot = RandomTimeSlot(Random, timeSlotList);

                Value[index] = indexLecturerCourseSlot;
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

                    var GetoverLaps = new Func<LecturerCourseSlot, List<LecturerCourseSlot>>(current => values
                    .Except(new[] { current })
                    .Where(slot => slot.TimeSlot.Id == current.TimeSlot.Id)
                    .ToList());

                    foreach (var value in values)
                    {
                        var overLaps = GetoverLaps(value);
                        score -= overLaps.GroupBy(slot => slot.LecturerCode).Sum(x => x.Count() - 1);
                        score -= overLaps.GroupBy(slot => slot.SubjectCode).Sum(x => x.Count() - 1);
                    }

                    score -= values.GroupBy(v => v.TimeSlot.DayOfWeek).Count() * 0.5;

                    return Math.Pow(Math.Abs(score), -1);
                }
            }
        }
    }


}