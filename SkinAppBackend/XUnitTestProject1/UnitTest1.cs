using Xunit;
using SkinAppBackend;
using System.Data.SqlClient;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("test@email.dk", true)]
        [InlineData("hej", false)]
        [InlineData(null, false)]
        [InlineData(" ", false)]
        public void TestEmailValidator(string email, bool expectedResult)
        {
            SkinAppBackend.Services.EmailValidator emailValidator = new SkinAppBackend.Services.EmailValidator();

            bool result = emailValidator.Validate(email);

            Assert.Equal(result, expectedResult);

        }

        //[Theory]
        //[InlineData(5, "jesper@gmail.com")]
        //public void TestEFMethod(int userId, string email)
        //{
        //    //SkinAppBackend.Storage.MyContext context ;
        //    SkinAppBackend.Storage.Database db = new SkinAppBackend.Storage.Database();

        //    string result = db.GetEmail(_context, userId);

        //    Assert.Equal(result, email);
        //}

        //[Fact]
        //public void DBIntegrationTest()
        //{
        //    SqlConnection conn = new SqlConnection("Server = localhost;Database = HovedOpgave; Integrated Security = True");
        //    string statement = "insert into Users values(@Email, @HashedPassword, @PasswordSalt, @CreationDate)";
        //    SqlCommand cmd = new SqlCommand(statement, conn);
        //    cmd.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 30).Value = "cad@email.dk";
        //    cmd.Parameters.Add("@HashedPassword", System.Data.SqlDbType.VarChar, 50).Value = "test";
        //    cmd.Parameters.Add("@PasswordSalt", System.Data.SqlDbType.VarChar, 100).Value = "saltedTest";
        //    cmd.Parameters.Add("@CreationDate", System.Data.SqlDbType.Date).Value = System.DateTime.Now;

        //    cmd.ExecuteNonQuery();

        //}
    }
}
