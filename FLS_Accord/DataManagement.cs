using FLS_Accord.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FLS_Accord
{
    public class DataManagement
    {
        private readonly FLSContext _context;

        public DataManagement(FLSContext context)
        {
            _context = context;
        }

        public GenerateTimetableInput AddDataToContext()
        {
            var courses = _context.Course.Include(x => x.Semester)
                                         .Include(x => x.Subject)
                                         .ToList();

            var subjects = _context.Subject.Include(x => x.Course)
                                           .ToList();

            var lecturers = _context.Lecturer.Include(x => x.Department)
                                             .Include(x => x.TeachableSubject)
                                             .Include(x => x.SubjectRegister)
                                             .Include(x => x.LectureSemesterRegister)
                                             .Include(x => x.LecturerType)
                                             .ToList();

            var subjectRegisters = _context.SubjectRegister.Include(x => x.Lecturer)
                                                           .Include(x => x.Subject)
                                                           .ToList();

            var teachableSubject = _context.TeachableSubject.Include(x => x.Lecturer)
                                                           .Include(x => x.Subject)
                                                           .ToList();

            var timeSlots = _context.TimeSlot.ToList();


            var studentGroups = _context.StudentGroup.ToList();

            var courseInputList = new List<CourseInput>();

            var subjectInputList = new List<SubjectInput>();

            var lecturerInputList = new List<LecturerInput>();

            var subjectRegisterInputList = new List<SubjectRegisterInput>();

            var teachableSubjectInputList = new List<TeachableSubjectInput>();

            var studentGroupInputList = new List<StudentGroupInput>();

            var timeSlotList = new List<TimeSlotInput>();


            foreach (var lecturer in lecturers)
            {
                LecturerInput lecturerInput = new LecturerInput
                {
                    Id = lecturer.Id,
                    Department = lecturer.Department,
                    DepartmentId = lecturer.DepartmentId,
                    LecturerCode = lecturer.LecturerCode,
                    LecturerType = lecturer.LecturerType,
                    LecturerTypeId = lecturer.LecturerTypeId,
                    LectureSemesterRegister = lecturer.LectureSemesterRegister.ToList(),
                    MaxCourse = lecturer.MaxCourse,
                    MinCourse = lecturer.MinCourse,
                    PriorityPoint = lecturer.PriorityPoint,
                    Status = lecturer.Status,
                    SubjectRegister = lecturer.SubjectRegister.ToList(),
                    TeachableSubject = lecturer.TeachableSubject.ToList(),
                };
                lecturerInputList.Add(lecturerInput);
            }
            foreach (var course in courses)
            {
                CourseInput courseInput = new CourseInput
                {
                    Id = course.Id,
                    Name = course.Name,
                    Semester = course.Semester,
                    SemesterId = course.SemesterId,
                    SubjectId = course.SubjectId,
                };
                courseInputList.Add(courseInput);
            }

            foreach (var subject in subjects)
            {
                SubjectInput subjectInput = new SubjectInput(subject.Name, courseInputList.Where(x => x.SubjectId == subject.Id).ToList());
                subjectInputList.Add(subjectInput);
            }

            foreach (var teachable in teachableSubject)
            {
                TeachableSubjectInput teachableinput = new TeachableSubjectInput
                {
                    Lecturer = lecturerInputList.SingleOrDefault(x => x.Id == teachable.LecturerId),
                    LecturerId = teachable.LecturerId,
                    MatchPoint = teachable.MatchPoint,
                    PreferPoint = teachable.PreferPoint,
                    Subject = subjectInputList.SingleOrDefault(x => x.Name.Equals(teachable.Subject.Name)),
                    SubjectId = teachable.SubjectId
                };
                teachableSubjectInputList.Add(teachableinput);
            }

            foreach(var subjectRegister in subjectRegisters)
            {
                SubjectRegisterInput subjectRegisterInput = new SubjectRegisterInput
                {
                    Id = subjectRegister.Id,
                    LecturerId = subjectRegister.LecturerId,
                    SemesterPlanId = subjectRegister.SemesterPlanId,
                    SubjectId = subjectRegister.SubjectId,
                    Lecturer = lecturerInputList.SingleOrDefault(x => x.Id == subjectRegister.LecturerId),
                    Subject = subjectInputList.SingleOrDefault(x => x.Name.Equals(subjectRegister.Subject.Name)),
                };
                subjectRegisterInputList.Add(subjectRegisterInput);
            }

            foreach(var timeSlot in timeSlots)
            {
                TimeSlotInput timeSlotInput = new TimeSlotInput
                {
                    Id = timeSlot.Id,
                    Name = timeSlot.Name,
                    StartTime = timeSlot.StartTime,
                    EndTime = timeSlot.EndTime,
                    DayOfWeek = timeSlot.DayOfWeek,
                };
                timeSlotList.Add(timeSlotInput);
            }

            foreach (var studentGroup in studentGroups)
            {
                StudentGroupInput studentGroupInput = new StudentGroupInput
                {
                    Id = studentGroup.Id,
                    Name = studentGroup.Name,
                };
                studentGroupInputList.Add(studentGroupInput);
            }

            GenerateTimetableInput generate = new GenerateTimetableInput(courseInputList, subjectInputList, lecturerInputList, timeSlotList, subjectRegisterInputList, teachableSubjectInputList, studentGroupInputList);
            return generate;
        }
    }
}
