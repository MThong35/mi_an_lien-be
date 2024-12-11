using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using thesis_comicverse_webservice_api.Database;
using thesis_comicverse_webservice_api.DTOs;
using thesis_comicverse_webservice_api.Models;

namespace thesis_comicverse_webservice_api.Repositories
{
    public interface IComicRepository
    {
        Task<List<Comic>> GetAllComics();
        Task<Comic> GetComicByIdAsync(int id);
        void AddComic(Comic comic);
        void DeleteComic(int id);
        void UpdateComic(Comic comic);
        Task<List<Author>> GetAllAuthorsAsync();
        Task<Author> GetAuthorByIdAsync(int id);
        Task<List<dynamic>> GetAllComicReleasingAsync();
        Task<Releasing> GetReleasingByIdAsync(int id);
        Task<Publisher> GetPublisherByIdAsync(int id);
        Task<List<Publisher>> GetAllPublishersAsync();
        Task<dynamic> UpdateReleasingAsync(ComicReleasingDTO releasing);
        Task<dynamic> CreateReleasingAsync(ComicReleasingDTO releasing);
    }

    public class ComicRepository : IComicRepository
    {
        private readonly AppDbContext _context;

        private readonly ILogger<ComicRepository> _logger;

        public ComicRepository(AppDbContext context, ILogger<ComicRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Comic>> GetAllComics()
        {
            try
            {
                if (_context.Comic == null) throw new ArgumentNullException(nameof(_context.Comic));

                var comics = await _context.Comic.ToListAsync();

                return comics;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve comics: {ex.Message}");
            }
        }

        public async Task<Comic> GetComicByIdAsync(int id)
        {
            try
            {
                if (_context.Comic == null) throw new ArgumentNullException(nameof(_context.Comic));

                var comic = await _context.Comic!.FirstOrDefaultAsync(p => p.ComicId == id);

                if (comic == null) throw new ArgumentNullException(nameof(comic));

                return comic;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve comic: {ex.Message}");
            }
        }

        public void AddComic(Comic comic)
        {
            try
            {
                //Condition
                if (comic == null) throw new ArgumentNullException(nameof(comic));


                _logger.LogInformation($"comic {comic.ComicId}__{comic.comicTitle}");

                // Do something
                _context.Comic!.Add(comic);
                //_context.
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't add comic: {ex.Message}");
            }
        }

        public void UpdateComic(Comic comic)
        {
            try
            {
                if (comic == null) throw new ArgumentNullException(nameof(comic));

                _context.Entry(comic).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't update comic: {ex.Message}");
            }
        }

        public void DeleteComic(int id)
        {
            try
            {
                if (_context.Comic == null) throw new ArgumentNullException(nameof(_context.Comic));

                var comic = _context.Comic.Find(id);

                if (comic == null) throw new ArgumentNullException(nameof(comic));

                _context.Comic.Remove(comic);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't delete comic: {ex.Message}");
            }
        }

        //public
        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            try
            {
                if (_context.Author == null) throw new ArgumentNullException(nameof(_context.Author));

                var authors = await _context.Author.ToListAsync();

                return authors;

            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve authors: {ex.Message}");
            }
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            try
            {
                if (_context.Author == null) throw new ArgumentNullException(nameof(_context.Author));

                var author = await _context.Author!.FirstOrDefaultAsync(p => p.authorID == id);

                if (author == null) throw new ArgumentNullException(nameof(author));

                return author;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve author: {ex.Message}");
            }
        }

        public async Task<List<dynamic>> GetAllComicReleasingAsync()
        {
            try
            {
                if (_context.Comic == null || _context.Releasing == null)
                {
                    _logger.LogInformation("Comic or Releasing is null");
                    throw new ArgumentNullException("Comic or Releasing context is null");
                }

                // Perform the LEFT JOIN between Comic and Releasing
                var result = await (from comic in _context.Comic
                                    join releasing in _context.Releasing
                                    on comic.releasingID equals releasing.releasingID into releasingGroup
                                    from r in releasingGroup.DefaultIfEmpty() // Left Join
                                    select new
                                    {
                                        comic.ComicId,
                                        comic.comicTitle,
                                        comic.localhostURL,
                                        comic.remoteURL,
                                        comic.taskID,
                                        comic.releaseDate,
                                        comic.publisherID,
                                        comic.releasingID,
                                        comic.authorID,
                                        comic.language,
                                        comic.categoryID,
                                        comic.Description,
                                        comic.avatarURL,
                                        isApprove = r == null ? null : r.isApprove, // Handle nullable properties
                                        approveAt = r == null ? null : r.approveAt,
                                        userID = r == null ? null : r.userID,
                                        status = r == null ? null : r.status,
                                        licenseID = r == null ? null : r.licenseID
                                    }).ToListAsync();

                _logger.LogInformation($"{JsonConvert.SerializeObject(result)}");
                var dynamicResult = result.Select(x => (dynamic)x).ToList();
                return dynamicResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Couldn't retrieve data: {ex.Message}");
                throw new Exception($"Couldn't retrieve data: {ex.Message}");
            }
        }


