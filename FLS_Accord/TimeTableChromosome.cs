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


            private LecturerInput RandomLecturer(List<LecturerInput> lecturerRegisterSubjectList)
            {
                Random random = new Random();

                bool canChoose = true;

                LecturerInput randomLecturer = new LecturerInput();
                List<TimeSlotInput> randomTimeSlotList;

                int count = 0;

                if (lecturerRegisterSubjectList.Count != 0)
                {
                    do
                    {
                        randomTimeSlotList = RandomTimeSlots(random, _dataContext.TimeSlots);

                        //check TimeSlot between Lecturer and Course
                        foreach (var lecturer in lecturerRegisterSubjectList)
                        {
                            randomLecturer = lecturer;

                            foreach (var timeSlot in randomLecturer.OccupiedTimeSlot)
                            {
                                foreach (var currentTimeSlot in randomTimeSlotList)
                                {
                                    if (timeSlot.Id == currentTimeSlot.Id)
                                    {
                                        canChoose = false;
                                    }

                                }
                            }
                        }


                    } while (canChoose && count <= 100);

                    return randomLecturer;

                }

                return null;
            }

            private List<TimeSlotInput> RandomTimeSlots(Random random, List<TimeSlotInput> timeSlotInputList)
            {
                List<TimeSlotInput> randomTimeSlotList = new List<TimeSlotInput>();

                int index1;
                int index2;
                int index3;

                do
                {
                    index1 = random.Next(1, 30);
                    index2 = random.Next(1, 30);
                    index3 = random.Next(1, 30);
                } while (index1 == index2 && index2 == index3 && index3 == index1);

                TimeSlotInput timeSlotInput1 = timeSlotInputList[index1];
                TimeSlotInput timeSlotInput2 = timeSlotInputList[index2];
                TimeSlotInput timeSlotInput3 = timeSlotInputList[index3];

                randomTimeSlotList.Add(timeSlotInput1);
                randomTimeSlotList.Add(timeSlotInput2);
                randomTimeSlotList.Add(timeSlotInput3);

                return randomTimeSlotList;
            }

            public override void Generate()
            {
                Random random = new Random();

                var courseList = _dataContext.Courses;

                bool canChoose = true;

                int count = 0;

                var lecturerList = _dataContext.SubjectRegisterInputs;

                var timeSlotList = _dataContext.TimeSlots;

                List<TimeSlotInput> timeSlotInputList = _dataContext.TimeSlots;

                LecturerInput randomLecturer = new LecturerInput();

                List<TimeSlotInput> randomTimeSlotList = new List<TimeSlotInput>();

                List<LecturerCourseSlot> listLecturerCourseSlot = new List<LecturerCourseSlot>();

                foreach (var course in courseList)
                {
                    var subjectInCourse = course.Subject.SubjectCode;

                    var groupInCourse = course.StudentGroup;

                    List<LecturerInput> lecturerRegisterSubjectList = new List<LecturerInput>();

                    foreach (var lecturer in lecturerList)
                    {
                        if (lecturer.Subject.SubjectCode.Equals(subjectInCourse))
                        {
                            lecturerRegisterSubjectList.Add(lecturer.Lecturer);
                        }
                    }


                    if (lecturerRegisterSubjectList.Count != 0)
                    {
                        randomLecturer = lecturerRegisterSubjectList.ElementAt(random.Next(0, lecturerRegisterSubjectList.Count - 1));
                        var teachRate = random.Next(randomLecturer.MinCourse, randomLecturer.MaxCourse);
                        do
                        {
                            randomTimeSlotList = RandomTimeSlots(random, timeSlotList);

                            //check TimeSlot between Lecturer and Course
                           
                            if (randomLecturer.OccupiedCourse.Count < teachRate)
                            {

                                if (randomLecturer.OccupiedTimeSlot.Count != 0)
                                {
                                    var separate = 0;
                                    foreach (var timeSlot in randomLecturer.OccupiedTimeSlot)
                                    {
                                        foreach (var currentTimeSlot in randomTimeSlotList)
                                        {
                                            if (timeSlot.Id == currentTimeSlot.Id)
                                            {
                                                canChoose = false;
                                                count++;
                                            }
                                            else
                                            {
                                                separate++;
                                            }
                                        }
                                    }
                                    if (separate == randomLecturer.OccupiedTimeSlot.Count * 3)
                                    {
                                        foreach (var currentTimeSlot in randomTimeSlotList)
                                        {
                                            randomLecturer.OccupiedTimeSlot.Add(currentTimeSlot);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var currentTimeSlot in randomTimeSlotList)
                                    {
                                        randomLecturer.OccupiedTimeSlot.Add(currentTimeSlot);
                                    }
                                }

                                if (groupInCourse.OccupiedTimeSlot.Count != 0)
                                {
                                    var separate = 0;
                                    foreach (var timeSlot in groupInCourse.OccupiedTimeSlot)
                                    {
                                        foreach (var currentTimeSlot in randomTimeSlotList)
                                        {
                                            if (timeSlot.Id == currentTimeSlot.Id)
                                            {
                                                canChoose = false;
                                                count++;
                                            }
                                            else
                                            {
                                                separate++;
                                            }
                                        }
                                    }
                                    if (separate == groupInCourse.OccupiedTimeSlot.Count * 3)
                                    {
                                        foreach (var currentTimeSlot in randomTimeSlotList)
                                        {
                                            groupInCourse.OccupiedTimeSlot.Add(currentTimeSlot);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var currentTimeSlot in randomTimeSlotList)
                                    {
                                        groupInCourse.OccupiedTimeSlot.Add(currentTimeSlot);
                                    }
                                }

                            }

                        } while (canChoose && count <= 100);
                        randomLecturer.OccupiedCourse.Add(course);

                        LecturerCourseSlot lecturerCourseSlot = new LecturerCourseSlot();
                        lecturerCourseSlot.Lecturer = randomLecturer;
                        lecturerCourseSlot.Course = course;
                        lecturerCourseSlot.TimeSlots = randomTimeSlotList;

                        listLecturerCourseSlot.Add(lecturerCourseSlot);
                    }

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

                } while (lecturer.Equals(indexLecturerCourseSlot.Lecturer));

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


                    score += fit.calMinMaxCouseOfLecturer(_dataContext.Lecturers, values);
                    score += fit.checkLecturerTeachableSubject(values, _dataContext);
                    score += fit.checkNotRegister(values, _dataContext);

                    return Math.Pow(Math.Abs(score), -1);
                }
            }
        }
    }


}
