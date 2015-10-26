using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibraryTest.UnitsTest.EntityFramework.DataContext
{

    public abstract class Animal
    {
        /// <summary>
        /// This needs to be an auto generated column on the animal table. Having it to not auto generate on animal didn't work. 
        /// Dog and Cat should not be auto generated. Essentially should write / use the id from animal.
        /// </summary>
        [Key]
        public int AnimalId { get; set; }
        public string Size { get; set; }
    }

    public class Dog : Animal
    {
        public int Bark { get; set; }
    }

    public class Cat : Animal
    {
        public int Meow { get; set; }
    }

}
