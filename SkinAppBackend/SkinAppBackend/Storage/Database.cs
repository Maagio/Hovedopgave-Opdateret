using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SkinAppBackend.Storage
{
    public class Database : IDatabase
    {
        Services.Hashing hashing = new Services.Hashing();
        Services.DateSwapper dateSwapper = new Services.DateSwapper();

        public bool CreateUser(MyContext context, string email, string password)
        {
            bool canCreateUser = false;

            try
            {
                // Splits the hashed password which also has the salt, so both can be saved in the database
                string[] split = password.Split(":");
                string salt = split[0];
                string hashedPassword = split[1];

                Models.User user = new Models.User
                {
                    Email = email,
                    HashedPassword = hashedPassword,
                    PasswordSalt = salt,
                    CreationDate = DateTime.Now.Date
                };

                context.Users.Add(user);
                context.SaveChanges();

                canCreateUser = true;
            }
            catch (Exception e)
            {
                string error = e.Message;
            }
            return canCreateUser;
        }
        public bool ValidateUser(MyContext context, string email, string password, ref string errorMessage)
        {
            bool validUser = false;
            if (email != null)
            {
                int id = GetUserId(context, email);

                string hashedPassword = null;

                if (id != 0)
                {
                    string[] split = hashing.HashPassword(password, GetSalt(context, id)).Split(":");
                    hashedPassword = split[1];

                    int passwordCheck = context.Users
                        .Where(p => p.HashedPassword == hashedPassword && p.Email == email).Count();
                    if (passwordCheck > 0)
                        validUser = true;
                    else
                        errorMessage = "Incorrect password";
                }
                else
                    errorMessage = "Could not find a user with that email";
            }

            return validUser;
        }
        public int GetUserId(MyContext context, string email)
        {
            try
            {
                List<Models.User> user = context.Users
                    .Where(p => p.Email == email)
                    .Select(p => new Models.User { UserId = p.UserId }).ToList();

                return user[0].UserId;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public string GetEmail(MyContext context, int userId)
        {
            var user = context.Users
                .Where(p => p.UserId == userId)
                .Select(p => new Models.User { Email = p.Email }).ToList();

            return user[0].Email;
        }
        public string GetSalt(MyContext context, int userId)
        {
            var user = context.Users
                .Where(p => p.UserId == userId)
                .Select(p => new Models.User { PasswordSalt = p.PasswordSalt }).ToList();

            return user[0].PasswordSalt;
        }
        public Models.User GetUser(MyContext context, int userId)
        {
            var user = context.Users
                .Where(p => p.UserId == userId)
                .Select(p => new Models.User
                {
                    UserId = p.UserId,
                    Email = p.Email,
                    CreationDate = p.CreationDate,
                    HashedPassword = p.HashedPassword,
                    PasswordSalt = p.PasswordSalt
                }).ToList();

            return user[0];
        }
        public bool CreateSection(MyContext context, int userId, string sectionName)
        {
            bool created = false;

            var user = GetUser(context, userId);

            try
            {
                var section = new Models.Section
                {
                    Name = sectionName,
                    SectionCreationDate = DateTime.Now.Date,
                    UserId = user.UserId
                };

                context.Sections.Add(section);
                context.SaveChanges();

                created = true;
            }
            catch (Exception e)
            {
                string error = e.Message;
            }

            return created;
        }
        public List<Models.Section> GetSections(MyContext context, string userId)
        {
            var sections = context.Sections
                .Where(p => p.UserId == Convert.ToInt32(userId))
                .Select(p => new Models.Section
                {
                    SectionId = p.SectionId,
                    Name = p.Name,
                    SectionCreationDate = p.SectionCreationDate,
                    UserId = p.UserId
                }).ToList();

            return sections;
        }
        public string SavePicture(MyContext context, int sectionId, string imageString)
        {
            string message = "";
            try
            {
                var Picture = new Models.Picture
                {
                    ImageString = imageString,
                    PictureTakenDate = DateTime.Now.Date,
                    UserTaken = true,
                    SectionId = sectionId
                };
                context.Pictures.Add(Picture);
                context.SaveChanges();

                message = "Sucess";
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return message;
        }
        public List<Models.Picture> GetImages(MyContext context, int sectionId)
        {
            var images = context.Pictures
                .Where(p => p.SectionId == sectionId)
                .Select(p => new Models.Picture
                {
                    PictureId = p.PictureId,
                    ImageString = p.ImageString,
                    PictureTakenDate = p.PictureTakenDate,
                    //PictureTakenDate = dateSwapper.SwapDates(p.PictureTakenDate),
                    UserTaken = p.UserTaken
                }).ToList();

            return images;
        }
        public Boolean DeleteImage(MyContext context, int pictureId)
        {
            Boolean couldDelete = false;

            try
            {
                var toDelete = new Models.Picture { PictureId = pictureId };
                context.Pictures.Attach(toDelete);
                context.Pictures.Remove(toDelete);
                context.SaveChanges();

                couldDelete = true;
            }
            catch (Exception e)
            {
                string error = e.Message;
            }
            return couldDelete;
        }
        public void DeleteSection(MyContext context, int sectionId, ref Boolean couldDelete)
        {
            try
            {
                List<Models.Picture> images = GetImages(context, sectionId);

                foreach (Models.Picture image in images)
                {
                    DeleteImage(context, image.PictureId);
                }

                Models.Section toDelete = new Models.Section { SectionId = sectionId };
                context.Sections.Attach(toDelete);
                context.Sections.Remove(toDelete);
                context.SaveChanges();

                couldDelete = true;
            }
            catch (Exception e)
            {
                string error = e.Message;
            }
        }
        public Models.Picture GetImage(MyContext context, int pictureId)
        {
            var images = context.Pictures
                .Where(p => p.PictureId == pictureId)
                .Select(p => new Models.Picture
                {
                    PictureId = p.PictureId,
                    ImageString = p.ImageString,
                    PictureTakenDate = p.PictureTakenDate,
                    //PictureTakenDate = dateSwapper.SwapDates(p.PictureTakenDate),
                    UserTaken = p.UserTaken,
                    SectionId = p.SectionId
                }).ToList();

            return images[0];
        }
        public void ComparePictures(MyContext context, int pictureId1, int pictureId2, ref Boolean couldCompare)
        {
            try
            {
                Models.Picture[] pictures = new Models.Picture[2];
                pictures[0] = GetImage(context, pictureId1);
                pictures[1] = GetImage(context, pictureId2);

                System.IO.FileInfo fileInfo = new System.IO.FileInfo(@"C:\Users\Jesper\Documents\Datamatiker\4. Semester\Projekt\Pictures\Training\Dogs\dog2.jpg");
                byte[] data = new byte[fileInfo.Length];
                using (System.IO.FileStream fs = fileInfo.OpenRead())
                {
                    fs.Read(data, 0, data.Length);
                }

                string imageString = Convert.ToBase64String(data);

                var Picture = new Models.Picture
                {
                    ImageString = imageString,
                    PictureTakenDate = DateTime.Now.Date,
                    UserTaken = false,
                    SectionId = pictures[0].SectionId
                };
                context.Pictures.Add(Picture);
                context.SaveChanges();

                couldCompare = true;
            }
            catch (Exception e)
            {

            }
        }
    }
}
