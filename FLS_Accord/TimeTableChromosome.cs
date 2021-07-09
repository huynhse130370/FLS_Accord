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


            //private List<LecturerCourseSlot> CreateLecturerWithCourses(List<LecturerInput> lecturerList, List<CourseInput> courseList)
            //{

            //    Random random = new Random();
            //    LecturerInput randomLecturer;
            //    List<LecturerCourseSlot> listLecturerCourseSlot = new List<LecturerCourseSlot>();

            //    foreach (var course in courseList)
            //    {
            //        randomLecturer = lecturerList[random.Next(0, lecturerList.Count)];


            //        LecturerCourseSlot lecturerCourse = new LecturerCourseSlot();
            //        lecturerCourse.Lecturer = randomLecturer;
            //        lecturerCourse.Course = course;

            //        listLecturerCourseSlot.Add(lecturerCourse);
            //    }

            //    return listLecturerCourseSlot;
            //}


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

                var lecturerList = _dataContext.SubjectRegisterInputs;


                LecturerInput randomLecturer;
                List<LecturerCourseSlot> listLecturerCourseSlot = new List<LecturerCourseSlot>();

                foreach (var course in courseList)
                {
                    //randomLecturer = lecturerList[random.Next(0, lecturerList.Count - 1)];

                    var subjectInCourse = course.Subject.Name;

                    List<LecturerInput> lecturerRegisterSubjectList = new List<LecturerInput>();

                    foreach (var lecturer in lecturerList)
                    {
                        if (lecturer.Subject.Name.Equals(subjectInCourse))
                        {
                            lecturerRegisterSubjectList.Add(lecturer.Lecturer);
                        }
                    }

                    List<LecturerInput> randomLecturerList = new List<LecturerInput>();

                    int randomLenght = random.Next(1, lecturerRegisterSubjectList.Count);

                    for (int i = 0; i <= randomLenght; i++)
                    {
                        //int randomIndex = random.Next(0, randomLenght - 1);
                        randomLecturerList.Add(lecturerRegisterSubjectList[i]);
                    }

                    List<TimeSlotInput> timeSlotInputList = _dataContext.TimeSlots;

                    int index1;
                    int index2;
                    int index3;

                    do
                    {
                        index1 = random.Next(1, 30);
                        index2 = random.Next(1, 30);
                        index3 = random.Next(1, 30);
                    } while (index1 != index2 && index2 != index3 && index3 != index1);

                    List<TimeSlotInput> randomTimeSlotList = new List<TimeSlotInput>();

                    TimeSlotInput timeSlotInput1 = timeSlotInputList[index1];
                    TimeSlotInput timeSlotInput2 = timeSlotInputList[index2];
                    TimeSlotInput timeSlotInput3 = timeSlotInputList[index3];

                    randomTimeSlotList.Add(timeSlotInput1);
                    randomTimeSlotList.Add(timeSlotInput2);
                    randomTimeSlotList.Add(timeSlotInput3);

                    LecturerCourseSlot lecturerCourseSlot = new LecturerCourseSlot();
                    lecturerCourseSlot.Lecturers = randomLecturerList;
                    lecturerCourseSlot.Course = course;
                    lecturerCourseSlot.TimeSlots = randomTimeSlotList;

                    listLecturerCourseSlot.Add(lecturerCourseSlot);
                }


                Value = listLecturerCourseSlot;

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

                LecturerCourseSlot indexLecturerCourseSlot = Value[index];

                var listLecturer = _dataContext.Lecturers;
                LecturerInput lecturer;
                do
                {
                    lecturer = listLecturer[Random.Next(0, listLecturer.Count - 1)];
                    
                } while (lecturer.Equals(indexLecturerCourseSlot.Lecturers.));

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


                private int globalCount = 0;

                public double Evaluate(IChromosome chromosome)
                {
                    double score = 1;
                    FitNessCalculation fit = new FitNessCalculation();

                    var values = (chromosome as TimeTableChromosome).Value;
                    //var timetableType = (chromosome as TimeTableChromosome)._dataContext.TimetableType;


                    score += fit.calMinMaxCouseOfLecturer(_dataContext.Lecturers, values);
                    score += fit.checkLecturerTeachableSubject(values, _dataContext);
                    score += fit.checkNotRegister(values, _dataContext);

                    return Math.Pow(Math.Abs(score), -1);
                }
            }
        }
    }


}
