using FLS_Accord.Models;
using System;
using System.Collections.Generic;

struct TimeSlotChromosome
{

    public int LecturerId { get; set; }
    public int SubjectId { get; set; }

    public TimeSlotChromosome(int lecturerId, int subjectId)
    {
        LecturerId = lecturerId;
        SubjectId = subjectId;
    }
}