using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model
{
    public class QuestionPostBody
    {
        public int User { get; set; }
        public bool IsCorrect { get; set; }
    }
}
