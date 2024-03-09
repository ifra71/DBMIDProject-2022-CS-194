using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB1
{
    public partial class RubricTest : Form
    {
        string ID_UpDate = "";
        public RubricTest()
        {
            InitializeComponent();
            load_data_in_data_gride_view();
        }
        //void Fillcombobox()
        //{
        //    var con = Configuration.getInstance().getConnection(); ;
        //    SqlCommand cmd = new SqlCommand("select ID From Clo", con);
        //    SqlDataReader Sdr = cmd.ExecuteReader();
        //    while (Sdr.Read())
        //    {
        //        comboBox1.Items.Add(Sdr["ID"].ToString());
        //    }
        //    Sdr.Close();
        //}

        void Fillcombobox()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                con.Open(); // Open the connection

                SqlCommand cmd = new SqlCommand("select ID From Clo", con);
                SqlDataReader Sdr = cmd.ExecuteReader();
                while (Sdr.Read())
                {
                    comboBox1.Items.Add(Sdr["ID"].ToString());
                }
                Sdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void RubricTest_Load(object sender, EventArgs e)
        {

            Fillcombobox();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }


        //private int get_id()
        //{
        //    var con = Configuration.getInstance().getConnection();
        //    SqlCommand cmd1 = new SqlCommand("Select max(ID) from  Rubric  ", con);
        //    cmd1.InitializeLifetimeService();
        //    decimal maxTotalAmount = Convert.ToDecimal(cmd1.ExecuteScalar());
        //    int id = int.Parse(maxTotalAmount.ToString());
        //    id = id + 1;
        //    return id;
        //}

        //private int get_id()
        //{
        //    int id = 0;
        //    SqlConnection con = null; // Declare connection variable outside try block
        //    try
        //    {
        //        con = Configuration.getInstance().getConnection();
        //        con.Open(); // Open the connection

        //        SqlCommand cmd1 = new SqlCommand("Select max(ID) from  Rubric", con);
        //        decimal maxTotalAmount = Convert.ToDecimal(cmd1.ExecuteScalar());
        //        id = Convert.ToInt32(maxTotalAmount) + 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("An error occurred: " + ex.Message);
        //    }
        //    finally
        //    {
        //        if (con != null && con.State == ConnectionState.Open)
        //        {
        //            con.Close(); // Close the connection if it's open
        //        }
        //    }
        //    return id;
        //}

        private int get_id()
        {
            int id = 0;
            SqlConnection con = null;
            try
            {
                con = Configuration.getInstance().getConnection();
                con.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT MAX(ID) FROM Rubric", con);
                object result = cmd1.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    id = Convert.ToInt32(result) + 1;
                }
                else
                {
                    // Handle case when there are no records in the Rubric table
                    id = 1; // Set id to 1 as the initial value
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return id;
        }


        public void load_data_in_data_gride_view()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Rubric  ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex.Equals(1) && e.RowIndex != -1)
            {
                if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.Value != null)
                {
                    DataGridViewRow row = dataGridView1.CurrentCell.OwningRow;
                    string value = row.Cells["Id"].Value.ToString();
                    MessageBox.Show(value);
                    {
                        var con = Configuration.getInstance().getConnection();
                        SqlCommand cmd = new SqlCommand("DELETE FROM Rubric  WHERE Id  = @Id", con);
                        cmd.Parameters.AddWithValue("@Id", value);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully DELETED");
                        load_data_in_data_gride_view();
                    }
                }
            }
            else if (dataGridView1.CurrentCell.ColumnIndex.Equals(0) && e.RowIndex != -1)
            {
                if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.Value != null)
                {
                    DataGridViewRow row = dataGridView1.CurrentCell.OwningRow;
                    ID_UpDate = row.Cells["Id"].Value.ToString();
                    MessageBox.Show(ID_UpDate);
                    MessageBox.Show("Edit Colume ");
                    this.textBox2.Text = row.Cells["Details"].Value.ToString();
                }
            }
        }









        //private void button1_Click(object sender, EventArgs e)
        //{

        //    if (this.textBox2.Text != "" && this.comboBox1.SelectedIndex > -1)
        //    {
        //        int ID = this.get_id();
        //        var con = Configuration.getInstance().getConnection();
        //        SqlCommand cmd = new SqlCommand("INSERT INTO Rubric (Details ,CloId , ID ) values(@Details, @CloId , @ID)", con);
        //        cmd.Parameters.AddWithValue("@Details", this.textBox2.Text);
        //        cmd.Parameters.AddWithValue("@CloId", comboBox1.Text);
        //        cmd.Parameters.AddWithValue("@ID", ID);
        //        cmd.ExecuteNonQuery();
        //        MessageBox.Show("Successfully saved");
        //        load_data_in_data_gride_view();
        //        this.textBox2.Text = "";
        //        this.comboBox1.Text = "CLOS ID";
        //    }
        //    else
        //    {
        //        MessageBox.Show("Something is Messing ");
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox2.Text != "" && this.comboBox1.SelectedIndex > -1)
            {
                int ID = this.get_id();
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open(); // Open the connection before executing the query

                    SqlCommand cmd = new SqlCommand("INSERT INTO Rubric (Details, CloId, ID) VALUES (@Details, @CloId, @ID)", con);
                    cmd.Parameters.AddWithValue("@Details", this.textBox2.Text);
                    cmd.Parameters.AddWithValue("@CloId", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@ID", ID);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully saved");

                    load_data_in_data_gride_view();
                    this.textBox2.Text = "";
                    this.comboBox1.SelectedIndex = -1; // Reset ComboBox selection
                }
            }
            else
            {
                MessageBox.Show("Something is missing.");
            }
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Please provide the ID.");
            }
            else
            {
                int id;
                if (!int.TryParse(textBox3.Text, out id))
                {
                    MessageBox.Show("Invalid ID. Please enter a valid integer.");
                    return;
                }

                try
                {
                    var con = Configuration.getInstance().getConnection();
                    con.Open(); // Open the connection

                    SqlCommand cmd = new SqlCommand("UPDATE Rubric SET Details=@details WHERE Id = @id", con);

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@details", textBox2.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Record successfully updated.");
                    this.Refresh();
                    data(); // Assuming this method updates your data grid or UI with the updated data

                    con.Close(); // Close the connection after use
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }


        }
        private void button2_Click(object sender, EventArgs e)
        {

            try
            {

                if (string.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("Please provide an ID to be deleted.");
                }
                else
                {

                    using (var con = Configuration.getInstance().getConnection())
                    {

                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Rubric WHERE Id=@id", con))
                        {
                            cmd.Parameters.AddWithValue("@id", int.Parse(textBox3.Text));


                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Record deleted successfully.");
                                this.Refresh();
                                data();
                            }
                            else
                            {
                                MessageBox.Show("No record found with the provided ID.");
                            }
                        }
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please provide a valid ID (numeric value).");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }
        public void data()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Rubric", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dataGridView1.DataSource = dta;
        }
        private void button6_Click(object sender, EventArgs e)
        {


            String connection = @"Data Source=IFRA-FAZAL;Initial Catalog=ProjectB;Integrated Security=True";
            string query = "Select * from Rubric";
            PDF(connection, query);
            MessageBox.Show("Pdf Successfully Created!!!");
        }

        public static void PDF(string connectionString, string query)
        {
            try
            {
                // Connect to the database and retrieve data
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        // Create a new PDF document and add a page
                        using (Document document = new Document())
                        {
                            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(@"C:\Users\ifraf\Desktop\4th Sem\DATABASE_LABS\DB1\Rubrics.pdf", FileMode.Create));

                         //   PdfWriter.GetInstance(document, new FileStream("Rubric.pdf", FileMode.Create));
                            document.Open();

                            // Add a table to the PDF document
                            PdfPTable table = new PdfPTable(reader.FieldCount);
                            table.WidthPercentage = 100;

                            // Add headers to the table
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(reader.GetName(i)));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                table.AddCell(cell);
                            }

                            // Add rows to the table
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    table.AddCell(reader[i].ToString());
                                }
                            }

                            // Add the table to the document
                            document.Add(table);
                        }

                        // Close the reader
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
