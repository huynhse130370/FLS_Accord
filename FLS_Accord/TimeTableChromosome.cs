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


            private List<TimeSlotInput> RandomTimeSlots(Random random, List<TimeSlotInput> timeSlotInputList)
            {
                List<TimeSlotInput> randomTimeSlotList = new List<TimeSlotInput>();

                int index1;
                int index2;
                int index3;

                do
                {
                    index1 = random.Next(0, 29);
                    index2 = random.Next(0, 29);
                    index3 = random.Next(0, 29);

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

                int count = 0;

                bool isExistGroup;

                bool isExistLecturer;

                var lecturerList = _dataContext.SubjectRegisterInputs;

                var timeSlotList = _dataContext.TimeSlots;

                List<TimeSlotInput> timeSlotInputList = _dataContext.TimeSlots;

                LecturerInput randomLecturer = new LecturerInput();

                List<TimeSlotInput> randomTimeSlotList = new List<TimeSlotInput>();

                List<LecturerCourseSlot> listLecturerCourseSlot = new List<LecturerCourseSlot>();

                foreach (var course in courseList)
                {
                    int conflictLecturer = 3;

                    int conflictGroup = 3;

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

                        if (randomLecturer.OccupiedCourse.Count < randomLecturer.MaxCourse)
                        {
                            do
                            {
                                isExistGroup = false;

                                isExistLecturer = false;

                                randomTimeSlotList = RandomTimeSlots(random, timeSlotList);

                                //check TimeSlot between Lecturer and Course
                                if (randomLecturer.OccupiedTimeSlot.Count != 0)
                                {
                                    conflictLecturer = 0;
                                    foreach (var timeSlot in randomLecturer.OccupiedTimeSlot)
                                    {
                                        foreach (var currentTimeSlot in randomTimeSlotList)
                                        {
                                            if (timeSlot.Id == currentTimeSlot.Id)
                                            {
                                                conflictLecturer++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    conflictLecturer = 0;
                                }

                                if (groupInCourse.OccupiedTimeSlot.Count != 0)
                                {
                                    conflictGroup = 0;
                                    foreach (var timeSlot in groupInCourse.OccupiedTimeSlot)
                                    {
                                        foreach (var currentTimeSlot in randomTimeSlotList)
                                        {
                                            if (timeSlot.Id == currentTimeSlot.Id)
                                            {
                                                conflictGroup++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    conflictGroup = 0;
                                }



                                if (conflictLecturer == 0)
                                {
                                    if (conflictGroup == 0)
                                    {
                                        foreach (var currentTimeSlot in randomTimeSlotList)
                                        {
                                            randomLecturer.OccupiedTimeSlot.Add(currentTimeSlot);
                                            groupInCourse.OccupiedTimeSlot.Add(currentTimeSlot);
                                        }
                                        isExistLecturer = true;
                                        isExistGroup = true;
                                    }
                                }



                            } while (isExistLecturer == false || isExistGroup == false);

                            randomLecturer.OccupiedCourse.Add(course);

                            LecturerCourseSlot lecturerCourseSlot = new LecturerCourseSlot();
                            lecturerCourseSlot.Lecturer = randomLecturer;
                            lecturerCourseSlot.Course = course;
                            lecturerCourseSlot.TimeSlots = randomTimeSlotList;
                            lecturerCourseSlot.StudentGroupInput = groupInCourse;
                            listLecturerCourseSlot.Add(lecturerCourseSlot);
                        }
                    }
                    count++;
                }

                if (count == 625)
                {
                    Console.WriteLine("Ket thuc tai day !!!");
                }

                Value = listLecturerCourseSlot;
                Console.WriteLine("Ket thuc tai day !!!");

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