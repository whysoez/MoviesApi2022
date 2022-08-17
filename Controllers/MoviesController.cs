using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi2022.Models;
using System.Diagnostics.CodeAnalysis;


namespace MoviesApi2022.Controllers
{
    // tùy biến route/action theo tên phương thức
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesDbContext _context;

        public MoviesController(MoviesDbContext context)
        {
            _context = context;
        }

        [HttpGet("/error")]
            public string error()
            {
                return "Trang bi loi";
            }

        // update tên method không phải là getMovies
        // GET: api/Movies

        [HttpGet("name2/[action]/{name?}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviee(string name)
        {
            var result = await (from movie in _context.Movies.Include("Actors").Include("Category")
                                where movie.Name == name
                                select movie).ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        // get theo tên cate và tên movie
        //[HttpGet("name3/tam ly/?namemovie=john wick")]
        [HttpGet("name3/{catename}/{namemovieee?}")]
        public async Task<ActionResult<Movie>> GetMovieInCategory(string catename, string namemovie)
        {
            // valid data từ model
            if (ModelState.IsValid) { }
            // binding data từ form.
            var cate = this.Request.Form["id"];
            var resultMovie = await (from movie in _context.Movies
                                     .Include("Category")
                                     .Include("Actors")
                                     let category = movie.Category
                                     where category.CategoryName == catename && movie.Name == namemovie 
                              select movie).FirstOrDefaultAsync();
            
            if (resultMovie == null)
            {
                return NotFound();
            }
            return resultMovie;
        }
        
        // get theo Id cateId và tên movie
        [HttpGet("name2/[action]/{cateId}/{name?}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovieInCategoryId(int cateId, string name)
        {
            var result = await (from movie in _context.Movies.Include("Actors")
                                where movie.Name == name && movie.CategoryId == cateId
                                select movie).ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }


        // tìm theo keywork movies, tìm kiếm theo tên categories
        // chỉnh sửa method trả về list các diễn viên trong bộ phim có MovieId = id
        // GET: api/Movies/5
        [HttpGet("name/{ida?}")]
        public async Task<ActionResult<IEnumerable<Actor>>> GetMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var actor = await (from s in _context.Movies.Include("Actors")
                               from c in s.Actors
                               where s.MovieId == id
                               select c).ToListAsync();

            if (actor == null)
            {
                return NotFound();
            }

            return actor;
        }

        [HttpGet("name1/{idb?}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie1(int idd)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movies = await (from s in _context.Movies.Include("Actors").Include("Category")
                                where s.MovieId == idd
                                select s).ToListAsync();

            if (movies == null)
            {
                return NotFound();
            }

            return movies;
        }
        [HttpGet("page/{i?}")]
        public async Task<ActionResult<customMovie>> GetMoviee(int i)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movies = await (from s in _context.Movies
                                select s).Skip((i-1)*10).Take(10).ToListAsync();
            var cus = new customMovie();

            var a = new Movie();
            var b = a.GetType().GetProperties();
            cus.headers = new List<autoHead>();
            foreach (var item in b)
            {
                foreach (var att in item.GetCustomAttributes(typeof(DisplayNameAttribute) ,true))
                {
                    DisplayNameAttribute attr = (DisplayNameAttribute)att;
                    string? an = attr.DisplayName;
                    if(an != null)
                    {
                        autoHead au = new autoHead();
                        au.propertyType = item.PropertyType.ToString().Split('.')[1];
                        // đổi ký tự đầu tiên thành chữ thường
                        string convert = item.Name.ToString();
                        var re = convert.Substring(0, 1).ToLower();
                        convert = convert.Replace(convert.Substring(0,1), re);
                        au.propertyName = convert;
                        au.propertyValue = an;
                        cus.headers.Add(au);
                    }
                };
            }

            //List<Movie> moviesList = new List<Movie>();
            cus.Movies = new List<Movie>();
            movies.ForEach(movie =>
            {
                cus.Movies.Add(movie);
            });

            cus.Id = i;

            if (cus == null)
            {
                return NotFound();
            }

            return cus;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.MovieId)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
          if (_context.Movies == null)
          {
              return Problem("Entity set 'MoviesDbContext.Movies'  is null.");
          }
            if (ModelState.IsValid)
            {
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMovie", new { id = movie.MovieId }, movie);
            }
            return RedirectToAction("errorValid");
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return (_context.Movies?.Any(e => e.MovieId == id)).GetValueOrDefault();
        }

        public string errorValid()
        {
            return "Du lieu khong dung";
        }
    }
}
