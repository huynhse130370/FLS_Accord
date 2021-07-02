using FLS_Accord.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLS_Accord
{
    class AddDataToSchedule
    {
        private readonly FLSContext _context;

        public AddDataToSchedule(FLSContext context)
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
            var courseInputList = new List<CourseInput>();
            var subjectInputList = new List<SubjectInput>();
            var lecturerInputList = new List<LecturerInput>();
            var subjectRegisterInputList = new List<SubjectRegister>();
            var teachableSubjectInputList = new List<TeachableSubjectInput>();
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

            GenerateTimetableInput generate = new GenerateTimetableInput(courseInputList, subjectInputList, lecturerInputList, subjectRegisters, teachableSubjectInputList, 1);
            return generate;
        }
    }
}
