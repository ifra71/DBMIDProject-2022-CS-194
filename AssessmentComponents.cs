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
    public partial class AssessmentComponents : Form
    {
        public AssessmentComponents()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Please enter all details.");
                }
                else
                {
                    
                    using (var con = Configuration.getInstance().getConnection())
                    {
                       
                        string insertQuery = "INSERT INTO AssessmentComponent (Name, RubricId, TotalMarks, DateCreated, DateUpdated, AssessmentId) " +
                                             "VALUES (@Name, @RubricId, @TotalMarks, @DateCreated, @DateUpdated, @AssessmentId)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                           
                            cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                            if (int.TryParse(comboBox1.Text, out int rubricId))
                            {
                                cmd.Parameters.AddWithValue("@RubricId", rubricId);
                            }
                            else
                            {
                                MessageBox.Show("Invalid value for RubricId. Please select a valid integer from the dropdown.");
                                return;
                            }

                            if (int.TryParse(textBox2.Text, out int totalMarks))
                            {
                                cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
                            }
                            else
                            {
                                MessageBox.Show("Invalid value for TotalMarks. Please enter a valid integer.");
                                return;
                            }

                            if (int.TryParse(comboBox2.Text, out int assessmentId))
                            {
                                cmd.Parameters.AddWithValue("@AssessmentId", assessmentId);
                            }
                            else
                            {
                                MessageBox.Show("Invalid value for AssessmentId. Please select a valid integer from the dropdown.");
                                return;
                            }

                            cmd.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);
                            cmd.Parameters.AddWithValue("@DateUpdated", dateTimePicker2.Value);

                       
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully saved");

                            this.Refresh();
                            data();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }
        private void data()
        {
            String ConnectionStr = @"Data Source=IFRA-FAZAL;Initial Catalog=ProjectB;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(ConnectionStr))
            {
                using (SqlCommand cmd = new SqlCommand("Select * from AssessmentComponent", con))
                {

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }
        private void AssessmentComponents_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Please provide a name to be deleted.");
                }
                else
                {
                    
                    using (var con = Configuration.getInstance().getConnection())
                    {
                      
                        string deleteQuery = "DELETE FROM AssessmentComponent WHERE Name = @Name";
                        using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                        {
                            
                            cmd.Parameters.AddWithValue("@Name", textBox1.Text);

                            // Execute SQL command
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Record has been deleted successfully.");
                                // Refresh UI and update data
                                this.Refresh();
                                data();
                            }
                            else
                            {
                                MessageBox.Show("No record found with the provided name.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if Name field is provided
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Please provide a name to be edited.");
                }
                else
                {
                    // Get a database connection
                    using (var con = Configuration.getInstance().getConnection())
                    {
                        // Prepare SQL command for updating record
                        string updateQuery = "UPDATE AssessmentComponent SET TotalMarks=@TotalMarks, RubricId=@RubricId, AssessmentId=@AssessmentId, DateCreated=@DateCreated, DateUpdated=@DateUpdated WHERE Name= @Name";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            // Set parameters for the SQL command
                            cmd.Parameters.AddWithValue("@Name", textBox1.Text);

                            // Parse and set RubricId, handle parse errors gracefully
                            if (int.TryParse(comboBox1.Text, out int rubricId))
                            {
                                cmd.Parameters.AddWithValue("@RubricId", rubricId);
                            }
                            else
                            {
                                MessageBox.Show("Invalid value for RubricId. Please select a valid integer from the dropdown.");
                                return;
                            }

                            // Parse and set TotalMarks, handle parse errors gracefully
                            if (int.TryParse(textBox2.Text, out int totalMarks))
                            {
                                cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
                            }
                            else
                            {
                                MessageBox.Show("Invalid value for TotalMarks. Please enter a valid integer.");
                                return;
                            }

                            // Parse and set AssessmentId, handle parse errors gracefully
                            if (int.TryParse(comboBox2.Text, out int assessmentId))
                            {
                                cmd.Parameters.AddWithValue("@AssessmentId", assessmentId);
                            }
                            else
                            {
                                MessageBox.Show("Invalid value for AssessmentId. Please select a valid integer from the dropdown.");
                                return;
                            }

                            // Set DateCreated and DateUpdated parameters
                            cmd.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);
                            cmd.Parameters.AddWithValue("@DateUpdated", dateTimePicker2.Value);

                            // Execute SQL command
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Record has been successfully edited.");

                            // Refresh UI and update data
                            this.Refresh();
                            data();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {

            String connection = @"Data Source=IFRA-FAZAL;Initial Catalog=ProjectB;Integrated Security=True";
            string query = "Select * from AssessmentComponent";
            PDF(connection, query);
            MessageBox.Show("Pdf Successfully Created!!!");
        }
        public static void PDF(string connectionString, string query)
        {
            // Connect to the database and retrieve data
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Create a new PDF document and add a page
                    Document document = new Document();
                    PdfWriter.GetInstance(document, new FileStream("AssessmentComponent.pdf", FileMode.Create));
                    document.Open();

                    // Add a table to the PDF document
                    PdfPTable table = new PdfPTable(reader.FieldCount);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 2f });

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

                    // Close the document and the reader
                    document.Close();
                    reader.Close();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
