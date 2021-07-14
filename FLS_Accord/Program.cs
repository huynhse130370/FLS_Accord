using Accord.Genetic;
using FLS_Accord.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static FLS_Accord.TimeTableGenerationHandler;

namespace FLS_Accord
{
    class Program
    {
        static void Main(string[] args)
        {
            FLSContext context = new FLSContext();

            DataManagement model = new DataManagement(context);

            GenerateTimetableInput dataContext = model
                .AddDataToContext();

            using (dataContext)
            {
                Console.WriteLine("Create population ...");
                Population population = new Population(100, new TimeTableChromosome(dataContext),
                    new TimeTableChromosome.FitnessFunction(), new EliteSelection());

                int i = 0;

                Console.WriteLine("Begin creating ...");
                while (true)
                {
                    population.RunEpoch();  
                    i++;
                    Console.WriteLine("Vong lap: " + i);
                    Console.WriteLine("Fitness MAX: " + population.FitnessMax);
                    Console.WriteLine("Fitness Function: " + population.FitnessFunction.Evaluate(population.BestChromosome));
                    if (population.FitnessMax >= 0.99 || i >= 1000)
                    {
                        Console.WriteLine(population.FitnessMax);
                        break;
                    }
                }

                Console.WriteLine("Best Chromosome: " + population.BestChromosome);

                var timetable = (population.BestChromosome as TimeTableChromosome).Value.ToList();


                Console.WriteLine("Lecturer : Course : Subject : Slot");
                foreach (var x in timetable)
                {
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine("Lecturer: " + x.LecturerCode);
                    Console.WriteLine("Course: " + x.CourseName);
                    Console.WriteLine("Subject: " + x.SubjectCode);
                    Console.WriteLine("Start Time: " + x.TimeSlot.StartTime);
                    Console.WriteLine("End Time: " + x.TimeSlot.EndTime);
                    Console.WriteLine("Day Time: " + x.TimeSlot.DayOfWeek);
                    Console.WriteLine("-----------------------------------------");
                }


                //    //IChromosome bestChromosome = population.BestChromosome;

                //    //var put = bestChromosome.Evaluate(new TimeTableChromosome.FitnessFunction());

                //    //Console.WriteLine();

                //    //Console.WriteLine(selectedChromosome.Fitness + "  vs  " + bestChromosome.Fitness + "= " + (selectedChromosome.Fitness.CompareTo(bestChromosome.Fitness) > 0));


                //    //if (selectedChromosome.Fitness.CompareTo(bestChromosome.Fitness) > 0)
                //    //{
                //    //    Console.WriteLine("Select:" + selectedChromosome.Fitness);
                //    //    return selectedChromosome;
                //    //}
                //    //Console.WriteLine("Select:" + bestChromosome.Fitness);
                //    //return bestChromosome;


                //    //dataContext.TimeSlots.AddRange(timetable);
                //    //dataContext.SaveChanges();

            }
        }

    }
}
