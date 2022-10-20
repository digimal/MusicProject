using Microsoft.AspNetCore.Mvc;
using MvcProject.Bll.Services.Abstract;
using MvcProject.WebApp.Helpers.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MvcProject.WebApp.Controllers
{
    public class ArtistTagController : BaseController
    {
        private readonly IArtistTagService _service;
        private readonly IArtistAssignedTagService _assignedTagService;

        public ArtistTagController(IArtistTagService artistService, IArtistAssignedTagService artistAssignedTagService)
        {
            _service = artistService;
            _assignedTagService = artistAssignedTagService;
        }

        public JsonResult Tags(string query)
        {
            if (Request.IsAjaxRequest())
            {
                return Json(_service.GetMatchesStartsWith(query));
            }

            return Json(string.Empty);
        }

        public IActionResult EditTagsForArtist(int id, string tags)
        {
            IEnumerable<int> tagIds;
            if (tags == string.Empty)
            {
                tagIds = new int[0];
            }
            else
            {
                try
                {
                    tagIds = tags.Split(',').Select(int.Parse).ToArray(); // Warning: Well, yeah, that's pretty bizzare approach 
                }
                catch
                {
                    return BadRequest("Invalid tags argument");
                }
            }
            try
            {
                _assignedTagService.UpdateTagsForArtist(id, tagIds);
            }
            catch
            {
                return BadRequest("Error occured during process of tags saving!");
            }
            return RedirectToAction("Tags", "Artist", new { id });
        }
    }
}