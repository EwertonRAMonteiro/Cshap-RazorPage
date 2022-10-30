using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace Vayvem.Pages.Clientes
{
    public class EditModel : PageModel
    {
        public ClienteInfo clienteInfo = new ClienteInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String Id = Request.Query["id"];
            try
            {
                String connectionString = "Data Source=DESKTOP-NE164G1;Initial Catalog=Vayvem;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM  cliente WHERE id=@Id";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clienteInfo.Id = "" + reader.GetInt32(0);
                                clienteInfo.Nome = reader.GetString(1);
                                clienteInfo.Cpf = reader.GetString(2);
                                clienteInfo.Endereco = reader.GetString(3);
                                clienteInfo.Email = reader.GetString(4);
                                clienteInfo.Telefone = reader.GetString(5);

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
            clienteInfo.Id = Request.Form["Id"];
            clienteInfo.Nome = Request.Form["Nome"];
            clienteInfo.Cpf = Request.Form["Cpf"];
            clienteInfo.Endereco = Request.Form["Endereco"];
            clienteInfo.Email = Request.Form["Email"];
            clienteInfo.Telefone = Request.Form["Telefone"];

            if ( clienteInfo.Id.Length == 0 || clienteInfo.Nome.Length == 0 ||
                clienteInfo.Cpf.Length == 0 || clienteInfo.Endereco.Length == 0 ||
                clienteInfo.Email.Length == 0 || clienteInfo.Telefone.Length == 0 )
            {
                errorMessage = "Preencha todos os campos";
                return;
            }

            try
            {
                String connectionString = "Data Source=DESKTOP-NE164G1;Initial Catalog=Vayvem;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE CLIENTE " +
                        "SET nome=@Nome, cpf=@Cpf, endereco=@Endereco, email=@Email, telefone=@Telefone " +
                        "WHERE id=@Id";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", clienteInfo.Nome);
                        cmd.Parameters.AddWithValue("@Cpf", clienteInfo.Cpf);
                        cmd.Parameters.AddWithValue("@Endereco", clienteInfo.Endereco);
                        cmd.Parameters.AddWithValue("@Email", clienteInfo.Email);
                        cmd.Parameters.AddWithValue("@Telefone", clienteInfo.Telefone);
                        cmd.Parameters.AddWithValue("@Id", clienteInfo.Id);

                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Clientes/Index");
        }
    }
}
