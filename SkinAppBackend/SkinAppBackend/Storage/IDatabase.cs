using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinAppBackend.Storage
{
    public interface IDatabase
    {
        Boolean CreateUser(MyContext context, string email, string password);
        Boolean ValidateUser(MyContext context, string email, string password, ref string errorMessage);
        int GetUserId(MyContext context, string email);
        string GetEmail(MyContext context, int userId);
        string GetSalt(MyContext context, int userId);
        Models.User GetUser(MyContext context, int userId);
        Boolean CreateSection(MyContext context, int userId, string sectionName);
        List<Models.Section> GetSections(MyContext context, string userId);
        string SavePicture(MyContext context, int sectionId, string imageString);
        List<Models.Picture> GetImages(MyContext context, int sectionId);
        Boolean DeleteImage(MyContext context, int pictureId);
        void DeleteSection(MyContext context, int sectionId, ref Boolean couldDelete);
        Models.Picture GetImage(MyContext context, int pictureId);
        void ComparePictures(MyContext context, int pictureId1, int pictureId2, ref Boolean couldCompare);
    }
}
