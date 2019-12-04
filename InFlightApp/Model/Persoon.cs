﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InFlightApp.Model {
    public class Persoon {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }

        public override string ToString() {
            return $"{FirstName} {LastName}";
        }
    }
}