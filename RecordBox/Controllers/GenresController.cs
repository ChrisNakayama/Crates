using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecordBox.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RecordBox.Controllers
{
  [Authorize]
  public class GenresController : Controller
  {
    private readonly RecordBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public GenresController(UserManager<ApplicationUser> userManager, RecordBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var userGenres = _db.Genres.Where(entry => entry.User.Id == currentUser.Id).ToList();
      return View(userGenres);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Genre genre, int RecordId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      genre.User = currentUser;
      _db.Genres.Add(genre);
      if (RecordId != 0)
      {
        _db.RecordGenres.Add(new RecordGenre{ GenreId = genre.GenreId, RecordId = RecordId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Genre foundGenre = _db.Genres
        .Include(genre => genre.JoinEntities)
        .ThenInclude(join => join.Record)
        .FirstOrDefault(model => model.GenreId == id);
      return View(foundGenre);
    }

    public ActionResult Edit(int id)
    {
      Genre foundGenre = _db.Genres.FirstOrDefault(model => model.GenreId == id);
      return View(foundGenre);
    }

    [HttpPost]
    public ActionResult Edit(Genre genre)
    {
      _db.Entry(genre).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = genre.GenreId});
    }

    public ActionResult Delete(int id)
    {
      Genre foundGenre = _db.Genres.FirstOrDefault(model => model.GenreId == id);
      return View(foundGenre);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Genre foundGenre = _db.Genres.FirstOrDefault(model => model.GenreId == id);
      _db.Genres.Remove(foundGenre);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> AddRecord(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.RecordId = new SelectList(_db.Records.Where(entry => entry.User.Id == currentUser.Id), "RecordId", "Name");
      Genre foundGenre = _db.Genres.FirstOrDefault(model => model.GenreId == id);
      return View(foundGenre);
    }

    [HttpPost]
    public ActionResult AddRecord(Genre genre, int RecordId)
    {
      if (RecordId != 0)
      {
        _db.RecordGenres.Add(new RecordGenre{ GenreId = genre.GenreId, RecordId = RecordId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteRecord(int id)
    {
      var joinEntry = _db.RecordGenres.FirstOrDefault(entry => entry.RecordGenreId == id);
      _db.RecordGenres.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}