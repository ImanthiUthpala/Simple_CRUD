using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyClass.Pages.Students
{
    public class IndexModel : PageModel
    {
        public List<StudentInfo> listStudents = new List<StudentInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DESKTOP-J558N00;Initial Catalog=myclass;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    //Read data from the students
                    String sql = "SELECT * FROM students";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StudentInfo studentInfo = new StudentInfo();

                                // id is given as String but in db it is in Int therefore an empty string is added to the front
                                studentInfo.id = "" + reader.GetInt32(0); 

                                studentInfo.name = reader.GetString(1);
                                studentInfo.email = reader.GetString(2);
                                studentInfo.phone = reader.GetString(3);
                                studentInfo.address = reader.GetString(4);
                                studentInfo.created_at = reader.GetDateTime(5).ToString();

                                //Add Student to the list
                                listStudents.Add(studentInfo);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        public class StudentInfo
        {
            public String id;
            public String name;
            public String email;
            public String phone;
            public String address;
            public String created_at;
        }
    }
}