        public async Task<Releasing> GetReleasingByIdAsync(int id)
        {
            try
            {
                if (_context.Releasing == null) throw new ArgumentNullException(nameof(_context.Releasing));

                var releasing = await _context.Releasing!.FirstOrDefaultAsync(p => p.releasingID == id);

                if (releasing == null) throw new ArgumentNullException(nameof(releasing));

                return releasing;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve releasing: {ex.Message}");
            }
        }

        public async Task<Publisher> GetPublisherByIdAsync(int id)
        {
            try
            {
                if (_context.Publisher == null) throw new ArgumentNullException(nameof(_context.Publisher));

                var publisher = await _context.Publisher!.FirstOrDefaultAsync(p => p.publisherID == id);

                if (publisher == null) throw new ArgumentNullException(nameof(publisher));

                return publisher;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve publisher: {ex.Message}");
            }
        }

        public async Task<List<Publisher>> GetAllPublishersAsync()
        {
            try
            {
                if (_context.Publisher == null) throw new ArgumentNullException(nameof(_context.Publisher));

                var publishers = await _context.Publisher.ToListAsync();

                return publishers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve publishers: {ex.Message}");
            }
        }
        public async Task<dynamic> UpdateReleasingAsync(ComicReleasingDTO releasing)
        {
            try
            {
                // Validate input
                if (releasing == null)
                    throw new ArgumentNullException(nameof(releasing));

                // Update Comic entity
                var comicEntity = await _context.Comic.FindAsync(releasing.ComicId);
                if (comicEntity == null)
                    throw new Exception("Comic not found.");

                comicEntity.comicTitle = releasing.comicTitle;
                comicEntity.localhostURL = releasing.localhostURL;
                comicEntity.remoteURL = releasing.remoteURL;
                comicEntity.taskID = releasing.taskID;
                comicEntity.releaseDate = releasing.releaseDate;
                comicEntity.publisherID = releasing.publisherID;
                comicEntity.authorID = releasing.authorID;
                comicEntity.language = releasing.language;
                comicEntity.categoryID = releasing.categoryID;
                comicEntity.Description = releasing.Description;
                comicEntity.avatarURL = releasing.avatarURL;

                _context.Comic.Update(comicEntity);

                // Update Releasing entity
                var releasingEntity = await _context.Releasing.FindAsync(releasing.releasingID);
                if (releasingEntity == null)
                    throw new Exception("Releasing record not found.");

                releasingEntity.isApprove = releasing.isApprove;
                releasingEntity.approveAt = releasing.approveAt;
                releasingEntity.userID = releasing.userID;
                releasingEntity.status = releasing.status;
                releasingEntity.licenseID = releasing.licenseID;

                _context.Releasing.Update(releasingEntity);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return new
                {
                    Success = true,
                    Message = "Releasing and Comic updated successfully."
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't update releasing: {ex.Message}");
            }
        }

        public async Task<dynamic> CreateReleasingAsync(ComicReleasingDTO releasing)
        {
            try
            {
                // Validate input
                if (releasing == null)
                    return new { Success = false, Message = "Invalid input data." };

                // Check if the Comic exists in the database
                var comicEntity = await _context.Comic!.FindAsync(releasing.ComicId);
                if (comicEntity == null)
                    return new { Success = false, Message = "Comic not found. Creation failed." };

                // Check if the Releasing entity exists
                var releasingEntity = await _context.Releasing!.FindAsync(releasing.releasingID);

                _logger.LogInformation($"Comic entity: {JsonConvert.SerializeObject(comicEntity)}");
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Releasing ON");

                if (releasingEntity == null)
                {
                    // Create new Releasing entity
                    releasingEntity = new Releasing
                    {
                        isApprove = releasing.isApprove,
                        approveAt = releasing.approveAt,
                        userID = releasing.userID,
                        status = releasing.status,
                        licenseID = releasing.licenseID
                    };
                    _logger.LogInformation($"Releasing entity: {JsonConvert.SerializeObject(releasingEntity)}");

                    await _context.Releasing.AddAsync(releasingEntity);
                }
                else
                {
                    // Update existing Releasing entity
                    releasingEntity.isApprove = releasing.isApprove;
                    releasingEntity.approveAt = releasing.approveAt;
                    releasingEntity.userID = releasing.userID;
                    releasingEntity.status = releasing.status;
                    releasingEntity.licenseID = releasing.licenseID;
                }

                // Associate the Releasing entity with the new Comic
                releasingEntity.releasingID = comicEntity.ComicId;

                // Save changes to the database
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Releasing OFF");

                return new
                {
                    Success = true,
                    Message = "Releasing updated and associated with the new Comic successfully.",
                    Data = new
                    {
                        ComicId = comicEntity.ComicId,
                        ReleasingId = releasingEntity.releasingID
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
