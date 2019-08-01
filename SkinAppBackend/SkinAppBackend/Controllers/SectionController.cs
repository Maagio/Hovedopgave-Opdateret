using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkinAppBackend.Storage;
using SkinAppBackend.Services;

namespace SkinAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly MyContext _context;
        Database db = new Database();
        Hashing hashing = new Hashing();
        EmailValidator validateEmail = new EmailValidator();

        public SectionController(MyContext context)
        {
            _context = context;
        }
        [HttpPost("[action]")]
        public bool CreateSection([FromBody] string[] userData)
        {
            bool success = false;
            int userId = Convert.ToInt32(userData[0]);
            try
            {
                //string email = db.GetEmail(_context, userId);
                db.CreateSection(_context, userId, userData[1]);
                success = true;
            }
            catch (Exception e)
            {
                string error = e.Message;
            }
            return success;
        }
        [HttpPost("[action]")]
        public List<Models.Section> GetSections([FromBody] string[] userData)
        {
            List<Models.Section> sections = db.GetSections(_context, userData[0]);

            return sections;
        }
        [HttpPost("[action]")]
        public Boolean DeleteSection([FromBody] string[] data)
        {
            Boolean couldDelete = false;

            db.DeleteSection(_context, Convert.ToInt32(data[0]), ref couldDelete);

            return couldDelete;
        }
        [HttpPost("[action]")]
        public Boolean SaveImage([FromBody] string[] data)
        {
            Boolean couldSave = false;
            string message = "";
            message = db.SavePicture(_context, Convert.ToInt32(data[0]), data[1]);
            if (message == "Sucess")
                couldSave = true;
            return couldSave;
        }
        [HttpPost("[action]")]
        public List<Models.Picture> GetImages([FromBody] string[] data)
        {
            List<Models.Picture> images = db.GetImages(_context, Convert.ToInt32(data[0]));

            return images;
        }
        [HttpPost("[action]")]
        public Boolean DeleteImage([FromBody] string[] data)
        {
            Boolean couldDelete = false;

            couldDelete = db.DeleteImage(_context, Convert.ToInt32(data[0]));

            return couldDelete;
        }
        [HttpPost("[action]")]
        public Boolean ComparePictures([FromBody] string[] data)
        {
            Boolean couldCompared = false;

            db.ComparePictures(_context, Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), ref couldCompared);

            return couldCompared;
        }
    }
}