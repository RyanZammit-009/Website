using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APICoreLibrary.DataModel
{
    [Table("Player")]
    public class PlayerDM
    {
        [Key]
        public int P_ID { get; set; }
        public ICollection<PlayerQuestionAnswerDM> PlayerQuestions { get; set; }
    }
}
