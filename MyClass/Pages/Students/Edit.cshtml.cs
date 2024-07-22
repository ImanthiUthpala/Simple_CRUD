using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static MyClass.Pages.Students.IndexModel;

namespace MyClass.Pages.Students
{
    public class EditModel : PageModel
    {
        public StudentInfo studentInfo = new StudentInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=DESKTOP-J558N00;Initial Catalog=myclass;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                { 
                    connection.Open();
                    String sql = "SELECT * FROM students WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    { 
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                studentInfo.id = "" + reader.GetInt32(0);
                                studentInfo.name = reader.GetString(1);
                                studentInfo.email = reader.GetString(2);
                                studentInfo.phone = reader.GetString(3);
                                studentInfo.address = reader.GetString(4);
                            }
                        }
                    }
                }
            }

            catch (Exception ex) 
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost() 
        {
            studentInfo.id = Request.Form["id"];
            studentInfo.name = Request.Form["name"];
            studentInfo.email = Request.Form["email"];
            studentInfo.phone = Request.Form["phone"];
            studentInfo.address = Request.Form["address"];

			if (studentInfo.name.Length == 0 || studentInfo.email.Length == 0 ||
			   studentInfo.phone.Length == 0 || studentInfo.address.Length == 0)
			{
				errorMessage = "All the fields are required";
				return;
			}

            try
            {
                String connectionString = "Data Source=DESKTOP-J558N00;Initial Catalog=myclass;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                { 
                    connection.Open();
                    String sql = "UPDATE students " +
                                 "SET name=@name, email=@email, phone=@phone, address=@address " +
                                 "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", studentInfo.name);
                        command.Parameters.AddWithValue("@email", studentInfo.email);
                        command.Parameters.AddWithValue("@phone", studentInfo.phone);
                        command.Parameters.AddWithValue("@address", studentInfo.address);
                        command.Parameters.AddWithValue("@id", studentInfo.id);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Students/Index");
		}
    }
}
