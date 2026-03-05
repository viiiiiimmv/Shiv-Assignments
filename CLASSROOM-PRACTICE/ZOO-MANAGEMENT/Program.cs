/*
This coding question evaluates inheritance, interfaces, and C# concepts,
ideal for junior-level roles.

The problem requires implementing a zoo management system with classes for animals
and the zoo, including methods for adding, removing, and counting animals.
*/
using System;
using System.Collections.Generic;

namespace ZOO_MANAGEMENT
{
    // Interface for Animal behavior
    public interface IAnimal
    {
        string Name { get; }
        string Species { get; }
        void MakeSound();
    }

    // Base Animal class
    public abstract class Animal : IAnimal
    {
        public string Name { get; protected set; }
        public string Species { get; protected set; }

        public Animal(string name, string species)
        {
            Name = name;
            Species = species;
        }

        public abstract void MakeSound();
    }

    // Example derived class
    public class Lion : Animal
    {
        public Lion(string name) : base(name, "Lion") { }

        public override void MakeSound()
        {
            // TODO: Implement lion sound
            Console.WriteLine("Roaring...");
        }
    }

    // Zoo class
    public class Zoo
    {
        private List<IAnimal> animals = new List<IAnimal>();

        public void AddAnimal(IAnimal animal)
        {
            // TODO: Add animal
            animals.Add(animal);
        }

        public void RemoveAnimal(string name)
        {
            // TODO: Remove animal by name
            animals.RemoveAll(a => a.Name == name);
        }

        public int GetAnimalCount()
        {
            // TODO: Return total animals
            return animals.Count;
        }

        public void DisplayAllAnimals()
        {
            // TODO: Print all animals
            foreach (var animal in animals)
            {
                Console.WriteLine(animal.Name+"\t"+animal.Species);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Zoo zoo = new Zoo();

            zoo.AddAnimal(new Lion("Simba"));
            zoo.AddAnimal(new Lion("Mufasa"));

            zoo.DisplayAllAnimals();

            Console.WriteLine("Total Animals: " + zoo.GetAnimalCount());

            zoo.RemoveAnimal("Simba");

            Console.WriteLine("After Removal:");
            zoo.DisplayAllAnimals();

            Console.ReadLine();
        }
    }
}