using System.Collections.Generic;

namespace FLS_Accord
{
    public class FitNessCalculation
    {


        public FitNessCalculation()
        {

        }

        //public int calMinMaxCouseOfLecturer(List<LecturerInput> lecturerInputs, List<LecturerCourseSlot> listLecturerCourse)
        //{
        //    int conflict = 0;
        //    int count = 0;
        //    foreach (var lecturer in lecturerInputs)
        //    {
        //        foreach (var lecturerCourse in listLecturerCourse)
        //        {
        //            if (lecturer.Equals(lecturerCourse.Lecturer))
        //            {
        //                count++;
        //            }

        //        }
        //        if (lecturer.MinCourse < count || lecturer.MaxCourse > count)
        //        {
        //            conflict++;
        //        }
        //    }
        //    return conflict;
        //}


        //public int checkLecturerTeachableSubject(List<LecturerCourseSlot> listLecturerCourse, GenerateTimetableInput context)
        //{
        //    int conflict = 0;

        //    foreach (var lecturerCourse in listLecturerCourse)
        //    {

        //        SubjectInput subjectInCourse = lecturerCourse.Course.Subject;
        //        LecturerInput lecturerInCourse = lecturerCourse.Lecturer;

        //        foreach (var teachableLecturer in context.TeachableSubjectInputs)
        //        {
        //            if (lecturerInCourse.Equals(teachableLecturer.Lecturer))
        //            {
        //                if (subjectInCourse.Equals(teachableLecturer.Subject))
        //                {
        //                    var isMatch = teachableLecturer.MatchPoint > 2;
        //                    var isPrefer = teachableLecturer.PreferPoint > 2;
        //                    if (!isMatch)
        //                    {
        //                        conflict++;
        //                    }
        //                    if (!isPrefer)
        //                    {
        //                        conflict++;
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    return conflict;
        //}

        //public int checkNotRegister(List<LecturerCourseSlot> listLecturerCourse, GenerateTimetableInput context)
        //{
        //    int conflict = 0;
        //    foreach (var lecturerCourse in listLecturerCourse)
        //    {

        //        SubjectInput subjectInCourse = lecturerCourse.Course.Subject;
        //        LecturerInput lecturerInCourse = lecturerCourse.Lecturer;

        //        foreach (var register in context.SubjectRegisterInputs)
        //        {
        //            if (lecturerInCourse.Equals(register.Lecturer))
        //            {
        //                if (subjectInCourse.Equals(register.Subject))
        //                {
        //                    conflict++;
        //                }

        //            }
        //        }
        //    }
        //    return conflict;
        //}
    }
}
