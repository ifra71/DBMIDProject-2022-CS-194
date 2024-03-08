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
    public partial class Rubrics : Form
    {
        public Rubrics()
        {
            InitializeComponent();
            this.Refresh();
            CloIDLoad();
            RubricIdLoad();
            data();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            LOGIN form = new LOGIN();
            form.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form = new Form1();
            form.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String ConnectionStr = @"Data Source=IFRA-FAZAL;Initial Catalog=ProjectB;Integrated Security=True";

            int pid = 0;
            using (SqlConnection con = new SqlConnection(ConnectionStr))
            {
                using (SqlCommand cmd = new SqlCommand("Select ISNULL(MAX(ID), 1) AS ID FROM Rubric", con))
                {
                    con.Open();
                    pid = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                    textBox1.Text = pid.ToString();

                    con.Close();
                }
            }
            try
            {
                if (textBox3.Text == "")
                {
                    MessageBox.Show("ENTER DETAILS");

                }
                else
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("Insert into Rubric values (@id,@Details,@CloId)", con);

                    cmd.Parameters.AddWithValue("@id", int.Parse(textBox2.Text));
                    cmd.Parameters.AddWithValue("@Details", textBox3.Text);
                    cmd.Parameters.AddWithValue("@CloId", int.Parse(comboBox1.Text));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("ADDED");
                    this.Refresh();
                    data();
                }
            }
            catch
            {
                MessageBox.Show("ERROR.CHECK THE INFO AGAIN!");
            }
            
        }
        public void data()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Student", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dataGridView1.DataSource = dta;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable rubricData = GetRubricData();
                if (rubricData != null && rubricData.Rows.Count > 0)
                {
                    dataGridView1.DataSource = rubricData;
                }
                else
                {
                    MessageBox.Show("No rubric data found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving rubric data: " + ex.Message);
            }
        }


        private DataTable GetRubricData()
        {
            DataTable rubricDataTable = new DataTable();
            using (SqlConnection connection = Configuration.getInstance().getConnection())
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Rubric", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(rubricDataTable);
            }
            return rubricDataTable;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    // Mark the current row as selected
                    dataGridView1.CurrentRow.Selected = true;

                    // Retrieve and display the values of selected row cells
                    textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["Id"].FormattedValue.ToString();
                    textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["Details"].FormattedValue.ToString();
                    textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["CloId"].FormattedValue.ToString();
                }
                else
                {
                    MessageBox.Show("No data exists in the selected cell.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while selecting the row: " + ex.Message);
            }

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
                            PdfWriter.GetInstance(document, new FileStream("Rubric.pdf", FileMode.Create));
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("PROVIDE THE ID DEAR");
            }
            else
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("UPDATE Rubric SET Details=@details WHERE Id = @id", con);

                cmd.Parameters.AddWithValue("@id", int.Parse(textBox1.Text));
                cmd.Parameters.AddWithValue("@details", textBox3.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("RECORD SUCCESSFULLY UPDATED");
                this.Refresh();
                data();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if ID is provided
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Please provide an ID to be deleted.");
                }
                else
                {
                    // Get database connection
                    using (var con = Configuration.getInstance().getConnection())
                    {
                        // Create SQL command to delete record
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Rubric WHERE Id=@id", con))
                        {
                            // Add parameter for ID
                            cmd.Parameters.AddWithValue("@id", int.Parse(textBox1.Text));

                            // Open connection and execute delete command
                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Record deleted successfully.");
                                this.Refresh(); // Refresh UI
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
        private void RubricIdLoad()
        {
            string constr = @"Data Source=IFRA-FAZAL;Initial Catalog=ProjectB;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter("SELECT Id FROM Rubric", con))
                {
                    //Fill the DataTable with records from Table.
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    //Insert the Default Item to DataTable.
                    DataRow row = dt.NewRow();
                    //row[0] = 0;
                    //row[0] = "Please select";
                    dt.Rows.InsertAt(row, 0);

                    //Assign DataTable as DataSource.
                    comboBox2.DataSource = dt;
                    comboBox2.DisplayMember = "Id";
                    comboBox2.ValueMember = "Id";
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CloIDLoad()
        {
            string constr = @"Data Source=IFRA-FAZAL;Initial Catalog=ProjectB;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter("SELECT Id FROM Clo", con))
                {
                    //Fill the DataTable with records from Table.
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    //Insert the Default Item to DataTable.
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    //Assign DataTable as DataSource.
                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "Id";
                    comboBox1.ValueMember = "Id";
                }
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}

