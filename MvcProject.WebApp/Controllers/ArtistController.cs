using MvcProject.Bll.Services.Abstract;
using MvcProject.Bll.ViewModels.Artist;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Extensions;

namespace MvcProject.WebApp.Controllers
{
    public class ArtistController : BaseController
    {
        private readonly IArtistService service;
        private readonly IPictureService pictureService;
        private readonly IArtistRelationService relationService;
        private readonly IArtistRelationTypeService relationTypeService;
        private readonly IArtistLikeService likeService;


        public ArtistController(
            IArtistService service, 
            IArtistRelationService relationService, 
            IPictureService pictureService, 
            IArtistRelationTypeService relationTypeService,
            IArtistLikeService likeService)
        {
            this.service = service;
            this.pictureService = pictureService;
            this.relationService = relationService;
            this.relationTypeService = relationTypeService;
            this.likeService = likeService;

        }

        public IActionResult Index()
        {
            return View(service.GetArtists());
        }

        public IActionResult Artists()
        {
            return PartialView(service.GetArtists());
        }

        [HttpPost]
        public IActionResult Artist(ArtistBaseViewModel viewModel)
        {
            ViewBag.Update = true;
            return PartialView("BaseArtist", viewModel);
        }


        public IActionResult Details(int id)
        {
            var artist = service.GetArtist(id);
            if (artist == null)
            {
                return InvokeNotFound();
            }
            return View(artist);
        }

        public IActionResult Tags(int id)
        {
            ViewBag.ArtistId = id;
            return PartialView(service.GetTags(id));
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult TagsEdit(int id)
        {
            ViewBag.ArtistId = id;
            return PartialView(service.GetTags(id));
        }

        public IActionResult RelationChoices(int id)
        {
            ViewBag.ArtistId = id;
            var choiceModel = User.IsInRole("Moderator")
                ? relationTypeService.GetAll()
                : service.GetRelationChoiceViewModel(id);
            if (!choiceModel.RelationKeys.Any())
            {
                RelationMap = null;
                return new EmptyResult();
            }
            RelationMap = choiceModel.RelationKeys;
            var listItems = choiceModel.RelationTitles.Select(x => new SelectListItem() { Text = x.Value, Value = x.Key.ToString() });
            return PartialView(new SelectList(listItems, "Value", "Text", listItems.First()));
        }


        public IActionResult RelationMembers(int artistId, int relationId)
        {
            if(RelationMap == null )
            {
                return new EmptyResult();
            }
            var artists = service.GetRelationMembers(artistId, relationId, RelationMap[relationId]);
            return PartialView(artists);
        }

        public IActionResult RelationCandidates(int artistId, int relationId, string query)
        {
            if (RelationMap == null)
            {
                return new EmptyResult();
            }
            return Json(service.GetRelationCandidates(artistId, RelationMap[relationId], query));
        }

        public IActionResult RelationMember(MemberViewModel model)
        {
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult RelationMembersAdd(string model)
        {
            MemberViewModel parsedModel = Newtonsoft.Json.JsonConvert.DeserializeObject(model, typeof(MemberViewModel)) as MemberViewModel;
            return PartialView("RelationMember", relationService.Create(parsedModel, RelationMap[parsedModel.RelationId]));
        }

        [HttpPost]
        public IActionResult RelationMembersEdit(string model)
        {

            MemberViewModel parsedModel = Newtonsoft.Json.JsonConvert.DeserializeObject(model, typeof(MemberViewModel)) as MemberViewModel;
            relationService.Update(parsedModel, RelationMap[parsedModel.RelationId]);
            return new StatusCodeResult((int)HttpStatusCode.OK);
        }

        public IActionResult RelationMembersDelete(string model) // ToDo
        {

            MemberViewModel parsedModel = Newtonsoft.Json.JsonConvert.DeserializeObject(model, typeof(MemberViewModel)) as MemberViewModel;
            relationService.Delete(parsedModel, RelationMap[parsedModel.RelationId]);
            return new Microsoft.AspNetCore.Mvc.StatusCodeResult((int)HttpStatusCode.OK);
        }

        public IActionResult Picture(int? id)
        {
            // Warning: Currently, pic with id=1 is default
            return PartialView(pictureService.Get(id ?? 1)); 
        }

        public IActionResult Likes(int id)
        {
            ViewBag.ArtistId = id;
            return PartialView(service.GetLikes(id, int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))));
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangeLikeState(int id)
        {
            ViewBag.ArtistId = id;
            return PartialView("Likes", likeService.ChangeState(id, int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))));
        }

        [Authorize(Roles = "Moderator")]
        public IActionResult Create()
        {
            ViewBag.Edit = false;
            return PartialView("CreateEdit");
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult Create(ArtistCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var _viewModel = service.Create(WithPictureSaving(viewModel));
                return PartialView("BaseArtist", _viewModel);
            }
            return BadRequest("Model is invalid!");
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult UploadPicture()
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null)
            {
                return BadRequest("No file loaded!");
            }
            if(ArtistPictures == null)
            {
                ArtistPictures = new Dictionary<Guid, IFileInfo>();
            }
            var guid = Guid.NewGuid();
            ArtistPictures.Add(guid, file);
            return Json(new { picId = guid });        
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult DeletePicture(Guid guid)
        {
            if (ArtistPictures == null)
            {
                return Ok();
            }
            ArtistPictures.Remove(guid);
            return Ok();
        }

        private ArtistViewModel WithPictureSaving(ArtistCreateViewModel viewModel)
        {
            if (Guid.TryParse(viewModel.TempPicId, out Guid picGuid) && ArtistPictures.TryGetValue(picGuid, out IFileInfo picture))
            {
                viewModel.PictureId = pictureService
                            .Create(new Bll.ViewModels.Common.PictureViewModel()
                            {
                                Path = Helpers.PictureBuilder.Default.Storage(Helpers.StorageLocation.LocalNotTracked).Save(picture)
                            }).Id;
                ArtistPictures.Remove(picGuid);
            }
            return viewModel;
        }

        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult Edit(int id)
        {
            ViewBag.Edit = true;
            ArtistViewModel artist = service.GetArtist(id);
            if (artist == null)
            {
                return InvokeNotFound(Request.GetDisplayUrl());
            }
            return PartialView("CreateEdit", AutoMapper.Mapper.Map<ArtistViewModel, ArtistCreateViewModel>(artist));
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult Edit(ArtistCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                service.Update(WithPictureSaving(viewModel));
                return PartialView("BaseArtistElement", viewModel);
            }
            return BadRequest("Invalid model!");
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult Delete(int id)
        {
            relationService.DeleteAllArtistRelations(id);
            service.Delete(id);
            return new EmptyResult();
        }

        private Dictionary<int, RelationKeyViewModel> RelationMap
        {
            get => HttpContext.Items["RelationMap"] as Dictionary<int, RelationKeyViewModel>;
            set => HttpContext.Items["RelationMap"] = value;
        }

        private Dictionary<Guid, IFileInfo> ArtistPictures
        {
            get => HttpContext.Items["Picture"] as Dictionary<Guid, IFileInfo>;
            set => HttpContext.Items["Picture"] = value;
        }
    }
}
