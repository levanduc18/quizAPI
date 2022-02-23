using QuizAPI.Controllers;
using QuizAPI.Models;
using System.Net.Http;
using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace QuizAPI
{
    public class QuizControllerTest
    {
        private QuestionContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<QuestionContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new QuestionContext(options);

            var answer1 = new Answer { Id = 1, AnswerContent = "In Asia", IsAnswer = true };
            var answer2 = new Answer { Id = 2, AnswerContent = "In Europe", IsAnswer = false };
            var answer3 = new Answer { Id = 3, AnswerContent = "In Africa", IsAnswer = false };
            var answer4 = new Answer { Id = 4, AnswerContent = "In America", IsAnswer = false };
            var question1 = new Question { Id = 1, Title = "Where is Vietnam?", AnswerArray = new Answer[] { answer1, answer2, answer3, answer4 } };
            context.Add(question1);
            context.SaveChangesAsync();
            return context;
        }

        [Fact]
        public async Task Get_Questions_Returns_The_Correct_Number_Of_Question()
        {
            // Arrange
            var context = GetContextWithData();
            var controller = new QuestionsController(context);
            var result = await controller.GetQuestions();
            var lstQuestions = ((OkObjectResult)result.Result).Value as IEnumerable<Question>;
            Assert.Equal(1, lstQuestions.Count());
        }

        [Fact]
        public async Task Get_QuestionById_WithQuestionIdNotValid_ShouldBeReturnNotFound()
        {
            // Arrange
            var context = GetContextWithData();
            var controller = new QuestionsController(context);
            var result = await controller.GetQuestion(2);
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Add_QuestionSuccess_ShouldBeReturnCreated()
        {
            // Arrange
            var context = GetContextWithData();
            var controller = new QuestionsController(context);
            var answer1 = new Answer { Id = 5, AnswerContent = "In Asia", IsAnswer = true };
            var answer2 = new Answer { Id = 6, AnswerContent = "In Europe", IsAnswer = false };
            var answer3 = new Answer { Id = 7, AnswerContent = "In Africa", IsAnswer = false };
            var answer4 = new Answer { Id = 8, AnswerContent = "In America", IsAnswer = false };
            var question1 = new Question { Id = 2, Title = "Where is Vietnam?", AnswerArray = new Answer[] { answer1, answer2, answer3, answer4 } };
            var result = await controller.PostQuestion(question1);

            Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task Edit_QuestionWithNotMatchId_ShouldBeReturnBadRequest()
        {
            // Arrange
            var context = GetContextWithData();
            var controller = new QuestionsController(context);
            var answer1 = new Answer { Id = 5, AnswerContent = "In Asia1", IsAnswer = true };
            var answer2 = new Answer { Id = 6, AnswerContent = "In Europe1", IsAnswer = false };
            var answer3 = new Answer { Id = 7, AnswerContent = "In Africa1", IsAnswer = false };
            var answer4 = new Answer { Id = 8, AnswerContent = "In America1", IsAnswer = false };
            var question1 = new Question { Id = 3, Title = "Where is Vietnam?", AnswerArray = new Answer[] { answer1, answer2, answer3, answer4 } };
            var result = await controller.PutQuestion(2, question1);
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }
    }
}