using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Bll.ViewModels.Artist;
using MvcProject.Domain;

namespace MvcProject.WebNewApp.Controllers
{
    public class ArtistController : BaseController
    {
        private IDictionary<string, string> dict = new Dictionary<string, string>();

        private readonly IArtistService _service;
        private readonly IPictureService _pictureService;
        private readonly IArtistRelationService _relationService;
        private readonly IArtistRelationTypeService _relationTypeService;
        private readonly IArtistLikeService _likeService;
        private readonly IMapper _mapper;


        public ArtistController(
            IArtistService service,
            IArtistRelationService relationService,
            IPictureService pictureService,
            IArtistRelationTypeService relationTypeService,
            IArtistLikeService likeService,
            UserManager<User> userManager,
            IMapper mapper)
            : base(userManager)
        {
            _service = service;
            _pictureService = pictureService;
            _relationService = relationService;
            _relationTypeService = relationTypeService;
            _likeService = likeService;
            _mapper = mapper;

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Artist", _service.GetArtists());
        }

        public ActionResult Artists()
        {
            return PartialView(_service.GetArtists());
        }

        [HttpPost]
        [Route("Artist/{s}")]
        public ActionResult Artist([FromBody] ArtistBaseViewModel viewModel, [FromRoute] string s, [FromQuery] string a)
        {
            ViewBag.Update = true;
            return PartialView("BaseArtist", viewModel);
        }


        public IActionResult Details(int id)
        {
            var artist = _service.GetArtist(id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        public ActionResult Tags(int id)
        {
            ViewBag.ArtistId = id;
            return PartialView(_service.GetTags(id));
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public ActionResult TagsEdit(int id)
        {
            ViewBag.ArtistId = id;
            return PartialView(_service.GetTags(id));
        }

        public ActionResult RelationChoices(int id)
        {
            ViewBag.ArtistId = id;
            var choiceModel = User.IsInRole("Moderator")
                ? _relationTypeService.GetAll()
                : _service.GetRelationChoiceViewModel(id);
            if (!choiceModel.RelationKeys.Any())
            {
                RelationMap = null;
                return new EmptyResult();
            }
            RelationMap = choiceModel.RelationKeys;
            var listItems = choiceModel.RelationTitles.Select(x => new SelectListItem() { Text = x.Value, Value = x.Key.ToString() });
            return PartialView(new SelectList(listItems, "Value", "Text", listItems.First()));
        }


        public ActionResult RelationMembers(int artistId, int relationId)
        {
            if (RelationMap == null)
            {
                return new EmptyResult();
            }
            var artists = _service.GetRelationMembers(artistId, relationId, RelationMap[relationId]);
            return PartialView(artists);
        }

        public ActionResult RelationCandidates(int artistId, int relationId, string query)
        {
            if (RelationMap == null)
            {
                return new EmptyResult();
            }
            return Json(_service.GetRelationCandidates(artistId, RelationMap[relationId], query));
        }

        public ActionResult RelationMember(MemberViewModel model)
        {
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult RelationMembersAdd(string model)
        {
            MemberViewModel parsedModel = Newtonsoft.Json.JsonConvert.DeserializeObject(model, typeof(MemberViewModel)) as MemberViewModel;
            return PartialView("RelationMember", _relationService.Create(parsedModel, RelationMap[parsedModel.RelationId]));
        }

        [HttpPost]
        public ActionResult RelationMembersEdit(string model)
        {

            MemberViewModel parsedModel = Newtonsoft.Json.JsonConvert.DeserializeObject(model, typeof(MemberViewModel)) as MemberViewModel;
            _relationService.Update(parsedModel, RelationMap[parsedModel.RelationId]);
            return Ok();
        }

        public ActionResult RelationMembersDelete(string model) // ToDo
        {

            MemberViewModel parsedModel = Newtonsoft.Json.JsonConvert.DeserializeObject(model, typeof(MemberViewModel)) as MemberViewModel;
            _relationService.Delete(parsedModel, RelationMap[parsedModel.RelationId]);
            return Ok();
        }

        public ActionResult Picture(int? id)
        {
            // Warning: Currently, pic with id=1 is default
            return PartialView(_pictureService.Get(id ?? 1));
        }

        [HttpGet]
        public IActionResult Likes(int artistId)
        {
            ViewBag.ArtistId = artistId;
            return PartialView("Likes", _likeService.GetLikes(artistId, GetUserId()));
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangeLikeState(int artistId)
        {
            ViewBag.ArtistId = artistId;
            return PartialView("Likes", _likeService.ChangeState(artistId, GetUserId()));
        }

        [Authorize(Roles = "Moderator")]
        public ActionResult Create()
        {
            ViewBag.Edit = false;
            return PartialView("CreateEdit");
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public ActionResult Create(ArtistCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var _viewModel = _service.Create(WithPictureSaving(viewModel));
                return PartialView("BaseArtist", _viewModel);
            }
            return BadRequest("Model is invalid!");
        }

        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public ActionResult Edit(int id)
        {
            ViewBag.Edit = true;
            ArtistViewModel artist = _service.GetArtist(id);
            if (artist == null)
            {
                return NotFound(Request.GetDisplayUrl());
            }

            var model = _mapper.Map<ArtistViewModel, ArtistCreateViewModel>(artist);

            return PartialView("CreateEdit", model);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public ActionResult Edit(ArtistCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _service.Update(WithPictureSaving(viewModel));
                return PartialView("BaseArtistElement", viewModel);
            }
            return BadRequest("Invalid model!");
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public ActionResult Delete(int id)
        {
            _relationService.DeleteAllArtistRelations(id);
            _service.Delete(id);
            return new EmptyResult();
        }

        private ArtistViewModel WithPictureSaving(ArtistCreateViewModel viewModel)
        {
            return viewModel;
        }

        private Dictionary<int, RelationKeyViewModel> RelationMap
        {
            get => HttpContext.Items["RelationMap"] as Dictionary<int, RelationKeyViewModel>;
            set => HttpContext.Items["RelationMap"] = value;
        }
    }
}
