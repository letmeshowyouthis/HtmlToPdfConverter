using HtmlToPdfConverter.BL.Services;
using HtmlToPdfConverter.DAL.Managers;
using HtmlToPdfConverter.DAL.Models;
using HtmlToPdfConverter.WEB.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HtmlToPdfConverter.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConverterService _service;
        private readonly IFileManager _fileManager;
        private readonly ISessionManager _sessionManager;

        public HomeController(
            IConverterService service,
            IFileManager fileManager,
            ISessionManager sessionManager)
        {
            _service = service;
            _fileManager = fileManager;
            _sessionManager = sessionManager;
        }

        [HttpGet]
        public ActionResult Convert()
        {
            return View("~/Views/Home/Convert.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Convert(IFormFile? file)
        {
            const string htmlContentType = "text/html";
            if (file == null
                || file.ContentType != htmlContentType)
                return RedirectToAction("Convert");

            var bytes = await file.GetBytesAsync();
            try
            {
                var sessionId = await _service.ConvertAsync(bytes, file.FileName);
                return RedirectToAction("Details", new { sessionId = sessionId });
            }
            catch
            {
                return View("~/Views/Home/Error.cshtml");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid sessionId)
        {
            var session = await _sessionManager.FindAsync(sessionId);
            if (session == null)
                return NotFound();

            return View("~/Views/Home/Details.cshtml", sessionId);
        }

        [HttpGet("GetStatus")]
        public async Task<JsonResult> GetStatus(Guid sessionId)
        {
            var session = await _sessionManager.FindAsync(sessionId);
            if (session == null)
                return JsonFail("Session is not found.");

            switch (session.Status)
            {
                case SessionStatus.IncorrectSourceFile:
                    return JsonFail("Your file can not be converted into PDF!");
                case SessionStatus.StoredConvertedFile:
                    return JsonSuccess(new { isDone = true });

                default:
                    return JsonSuccess(new { isDone = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Download(Guid sessionId)
        {
            var session = await _sessionManager.FindAsync(sessionId);
            if (session == null)
                return NotFound();

            if (session.Status != SessionStatus.StoredConvertedFile)
                return NotFound();

            var file = await _fileManager.FindAsync(session.ConvertedFileUniqueFullName);
            var fileName = $"{session.FileDisplayName}.pdf";
            return File(file, "application/pdf", fileName);
        }

        private static JsonResult JsonSuccess(object? data = null)
        {
            return new JsonResult(new { success = true, data = data });
        }

        private static JsonResult JsonFail(string errorMessage)
        {
            return new JsonResult(new { success = false, errorMessage = errorMessage });
        }
    }
}
