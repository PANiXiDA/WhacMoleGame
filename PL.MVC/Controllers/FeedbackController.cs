using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Common.SearchParams;
using PL.MVC.Infrastructure.Models;
using PL.MVC.Infrastructure.Requests;
using Entities;

namespace PL.MVC.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbacksBL _feedbacksBL;

        public FeedbackController(IFeedbacksBL feedbacksBL)
        {
            _feedbacksBL = feedbacksBL;
        }

        [HttpGet]
        public async Task<JsonResult> GetFeedbacks()
        {
            var feedbacks = FeedbackModel.FromEntitiesList((await _feedbacksBL.GetAsync(new FeedbacksSearchParams())).Objects);

            return Json(feedbacks);
        }

        [HttpPost]
        public async Task<JsonResult> AddFeedback([FromBody] FeedbackRequest feedbackRequest)
        {
            var feedback = new Feedback(
                0,
                feedbackRequest.Title,
                feedbackRequest.Description,
                feedbackRequest.CountStars);

            await _feedbacksBL.AddOrUpdateAsync(feedback);

            return Json(new { ok = true });
        }
    }
}
