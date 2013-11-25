﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericsCovarianceContravariance
{

    public interface IEntity
    {
        bool IsValid();
    }

    public class Person
    {
        public string Name { get; set; }
    }

    public class Employee : Person, IEntity
    {
        public int Id { get; set; }

        public virtual void DoWork()
        {
            Console.WriteLine("Do some work");
        }

        public bool IsValid()
        {
            return true;
        }
    }

    public class Manager : Employee
    {
        public override void DoWork()
        {
            Console.WriteLine("Send meeting request");
        }
    }
}
