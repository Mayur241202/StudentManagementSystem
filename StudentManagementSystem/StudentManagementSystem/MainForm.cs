// MainForm.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace StudentManagementSystem
{
    public partial class MainForm : Form
    {
        // Data-holding variables (sName, sDOB, etc.)
        string sName, sGender, sAddress, sContact, sEmail, sCourse, sPhotoPath;
        DateTime sDOB, sAdmissionDate;

        decimal totalFees, feesPaid, pendingFees, scholarship, fine;

        public MainForm()
        {
            InitializeComponent();

            // when the form first loads, size to fit
            this.Load += (s, e) => AdjustFormHeight();

            // and every time the user switches tabs, re‑fit:
            tabControl.SelectedIndexChanged += (s, e) => AdjustFormHeight();
        }

        private bool ValidateStudentTab()
        {
            var errors = new List<string>();

            // Full Name
            if (string.IsNullOrWhiteSpace(txtName.Text) || !Regex.IsMatch(txtName.Text, @"^[A-Za-z\s]+$"))
                errors.Add("Full Name must contain only alphabets and spaces.");

            // DOB - Age check (17 to 30 years)
            int age = DateTime.Now.Year - dtDOB.Value.Year;
            if (dtDOB.Value > DateTime.Now.AddYears(-age)) age--;
            if (age < 17 || age > 30)
                errors.Add("Age must be between 17 and 30 years.");

            // Gender
            if (cbGender.SelectedIndex == -1)
                errors.Add("Please select a gender.");

            // Address
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
                errors.Add("Address cannot be empty.");

            // Contact Number
            if (!Regex.IsMatch(txtContact.Text, @"^[6-9]\d{9}$"))
                errors.Add("Contact must be a valid 10-digit Indian mobile number.");

            // Email
            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errors.Add("Email format is invalid.");

            // Course
            if (string.IsNullOrWhiteSpace(txtCourse.Text))
                errors.Add("Course/Branch cannot be empty.");

            // Admission Date (cannot be in future)
            if (dtAdmission.Value > DateTime.Now)
                errors.Add("Admission date cannot be in the future.");

            // Photo
            if (string.IsNullOrEmpty(photoPath) || (!photoPath.EndsWith(".jpg") && !photoPath.EndsWith(".png") && !photoPath.EndsWith(".jpeg")))
                errors.Add("Please upload a valid photo (.jpg/.jpeg/.png).");

            // Show error messages if any
            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Student Info Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void SaveStudentDataToMemory()
        {
            sName = txtName.Text.Trim();
            sDOB = dtDOB.Value;
            sGender = cbGender.SelectedItem.ToString();
            sAddress = txtAddress.Text.Trim();
            sContact = txtContact.Text.Trim();
            sEmail = txtEmail.Text.Trim();
            sCourse = txtCourse.Text.Trim();
            sAdmissionDate = dtAdmission.Value;
            sPhotoPath = photoPath;
        }

        private bool ValidateAccountTab()
        {
            if (!decimal.TryParse(txtTotalFees.Text.Trim(), out totalFees) || totalFees < 0)
            {
                MessageBox.Show("Please enter a valid Total Fees amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!decimal.TryParse(txtFeesPaid.Text.Trim(), out feesPaid) || feesPaid < 0)
            {
                MessageBox.Show("Please enter a valid Fees Paid amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!decimal.TryParse(txtPendingFees.Text.Trim(), out pendingFees) || pendingFees < 0)
            {
                MessageBox.Show("Please enter a valid Pending Fees amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!decimal.TryParse(txtScholarship.Text.Trim(), out scholarship) || scholarship < 0)
            {
                MessageBox.Show("Please enter a valid Scholarship amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!decimal.TryParse(txtFine.Text.Trim(), out fine) || fine < 0)
            {
                MessageBox.Show("Please enter a valid Fine amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void SaveAccountDataToMemory()
        {
            // Data already parsed in validation and stored in global variables
            // You can later use: totalFees, feesPaid, pendingFees, scholarship, fine
        }

        

        // Validate the entire Library tab before “Save”
        private bool ValidateLibraryTab()
        {
            if (dgvLibrary.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one book before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            foreach (DataGridViewRow row in dgvLibrary.Rows)
            {
                string bookName = row.Cells["BookName"].Value?.ToString().Trim();
                DateTime issue = DateTime.Parse(row.Cells["IssueDate"].Value.ToString());
                DateTime ret = DateTime.Parse(row.Cells["ReturnDate"].Value.ToString());
                DateTime today = DateTime.Today;

                if (string.IsNullOrWhiteSpace(bookName))
                {
                    MessageBox.Show("Book name cannot be empty in any row.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (issue > today)
                {
                    MessageBox.Show($"Issue date {issue:yyyy-MM-dd} is in the future.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (ret < issue)
                {
                    MessageBox.Show($"Return date {ret:yyyy-MM-dd} is before issue date {issue:yyyy-MM-dd}.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        // Stub for keeping data in memory; you’ll use dgvLibrary.Rows when submitting to DB
        private void SaveLibraryDataToMemory()
        {
            // Nothing needed here now – entries live in dgvLibrary for final submit
        }

        

        // Validation for adding a subject
        private bool ValidateSubjectEntry()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtSubject.Text))
                errors.Add("Subject cannot be empty.");

            if (!decimal.TryParse(txtMarks.Text.Trim(), out _))
                errors.Add("Marks must be a valid number.");

            if (string.IsNullOrWhiteSpace(txtGrade.Text))
                errors.Add("Grade cannot be empty.");

            if (!decimal.TryParse(txtSGPA.Text.Trim(), out _))
                errors.Add("SGPA must be a valid number.");

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Entry Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // Save button click event
        private void BtnSaveExam_Click(object sender, EventArgs e)
        {
            // 1. Validate Student Tab
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtContact.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtCourse.Text) || cbGender.SelectedIndex == -1 || string.IsNullOrEmpty(photoPath))
            {
                MessageBox.Show("Please fill all student details and upload a photo.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Validate Account Tab
            if (string.IsNullOrWhiteSpace(txtTotalFees.Text) || string.IsNullOrWhiteSpace(txtFeesPaid.Text) ||
                string.IsNullOrWhiteSpace(txtPendingFees.Text) || string.IsNullOrWhiteSpace(txtScholarship.Text) ||
                string.IsNullOrWhiteSpace(txtFine.Text))
            {
                MessageBox.Show("Please fill all account details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Validate Library Tab
            if (dgvLibrary.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one library record.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // Validate the entire Exam tab before saving
            if (!ValidateExamTab()) return;

            SaveExamDataToMemory();
            MessageBox.Show("Exam data saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Validate the entire Exam tab
        private bool ValidateExamTab()
        {
            if (dgvExams.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one subject before saving.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbSemester.SelectedItem == null)
            {
                MessageBox.Show("Please select a semester.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // Stub for saving Exam data to memory (this will be the final data submission part)
        private void SaveExamDataToMemory()
        {
            // At this point, we would grab data from dgvExams to submit it to the database or another system.
        }

        private void SubmitAll_Click(object sender, EventArgs e)
        {
            // 1. Validate Student Tab
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtContact.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtCourse.Text) || cbGender.SelectedIndex == -1 || string.IsNullOrEmpty(photoPath))
            {
                MessageBox.Show("Please fill all student details and upload a photo.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Validate Account Tab
            if (string.IsNullOrWhiteSpace(txtTotalFees.Text) || string.IsNullOrWhiteSpace(txtFeesPaid.Text) ||
                string.IsNullOrWhiteSpace(txtPendingFees.Text) || string.IsNullOrWhiteSpace(txtScholarship.Text) ||
                string.IsNullOrWhiteSpace(txtFine.Text))
            {
                MessageBox.Show("Please fill all account details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Validate Library Tab
            if (dgvLibrary.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one library record.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. Validate Exam Tab
            if (dgvExams.Rows.Count == 0 || cbSemester.SelectedIndex == -1)
            {
                MessageBox.Show("Please add at least one exam record and select a semester.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate Placement Tab
            if (string.IsNullOrEmpty(resumePath))
            {
                MessageBox.Show("Please select a resume file.");
                return;
            }

            if (cbType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a placement type.");
                return;
            }

            if (clbCompanies.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one company.");
                return;
            }

            try
            {
                // Insert student
                string stuQ = $@"
        INSERT INTO Students (FullName,DOB,Gender,Address,ContactNumber,Email,Course,AdmissionDate,PhotoPath)
        VALUES('{txtName.Text}','{dtDOB.Value:yyyy-MM-dd}','{cbGender.Text}','{txtAddress.Text}','{txtContact.Text}','{txtEmail.Text}','{txtCourse.Text}','{dtAdmission.Value:yyyy-MM-dd}','{photoPath}')";
                int sid = DatabaseHelper.ExecuteInsert(stuQ);

                // Insert account
                string accQ = $@"
        INSERT INTO Accounts (StudentID,TotalFees,FeesPaid,PendingFees,ScholarshipDetails,Fine)
        VALUES({sid},'{txtTotalFees.Text}','{txtFeesPaid.Text}','{txtPendingFees.Text}','{txtScholarship.Text}','{txtFine.Text}')";
                DatabaseHelper.ExecuteQuery(accQ);

                // Insert library transactions (loop through DataGridView)
                foreach (DataGridViewRow row in dgvLibrary.Rows)
                {
                    if (row.IsNewRow) continue; // Skip the new row placeholder
                    string bookName = row.Cells["BookName"].Value.ToString();
                    string issueDate = row.Cells["IssueDate"].Value.ToString();
                    string returnDate = row.Cells["ReturnDate"].Value.ToString();

                    string libQ = $@"
            INSERT INTO Library (StudentID, BookName, IssueDate, ReturnDate)
            VALUES ({sid}, '{bookName}', '{issueDate}', '{returnDate}')";
                    DatabaseHelper.ExecuteQuery(libQ);
                }

                // Insert exam subjects (loop through DataGridView)
                foreach (DataGridViewRow row in dgvExams.Rows)
                {
                    if (row.IsNewRow) continue; // Skip the new row placeholder
                    string semester = cbSemester.Text; // Assuming semester is the same for all subjects
                    string subject = row.Cells["Subject"].Value.ToString();
                    string marks = row.Cells["Marks"].Value.ToString();
                    string grade = row.Cells["Grade"].Value.ToString();
                    string sgpa = row.Cells["SGPA"].Value.ToString();

                    string exQ = $@"
            INSERT INTO Exams (StudentID,Semester,Subject,Marks,Grade,SGPA)
            VALUES({sid},'{semester}','{subject}','{marks}','{grade}','{sgpa}')";
                    DatabaseHelper.ExecuteQuery(exQ);
                }

                // Insert placement data
                string compList = string.Join(",", clbCompanies.CheckedItems.Cast<string>());
                string plcQ = $@"
        INSERT INTO Placements (StudentID,ResumePath,PlacementType,Company)
        VALUES({sid},'{resumePath}','{cbType.Text}','{compList}')";
                DatabaseHelper.ExecuteQuery(plcQ);

                MessageBox.Show("All data saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}