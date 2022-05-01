using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace GestionZoo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["GestionZoo.Properties.Settings.CursoDBConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);

            MuestraZoos();
            MuestraAnimales();
            
        }

        private void MuestraZoos()
        {
            try
            {
                //string consulta = ; //Creamos una consulta que busca todo lo que hay dentro de Zoo.

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select Id, Ubicacion from Zoo", sqlConnection); //Adaptamos la conulta y la envia a la conexion.
                DataTable zooTabla = new DataTable(); //Almacena datos de tablas en un objeto.
                sqlDataAdapter.Fill(zooTabla);

                using (sqlDataAdapter) //Usamos el adaptador para
                {   
                    lbox_1.SelectedValuePath = zooTabla.Columns[0].ToString(); //Valor para identificar cada ubicacion.
                    lbox_1.DisplayMemberPath = zooTabla.Columns[1].ToString(); //Muestra todos los elementos de la tabla ubicacion.
                    lbox_1.ItemsSource = zooTabla.DefaultView; //Metodo que le da formato a la tabla.
                    
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(),"Muestra zoo");
            }

        }

        private void MuestraAnimales()
        {
            try
            {
                string consulta = "select Id, Nombre from Animal"; //Creamos una consulta que busca todo lo que hay dentro de Zoo.

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, sqlConnection); //Adaptamos la conulta y la envia a la conexion.
                DataTable AnimalesTabla = new DataTable(); //Almacena datos de tablas en un objeto.
                sqlDataAdapter.Fill(AnimalesTabla);

                using (sqlDataAdapter) //Usamos el adaptador para
                {
                    lbox_3.SelectedValuePath = AnimalesTabla.Columns[0].ToString(); //Valor para identificar cada ubicacion.
                    lbox_3.DisplayMemberPath = AnimalesTabla.Columns[1].ToString(); //Muestra todos los elementos de la tabla ubicacion.
                    lbox_3.ItemsSource = AnimalesTabla.DefaultView; //Metodo que le da formato a la tabla.

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(),"Muestra animales");
            }

        }

        private void MuestraAnimalesAsociados()
        {
            try
            {
                //ZooId funciona como una variable donde con cmd parameters le mandamos el valor a la consulta para que dependiendo lo que seleccione el usuario se le va a mandar.
                string consulta = "select * from Animal Inner Join AnimalZoo on Animal.Id = AnimalZoo.AnimalId where AnimalZoo.ZooId = @ZooId"; //Creamos una consulta que busca todos los animales que esten asociados a un zoologico.

                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); //Adaptamos la conulta y la envia a la conexion.

                using (sqlDataAdapter) //Usamos el adaptador para
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", lbox_1.SelectedValue);

                    DataTable AnimalTabla = new DataTable(); //Almacena datos de tablas en un objeto.
                    sqlDataAdapter.Fill(AnimalTabla);

                    lbox_2.DisplayMemberPath = "Nombre"; //Muestra todos los elementos de la tabla ubicacion.
                    lbox_2.SelectedValue = "Id"; //Valor para identificar cada ubicacion.
                    lbox_2.ItemsSource = AnimalTabla.DefaultView; //Metodo que le da formato a la tabla.
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(),"Muestra asociacion");
            }

        }  

        private void lbox_1_SelectionChanged(object sender, SelectionChangedEventArgs e) //Creamos un evento para que detecte cuando se cambia de ciudad.
        {
            MuestraAnimalesAsociados();
        }

        private void btn_eliminarZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "delete from Zoo where Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", lbox_1.SelectedValue);

                sqlCommand.ExecuteScalar();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                MuestraZoos();
            }
        }

        
    }
}
