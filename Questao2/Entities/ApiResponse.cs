using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questao2.Entities
{
    public class ApiResponse
    {
        public int Page { get; set; }
        public int Per_Page { get; set; }
        public int Total { get; set; }
        public int Total_Pages { get; set; }
        public Match[] Data { get; set; }

        public static implicit operator List<object>(ApiResponse v)
        {
            throw new NotImplementedException();
        }
    }
}