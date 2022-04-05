using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecordBox.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecordBox.Controllers
{
  [Authorize]
  public class RecordsController : Controller
  {
    private readonly RecordBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public RecordsController(UserManager<ApplicationUser> userManager, RecordBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var userRecords = _db.Records.Where(entry => entry.User.Id == currentUser.Id).ToList();
      return View(userRecords);
    }

    public async Task<ActionResult> Create()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.GenreId = new SelectList(_db.Genres.Where(entry => entry.User.Id == currentUser.Id), "GenreId", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Record record, int GenreId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      record.User = currentUser;
      _db.Records.Add(record);
      _db.SaveChanges();
      if (GenreId != 0)
      {
        _db.RecordGenres.Add(new RecordGenre() {GenreId = GenreId, RecordId = record.RecordId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Record foundRecord = _db.Records
        .Include(record => record.JoinEntities)
        .ThenInclude(join => join.Genre)
        .FirstOrDefault(record => record.RecordId == id);
      return View(foundRecord);
    }

    public ActionResult Edit(int id)
    {
      Record foundRecord = _db.Records.FirstOrDefault(record => record.RecordId == id);
      ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name");
      return View(foundRecord);
    }

    [HttpPost]
    public ActionResult Edit(Record record, int GenreId)
    {
      if (GenreId != 0)
      {
        _db.RecordGenres.Add(new RecordGenre() {GenreId = GenreId, RecordId = record.RecordId});
      }
      _db.Entry(record).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Record foundRecord = _db.Records.FirstOrDefault(record => record.RecordId == id);
      return View(foundRecord);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Record foundRecord = _db.Records.FirstOrDefault(record => record.RecordId == id);
      _db.Records.Remove(foundRecord);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> AddGenre(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      ViewBag.GenreId = new SelectList(_db.Genres.Where(entry => entry.User.Id == currentUser.Id), "GenreId", "Name");
      Record foundRecord = _db.Records.FirstOrDefault(record => record.RecordId == id);
      return View(foundRecord);
    }

    [HttpPost]
    public ActionResult AddGenre(Record record, int GenreId)
    {
      if (GenreId != 0)
      {
        _db.RecordGenres.Add(new RecordGenre() {GenreId = GenreId, RecordId = record.RecordId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteGenre(int joinId)
    {
        var joinEntry = _db.RecordGenres.FirstOrDefault(entry => entry.RecordGenreId == joinId);
        _db.RecordGenres.Remove(joinEntry);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
  }
}