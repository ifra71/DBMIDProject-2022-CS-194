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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DB1
{
    public partial class StudentResult : Form
    {
        public StudentResult()
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var connection = Configuration.getInstance().getConnection())
                {
                    string query = "INSERT INTO StudentResult (StudentId, AssessmentComponentId, RubricMeasurementId, EvaluationDate) " +
                                   "VALUES (@StudentId, @AssessmentComponentId, @RubricMeasurementId, @EvaluationDate)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentId", comboBox3.SelectedValue);
                        command.Parameters.AddWithValue("@AssessmentComponentId", comboBox2.SelectedValue);
                        command.Parameters.AddWithValue("@RubricMeasurementId", comboBox1.SelectedValue);
                        command.Parameters.AddWithValue("@EvaluationDate", dateTimePicker1.Value);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Successfully saved");
                            this.Refresh();
                            data();
                        }
                        else
                        {
                            MessageBox.Show("Failed to save data");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }

        }
        public void data()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from StudentResult", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dataGridView1.DataSource = dta;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Get data from the database
                DataTable dt = GetDataFromDatabase();

                // Create a new PDF document
                using (Document doc = new Document())
                {
                    PdfWriter.GetInstance(doc, new FileStream("StudentResultReport.pdf", FileMode.Create));
                    doc.Open();

                    // Add a table with headers
                    PdfPTable table = new PdfPTable(5);
                    AddTableHeaders(table);

                    // Add data rows to the table
                    foreach (DataRow row in dt.Rows)
                    {
                        int studentID = Convert.ToInt32(row["StudentID"]);
                        int component = Convert.ToInt32(row["AssessmentComponentId"]);
                        int rubricLevel = Convert.ToInt32(row["RubricMeasurementId"]);
                        DateTime evalDate = Convert.ToDateTime(row["EvaluationDate"]);
                        double maxRubricLevel = GetMaxRubricLevel(component);
                        double obtainedMarks = CalculateObtainedMarks(rubricLevel, maxRubricLevel, component);
                        AddRowToTable(table, studentID, component, rubricLevel, evalDate, obtainedMarks);
                    }

                    // Add the table to the document
                    doc.Add(table);
                }

                MessageBox.Show("PDF report generated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}");
            }
        }

        private DataTable GetDataFromDatabase()
        {
            using (var con = Configuration.getInstance().getConnection())
            {
                string query = "SELECT StudentID, AssessmentComponentId, RubricMeasurementId, EvaluationDate FROM StudentResult";

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        private void AddTableHeaders(PdfPTable table)
        {
            table.AddCell("Student ID");
            table.AddCell("Assessment Component");
            table.AddCell("Rubric Level");
            table.AddCell("Evaluation Date");
            table.AddCell("Obtained Marks");
        }

        private double CalculateObtainedMarks(int rubricLevel, double maxRubricLevel, int component)
        {
            double obtainedMarks = (rubricLevel / maxRubricLevel) * GetComponentMarks(component);
            return obtainedMarks;
        }

        private void AddRowToTable(PdfPTable table, int studentID, int component, int rubricLevel, DateTime evalDate, double obtainedMarks)
        {
            table.AddCell(studentID.ToString());
            table.AddCell(component.ToString());
            table.AddCell(rubricLevel.ToString());
            table.AddCell(evalDate.ToString());
            table.AddCell(obtainedMarks.ToString());
        }

        private double GetMaxRubricLevel(int com)
        {
            var Con = Configuration.getInstance().getConnection();
            //string query = $"SELECT MAX(RML.RubricMeasurementId) FROM StudentResult SR JOIN RubricLevel RML ON SR.RubricMeasurementId = RML.Id WHERE SR.AssessmentComponentId = {component}";
            string query = $"SELECT MAX(RML.MeasurementLevel) FROM StudentResult SR JOIN RubricLevel RML ON SR.RubricMeasurementId = RML.Id";

            SqlCommand command = new SqlCommand(query, Con);

            object result = command.ExecuteScalar();
            if (result != null && double.TryParse(result.ToString(), out double maxLevel))
            {
                MessageBox.Show("Max Rubric:" + result);
                return maxLevel;
            }
            else
            {
                throw new ArgumentException($"No data found for component:");
            }


        }

        private double GetComponentMarks(int component)
        {
            var Con = Configuration.getInstance().getConnection();
            string query = $"SELECT TotalMarks FROM AssessmentComponent WHERE Id = {component}";

            SqlCommand command = new SqlCommand(query, Con);

            object result = command.ExecuteScalar();
            if (result != null && double.TryParse(result.ToString(), out double marks))
            {
                return marks;
            }
            else
            {
                throw new ArgumentException($"Invalid component: {component}");
            }

        }
        private double GetObtainedMarks(int studentId, int component)
        {
            var Con = Configuration.getInstance().getConnection();
            double maxLevel = GetMaxRubricLevel(component);
            double componentMarks = GetComponentMarks(component);
            string query = $"SELECT RubricMeasurementId FROM StudentResult WHERE StudentID = {studentId} AND AssessmentComponentId = {component}";

            SqlCommand command = new SqlCommand(query, Con);

            SqlDataReader reader = command.ExecuteReader();
            double obtainedLevel = 0;
            while (reader.Read())
            {
                int rubricLevelId = reader.GetInt32(0);
                double measurementLevel = GetMeasurementLevel(rubricLevelId);
                obtainedLevel += measurementLevel;
            }
            reader.Close();
            double obtainedMarks = (obtainedLevel / maxLevel) * componentMarks;
            return obtainedMarks;

        }

        private double GetMeasurementLevel(int rubricLevelId)
        {
            var Con = Configuration.getInstance().getConnection();

            string query = $"SELECT MeasurementLevel FROM RubricLevel WHERE Id = {rubricLevelId}";

            SqlCommand command = new SqlCommand(query, Con);

            object result = command.ExecuteScalar();
            if (result != null && double.TryParse(result.ToString(), out double measurementLevel))
            {
                return measurementLevel;
            }
            else
            {
                throw new ArgumentException($"Invalid rubric level: {rubricLevelId}");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            String connection = @"Data Source=IFRA-FAZAL;Initial Catalog=ProjectB;Integrated Security=True";
            string query = "Select * from StudentResult";
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
                    PdfWriter.GetInstance(document, new FileStream("StudentResult.pdf", FileMode.Create));
                    document.Open();

                    // Add a table to the PDF document
                    PdfPTable table = new PdfPTable(reader.FieldCount);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 2f, 2f, 2f, 2f });

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

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("No ID provided for editing.");
            }
            else
            {
                try
                {
                    using (var connection = Configuration.getInstance().getConnection())
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE StudentResult SET RubricMeasurementId = @RubricMeasurementId, AssessmentComponentId = @AssessmentComponentId, EvaluationDate = @EvaluationDate WHERE StudentId = @StudentId", connection);

                        cmd.Parameters.AddWithValue("@StudentId", comboBox3.SelectedValue);
                        cmd.Parameters.AddWithValue("@AssessmentComponentId", comboBox2.SelectedValue);
                        cmd.Parameters.AddWithValue("@RubricMeasurementId", comboBox1.SelectedValue);
                        cmd.Parameters.AddWithValue("@EvaluationDate", dateTimePicker1.Value);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record has been successfully edited.");
                            this.Refresh();
                            data();
                        }
                        else
                        {
                            MessageBox.Show("No record found for the provided ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("No Student ID provided to be deleted.");
                }
                else
                {
                    using (var connection = Configuration.getInstance().getConnection())
                    {
                        SqlCommand cmd = new SqlCommand("DELETE FROM StudentResult WHERE StudentId = @StudentId", connection);
                        cmd.Parameters.AddWithValue("@StudentId", textBox1.Text);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully.");
                            this.Refresh();
                            data(); 
                        }
                        else
                        {
                            MessageBox.Show("No records found for the provided Student ID.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

