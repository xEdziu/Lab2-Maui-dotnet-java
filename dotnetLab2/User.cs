using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLab2
{
    class User
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? website { get; set; }

        public override string ToString()
        {
            return $"{name} ({username}), Email: {email}, Phone: {phone}, Website: {website}\n";
        }
    }
}
