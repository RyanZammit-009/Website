using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICoreLibrary.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuestionWeb.Model;
using Web.Model;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionContext _db;
        public static readonly int QUESTIONS_PER_PLAYER = 5;

        private readonly ILogger<QuestionController> _logger;

        public QuestionController(ILogger<QuestionController> logger, QuestionContext db)
        {
            _db = db;
            _logger = logger;
        }

        // Check that output of endpoint matches the records in the db (questions, answers, iscorrect....) for given player id
        [HttpGet("api/{user}")]
        [HttpGet]
        public IEnumerable<Question> Get(int? user)
        {
            if (user != null)
            {
                var user_id = (int)user;
                var player = GetPlayerIncludingAllRelatedEntities(user_id);
                
                if (player == null)
                {
                    player = InstantiatePlayer(user_id);
                }
                var userQuestions = player.PlayerQuestions;
                return userQuestions.Select(x => new Question()
                {
                    PlayerID = user_id,
                    Answer = x.Question.Answers.Select(a => new Answer()
                    {
                        Text = a.A_AnswerText,
                        IsCorrectAnswer = x.Question.Question.First(y => y.QA_A_ID == a.A_ID).QA_IsCorrectAnswer
                    }).ToArray(),
                    Text = x.Question.Q_QuestionText,
                    IsAnswered = x.PQA_IsAnswered,
                    IsCorrect = x.PQA_IsCorrect
                });
            }
            return null;
        }

        // Write unit test confirming that the method works with null input.
        // Write unit test confirming that the method takes the correct input from JSON and applies it to first question only.
        [HttpPost("api/post")]
        public void Post([FromBody]QuestionPostBody json)
        {
            if (json != null) {
                var user_id = json.User;
                var player = _db.Players.Include(x => x.PlayerQuestions).FirstOrDefault(x => x.P_ID == user_id);
                var question = player.PlayerQuestions.FirstOrDefault(x => x.PQA_IsAnswered == false);
                if (question != null) {
                    question.PQA_IsAnswered = true;
                    question.PQA_IsCorrect = json.IsCorrect;
                    _db.SaveChanges();
                }
            }
        }

        //Write unit test that confirms that the player is not created with existing id
        public PlayerDM InstantiatePlayer(int user_id)
        {
            var player = new PlayerDM()
            {
                P_ID = user_id
            };
            player.PlayerQuestions = GenerateQuestionList(player);
            _db.Players.Add(player);
            _db.SaveChanges();
            return player;
        }

        // Write a unit test for any number and compare programmatically to _db.
        public PlayerDM GetPlayerIncludingAllRelatedEntities(int user_id)
        {
            return _db.Players
                                .Include(x => x.PlayerQuestions)
                                .ThenInclude(x => x.Question).ThenInclude(x => x.Question)
                                .Include(x => x.PlayerQuestions)
                                .ThenInclude(x => x.Question).ThenInclude(x => x.Answers)
                                .FirstOrDefault(x => x.P_ID == user_id);
        }

        // Write a unit test that checks that returned question list length is the same as static number in class 
        public List<PlayerQuestionAnswerDM> GenerateQuestionList(PlayerDM player)
        {
            var result = new List<PlayerQuestionAnswerDM>();
            var list = new HashSet<int>();
            var questions = _db.Questions.ToList();
            var rand = new Random();
            for (int i = 0; i < QUESTIONS_PER_PLAYER; i++)
            {
                var value = rand.Next(0, questions.Count() - 1);
                var diff = 0;
                while (!list.Add(value))
                {
                    var temp = Math.Abs(diff) + 1;
                    diff = diff > 0 ? temp * -1 : temp;
                    value += diff;
                    if (value < 0)
                    {
                        value += questions.Count();
                    }
                    if (value >= questions.Count())
                    {
                        value %= questions.Count();
                    }
                }
                result.Add(new PlayerQuestionAnswerDM()
                {
                    Player = player,
                    Question = questions[list.Last()],
                    PQA_IsAnswered = false
                });
            }
            return result;
        }
    }
}
