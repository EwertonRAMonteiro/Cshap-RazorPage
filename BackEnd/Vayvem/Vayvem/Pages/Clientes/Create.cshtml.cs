using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Vayvem.Pages.Clientes
{
    public class CreateModel : PageModel
    {
        public ClienteInfo clienteInfo = new ClienteInfo();

        public String errorMessage = "";
        public String successMesage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            clienteInfo.Nome = Request.Form["nome"];
            clienteInfo.Cpf = Request.Form["cpf"];
            clienteInfo.Endereco = Request.Form["endereco"];
            clienteInfo.Email = Request.Form["email"];
            clienteInfo.Telefone = Request.Form["telefone"];

            if (clienteInfo.Nome.Length == 0 || clienteInfo.Cpf.Length == 0 ||
                clienteInfo.Endereco.Length == 0 || clienteInfo.Email.Length == 0 ||
                clienteInfo.Telefone.Length ==0)
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
                    String sql = "INSERT INTO CLIENTE " +
                        "(nome, cpf, endereco, email, telefone) VALUES" +
                        "(@Nome, @Cpf, @Endereco, @Email, @Telefone);";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Nome", clienteInfo.Nome);
                        cmd.Parameters.AddWithValue("@Cpf", clienteInfo.Cpf);
                        cmd.Parameters.AddWithValue("@Endereco", clienteInfo.Endereco);
                        cmd.Parameters.AddWithValue("@Email", clienteInfo.Email);
                        cmd.Parameters.AddWithValue("@Telefone", clienteInfo.Telefone);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clienteInfo.Nome = ""; clienteInfo.Cpf = ""; clienteInfo.Endereco = ""; clienteInfo.Email = ""; clienteInfo.Telefone = "";
            successMesage = "Cliente cadastrado com Sucesso!";

            Response.Redirect("/Clientes/Index");
        }
    }
}
