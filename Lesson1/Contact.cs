using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1
{
    class Contact
    {
        public Contact(string name, string surname, string email, string pn)
        {
            this.Name = name;
            this.SurName = surname;
            this.Email = email;
            this.PNumber = pn;
        }

        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string PNumber { get; set; }

        public override string ToString()
        {
            return $"{Name}, {SurName}, {PNumber}";
        }
    }
}
