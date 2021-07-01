using Accord.Genetic;
using FLS_Accord.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLS_Accord
{

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

        static TimeSpan RandomStartTime()
        {
            return TimeSpan.FromMilliseconds(Random.Next((int)TimeSpan.FromHours(7).TotalMilliseconds,
                (int)TimeSpan.FromHours(17).TotalMilliseconds));
        }

        public List<TimeSlotChromosome> Value;

        public TimeTableChromosome(GenerateTimetableInput dataContext)
        {
            _dataContext = dataContext;
            Generate();
        }
        public TimeTableChromosome(List<TimeSlotChromosome> slots, GenerateTimetableInput dataContext)
        {
            _dataContext = dataContext;
            Value = slots.ToList();
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
                            foreach(var registerCourse in registers)
                            {
                                if(teacherRandom.Id == registerCourse.LecturerId && currentCourse.SubjectId != registerCourse.SubjectId)
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
            IEnumerable<SubjectInput> generateRandomLecturer()
            {
                var subjects = _dataContext.Subjects;

                //List<Lecturer> lecturers = _dataContext.Lecturer.ToList();
                //List<Subject> subjects = _dataContext.Subject.ToList();

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
                        yield return new SubjectValue(newCourse.ElementAt(0).Name.ToString(), newCourse.ElementAt(0).Lecturer.Id, newCourse);

                    }
                    else
                    {
                        yield return new ClassValue(newCourse.ElementAt(0).Class, null, newSession);
                    }


                    //yield return new TimeSlotChromosome()
                    //{
                    //    //code here
                    //};
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
            var index = Random.Next(0, Value.Count - 1);
            var timeSlotChromosome = Value.ElementAt(index);
            timeSlotChromosome.StartAt = RandomStartTime();
            timeSlotChromosome.Day = Random.Next(1, 5);
            Value[index] = timeSlotChromosome;
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
                var values = (chromosome as TimeTableChromosome).Value;
                var GetoverLaps = new Func<TimeSlotChromosome, List<TimeSlotChromosome>>(current => values
                    .Except(new[] { current })
                    .Where(slot => slot.Day == current.Day)
                    .Where(slot => slot.StartAt == current.StartAt
                                  || slot.StartAt <= current.EndAt && slot.StartAt >= current.StartAt
                                  || slot.EndAt >= current.StartAt && slot.EndAt <= current.EndAt)
                    .ToList());



                foreach (var value in values)
                {
                    var overLaps = GetoverLaps(value);
                    score -= overLaps.GroupBy(slot => slot.LecturerId).Sum(x => x.Count() - 1);
                    //score -= overLaps.GroupBy(slot => slot.PlaceId).Sum(x => x.Count() - 1);
                    //score -= overLaps.GroupBy(slot => slot.CourseId).Sum(x => x.Count() - 1);
                    //score -= overLaps.Sum(item => item.Students.Intersect(value.Students).Count());
                }

                score -= values.GroupBy(v => v.Day).Count() * 0.5;
                return Math.Pow(Math.Abs(score), -1);
            }
        }
    }


}
