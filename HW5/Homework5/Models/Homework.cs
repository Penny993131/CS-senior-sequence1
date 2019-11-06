﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Homework5.models
{
    public class Homework
    {

        [Key]
        public int ID { get; set; }

        [Required]
        public string HomeworkPriority { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public TimeSpan DueTime { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public int Course { get; set; }

        [Required]
        public string HomeworkTitle { get; set; }

        [Required]
        public string Notes { get; set; }



        public override string ToString()
        {
            return $"{base.ToString()}: {HomeworkPriority} {DueDate} {DueTime} {Department} {Course} {HomeworkTitle} {Notes}";
        }

    }

}