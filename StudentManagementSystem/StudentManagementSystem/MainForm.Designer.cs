using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace StudentManagementSystem
{
    partial class MainForm
    {
        // Main container
        private TabControl tabControl;
        private TabPage tabStudent, tabAccount, tabLibrary, tabExam, tabPlacement;

        // Student controls
        private TextBox txtName, txtAddress, txtContact, txtEmail, txtCourse;
        private DateTimePicker dtDOB, dtAdmission;
        private ComboBox cbGender;
        private PictureBox pictureBox;
        private string photoPath;
        private Button btnBrowsePhoto;

        // Account controls
        private TextBox txtTotalFees, txtFeesPaid, txtPendingFees, txtScholarship, txtFine;

        // Library controls
        private TextBox txtBookName;
        private DateTimePicker dtIssueDate, dtReturnDate;
        private Button btnAddBook;
        private DataGridView dgvLibrary;
        private Button btnSaveLibrary;

        // Exam controls
        private ComboBox cbSemester;
        private TextBox txtSubject, txtMarks, txtGrade, txtSGPA;
        private DataGridView dgvExams;
        private Button btnAddSubject;
        private Button btnSaveExam;

        // Placement controls
        private Label lblResumeName;
        private string resumePath;
        private Button btnBrowseResume;
        private ComboBox cbType;
        private CheckedListBox clbCompanies;

        // Final submit
        private Button btnSubmitAll;

        private void ApplyModernStyling(Control container)
        {
            container.BackColor = Color.FromArgb(240, 255, 240); // Light pastel background

            foreach (Control ctrl in container.Controls)
            {
                if (ctrl.Tag?.ToString() == "SkipStyle")
                    continue;

                if (ctrl is Label lbl)
                {
                    lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    lbl.ForeColor = Color.FromArgb(50, 50, 50);
                }
                else if (ctrl is TextBox txt)
                {
                    txt.Font = new Font("Segoe UI", 10);
                    txt.BackColor = Color.White;
                    txt.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (ctrl is ComboBox cb)
                {
                    cb.Font = new Font("Segoe UI", 10);
                    cb.BackColor = Color.White;
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                else if (ctrl is Button btn)
                {
                    btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.BackColor = Color.FromArgb(64, 64, 64); // Default Blue
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Cursor = Cursors.Hand;

                    // 3D shadow effect using padding and margin (optional look)
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 144, 255); // Slightly lighter blue on hover
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 100, 210); // Darker blue when pressed

                    // Set rounded corners for button
                    int cornerRadius = 5; // You can change this value
                    GraphicsPath roundPath = new GraphicsPath();
                    int arcSize = cornerRadius * 2;

                    roundPath.StartFigure();
                    roundPath.AddArc(0, 0, arcSize, arcSize, 180, 90);
                    roundPath.AddLine(cornerRadius, 0, btn.Width - cornerRadius, 0);
                    roundPath.AddArc(btn.Width - arcSize, 0, arcSize, arcSize, 270, 90);
                    roundPath.AddLine(btn.Width, cornerRadius, btn.Width, btn.Height - cornerRadius);
                    roundPath.AddArc(btn.Width - arcSize, btn.Height - arcSize, arcSize, arcSize, 0, 90);
                    roundPath.AddLine(btn.Width - cornerRadius, btn.Height, cornerRadius, btn.Height);
                    roundPath.AddArc(0, btn.Height - arcSize, arcSize, arcSize, 90, 90);
                    roundPath.AddLine(0, btn.Height - cornerRadius, 0, cornerRadius);
                    roundPath.CloseFigure();

                    btn.Region = new Region(roundPath);

                    // Optional: maintain rounded corners on resize
                    btn.Resize += (s, e) =>
                    {
                        GraphicsPath resizePath = new GraphicsPath();
                        resizePath.StartFigure();
                        resizePath.AddArc(0, 0, arcSize, arcSize, 180, 90);
                        resizePath.AddLine(cornerRadius, 0, btn.Width - cornerRadius, 0);
                        resizePath.AddArc(btn.Width - arcSize, 0, arcSize, arcSize, 270, 90);
                        resizePath.AddLine(btn.Width, cornerRadius, btn.Width, btn.Height - cornerRadius);
                        resizePath.AddArc(btn.Width - arcSize, btn.Height - arcSize, arcSize, arcSize, 0, 90);
                        resizePath.AddLine(btn.Width - cornerRadius, btn.Height, cornerRadius, btn.Height);
                        resizePath.AddArc(0, btn.Height - arcSize, arcSize, arcSize, 90, 90);
                        resizePath.AddLine(0, btn.Height - cornerRadius, 0, cornerRadius);
                        resizePath.CloseFigure();

                        btn.Region = new Region(resizePath);
                    };
                }
                else if (ctrl is DateTimePicker dt)
                {
                    dt.Font = new Font("Segoe UI", 10);
                }
                else if (ctrl is CheckedListBox clb)
                {
                    clb.Font = new Font("Segoe UI", 10);
                }
                else if (ctrl is DataGridView dgv)
                {
                    dgv.BackgroundColor = Color.White;
                    dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
                    dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 123, 255);
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgv.EnableHeadersVisualStyles = false;
                }

                if (ctrl.HasChildren)
                {
                    ApplyModernStyling(ctrl);
                }
            }
        }

        private void InitializeComponent()
        {
            tabControl = new TabControl { Dock = DockStyle.Fill };
            tabStudent = new TabPage("Student Info");
            tabAccount = new TabPage("Accounts");
            tabLibrary = new TabPage("Library");
            tabExam = new TabPage("Exam Cell");
            tabPlacement = new TabPage("Placement Cell");
            tabControl.TabPages.AddRange(new[] { tabStudent, tabAccount, tabLibrary, tabExam, tabPlacement });

            BuildStudentTab();
            BuildAccountTab();
            BuildLibraryTab();
            BuildExamTab();
            BuildPlacementTab();

            btnSubmitAll = new Button { Text = "Submit All", Top = 320, Left = 130, Width = 120 };
            btnSubmitAll.Click += SubmitAll_Click;
            tabPlacement.Controls.Add(btnSubmitAll);

            Controls.Add(tabControl);
            Text = "Student Registration and Management System";
            Size = new Size(500, 700); // width = 1000px, height = 700px
            WindowState = FormWindowState.Normal;
            StartPosition = FormStartPosition.CenterScreen;

            foreach (TabPage tab in tabControl.TabPages)
            {
                ApplyModernStyling(tab);
            }
        }

        private void AdjustFormHeight()
        {
            // find the bottom of the lowest control in the current tab:
            var tab = tabControl.SelectedTab;
            if (tab == null) return;

            int maxBottom = 0;
            foreach (Control c in tab.Controls)
                maxBottom = Math.Max(maxBottom, c.Bottom);

            // Add some padding below the last control:
            const int padding = 20;

            // compute the height needed for the client area:
            int requiredClientHeight = maxBottom + padding;

            // account for the non‐client area (title bar, borders):
            int nonClientHeight = this.Height - this.ClientSize.Height;

            // set the new overall form height:
            this.Height = requiredClientHeight + nonClientHeight + 30;
        }

        private void BuildStudentTab()
        {
            // Student Info controls layout
            tabStudent.SuspendLayout();

            var lblTabHeading = new Label
            {
                Text = "Student Information",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 153, 51),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Tag = "SkipStyle" // <-- tell styling to ignore this label
            };

            // Temporarily add to tab to calculate PreferredSize
            tabStudent.Controls.Add(lblTabHeading);
            lblTabHeading.Left = (tabStudent.ClientSize.Width - lblTabHeading.PreferredWidth) / 2;

            // Re-center on tab resize
            tabStudent.Resize += (s, e) =>
            {
                lblTabHeading.Left = (tabStudent.ClientSize.Width - lblTabHeading.PreferredWidth) / 2;
            };


            var lblName = new Label { Text = "Full Name", Top = 60, Left = 20, Width = 100 };
            txtName = new TextBox { Top = 60, Left = 130, Width = 200 };

            var lblDOB = new Label { Text = "Date of Birth", Top = 100, Left = 20, Width = 100 };
            dtDOB = new DateTimePicker { Top = 100, Left = 130 };

            var lblGender = new Label { Text = "Gender", Top = 140, Left = 20, Width = 100 };
            cbGender = new ComboBox { Top = 140, Left = 130, Width = 200 };
            cbGender.Items.AddRange(new[] { "Male", "Female", "Other" });

            var lblAddress = new Label { Text = "Address", Top = 180, Left = 20, Width = 100 };
            txtAddress = new TextBox { Top = 180, Left = 130, Width = 200 };

            var lblContact = new Label { Text = "Contact", Top = 220, Left = 20, Width = 100 };
            txtContact = new TextBox { Top = 220, Left = 130, Width = 200 };

            var lblEmail = new Label { Text = "Email", Top = 260, Left = 20, Width = 100 };
            txtEmail = new TextBox { Top = 260, Left = 130, Width = 200 };

            var lblCourse = new Label { Text = "Course/Branch", Top = 300, Left = 20, Width = 100 };
            txtCourse = new TextBox { Top = 300, Left = 130, Width = 200 };

            var lblAdmission = new Label { Text = "Admission Date", Top = 340, Left = 20, Width = 100 };
            dtAdmission = new DateTimePicker { Top = 340, Left = 130 };

            var lblPhoto = new Label { Text = "Photograph", Top = 380, Left = 20, Width = 100 };
            pictureBox = new PictureBox { Top = 380, Left = 130, Width = 100, Height = 100, BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.StretchImage };
            btnBrowsePhoto = new Button { Text = "Browse...", Top = 490, Left = 130, Width = 70, Height = 30};
            btnBrowsePhoto.Click += (s, e) =>
            {
                var ofd = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png" };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    photoPath = ofd.FileName;
                    pictureBox.ImageLocation = photoPath;
                }
            };

            var btnSaveStudent = new Button { Text = "Save", Top = 490, Left = 220, Width = 70, Height = 30 };
            btnSaveStudent.Click += (s, e) =>
            {
                if (ValidateStudentTab())
                {
                    SaveStudentDataToMemory();
                    MessageBox.Show("Student info saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            tabStudent.Controls.AddRange(new Control[]
            {
                lblName, txtName, lblDOB, dtDOB, lblGender, cbGender,
                lblAddress, txtAddress, lblContact, txtContact,
                lblEmail, txtEmail, lblCourse, txtCourse,
                lblAdmission, dtAdmission, lblPhoto, pictureBox, btnBrowsePhoto, btnSaveStudent
            });
            tabStudent.ResumeLayout();
        }

        private void BuildAccountTab()
        {
            tabAccount.SuspendLayout();

            var lblAccountHeading = new Label
            {
                Text = "Account Information",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 153, 51),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Tag = "SkipStyle"
            };

            tabAccount.Controls.Add(lblAccountHeading);
            lblAccountHeading.Left = (tabAccount.ClientSize.Width - lblAccountHeading.PreferredWidth) / 2;
            lblAccountHeading.Top = 10;

            tabAccount.Resize += (s, e) =>
            {
                lblAccountHeading.Left = (tabAccount.ClientSize.Width - lblAccountHeading.PreferredWidth) / 2;
            };

            var lblTotal = new Label { Text = "Total Fees", Top = 60, Left = 20, Width = 100 };
            txtTotalFees = new TextBox { Top = 60, Left = 130, Width = 200 };

            var lblPaid = new Label { Text = "Fees Paid", Top = 100, Left = 20, Width = 100 };
            txtFeesPaid = new TextBox { Top = 100, Left = 130, Width = 200 };

            var lblPending = new Label { Text = "Pending Fees", Top = 140, Left = 20, Width = 100 };
            txtPendingFees = new TextBox { Top = 140, Left = 130, Width = 200 };

            var lblScholar = new Label { Text = "Scholarship", Top = 180, Left = 20, Width = 100 };
            txtScholarship = new TextBox { Top = 180, Left = 130, Width = 200 };

            var lblFine = new Label { Text = "Fine", Top = 220, Left = 20, Width = 100 };
            txtFine = new TextBox { Top = 220, Left = 130, Width = 200 };

            var btnSaveAccount = new Button { Text = "Save", Top = 270, Left = 130, Width = 70, Height = 30 };
            btnSaveAccount.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtContact.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtCourse.Text) || cbGender.SelectedIndex == -1 || string.IsNullOrEmpty(photoPath))
                {
                    MessageBox.Show("Please fill all student details and upload a photo.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ValidateAccountTab())
                {
                    SaveAccountDataToMemory();
                    MessageBox.Show("Account info saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            tabAccount.Controls.AddRange(new Control[]
            {
                lblTotal, txtTotalFees, lblPaid, txtFeesPaid,
                lblPending, txtPendingFees, lblScholar, txtScholarship,
                lblFine, txtFine, btnSaveAccount
            });
            tabAccount.ResumeLayout();
        }

        private void BuildLibraryTab()
        {
            tabLibrary.SuspendLayout();

            var lblLibraryHeading = new Label
            {
                Text = "Library Book Records",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 153, 51),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Tag = "SkipStyle"
            };

            tabLibrary.Controls.Add(lblLibraryHeading);
            lblLibraryHeading.Left = (tabLibrary.ClientSize.Width - lblLibraryHeading.PreferredWidth) / 2;
            lblLibraryHeading.Top = 10;

            tabLibrary.Resize += (s, e) =>
            {
                lblLibraryHeading.Left = (tabLibrary.ClientSize.Width - lblLibraryHeading.PreferredWidth) / 2;
            };

            var lblBook = new Label { Text = "Book Name", Top = 60, Left = 20, Width = 100 };
            txtBookName = new TextBox { Top = 60, Left = 130, Width = 200 };

            var lblIssue = new Label { Text = "Issue Date", Top = 100, Left = 20, Width = 100 };
            dtIssueDate = new DateTimePicker { Top = 100, Left = 130 };

            var lblReturn = new Label { Text = "Return Date", Top = 140, Left = 20, Width = 100 };
            dtReturnDate = new DateTimePicker { Top = 140, Left = 130 };

            btnAddBook = new Button { Text = "Add Book", Top = 180, Left = 130, Width = 70, Height = 30 };
            btnAddBook.Click += (s, e) =>
            {
                string bookName = txtBookName.Text.Trim();
                DateTime issue = dtIssueDate.Value.Date;
                DateTime ret = dtReturnDate.Value.Date;
                DateTime today = DateTime.Today;

                if (string.IsNullOrWhiteSpace(bookName))
                {
                    MessageBox.Show("Book name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (issue > today)
                {
                    MessageBox.Show("Issue date cannot be in the future.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (ret < issue)
                {
                    MessageBox.Show("Return date cannot be before issue date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                dgvLibrary.Rows.Add(bookName, issue.ToString("yyyy-MM-dd"), ret.ToString("yyyy-MM-dd"));

                txtBookName.Clear();
                dtIssueDate.Value = today;
                dtReturnDate.Value = today;
            };

            // Initialize DataGridView
            dgvLibrary = new DataGridView
            {
                Top = 220,
                Left = 20,
                Width = 340,
                Height = 200,
                ColumnCount = 3,
                AllowUserToAddRows = false,
                ReadOnly = true
            };

            dgvLibrary.Left = (tabLibrary.ClientSize.Width - dgvLibrary.Width) / 2;

            // Keep it centered on resize:
            tabLibrary.Resize += (s, e) =>
            {
                dgvLibrary.Left = (tabLibrary.ClientSize.Width - dgvLibrary.Width) / 2;
            };
            dgvLibrary.Columns[0].Name = "BookName";
            dgvLibrary.Columns[1].Name = "IssueDate";
            dgvLibrary.Columns[2].Name = "ReturnDate";

            btnSaveLibrary = new Button { Text = "Save", Top = 440, Left = 130, Width = 70, Height = 30 };
            btnSaveLibrary.Click += (s, e) =>
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

                if (!ValidateLibraryTab()) return;
                SaveLibraryDataToMemory();
                MessageBox.Show("Library entries saved!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            // Add controls
            tabLibrary.Controls.AddRange(new Control[]
            {
                lblBook, txtBookName,
                lblIssue, dtIssueDate,
                lblReturn, dtReturnDate,
                btnAddBook, dgvLibrary, btnSaveLibrary
            });
            tabLibrary.ResumeLayout();
        }

        private bool ValidateLibraryEntry()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtBookName.Text))
                errors.Add("Book Name cannot be empty.");

            if (dtIssueDate.Value > DateTime.Today)
                errors.Add("Issue Date cannot be in the future.");

            if (dtReturnDate.Value < dtIssueDate.Value)
                errors.Add("Return Date cannot be before Issue Date.");

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Entry Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void BuildExamTab()
        {
            tabExam.SuspendLayout();

            var lblExamHeading = new Label
            {
                Text = "Exam Details",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 153, 51),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Tag = "SkipStyle"
            };

            tabExam.Controls.Add(lblExamHeading);
            lblExamHeading.Left = (tabExam.ClientSize.Width - lblExamHeading.PreferredWidth) / 2;
            lblExamHeading.Top = 10;

            tabExam.Resize += (s, e) =>
            {
                lblExamHeading.Left = (tabExam.ClientSize.Width - lblExamHeading.PreferredWidth) / 2;
            };

            var lblSem = new Label { Text = "Semester", Top = 60, Left = 20, Width = 100 };
            cbSemester = new ComboBox { Top = 60, Left = 130, Width = 200 };
            cbSemester.Items.AddRange(new[] { "1", "2", "3", "4", "5", "6", "7", "8" });

            var lblSub = new Label { Text = "Subject", Top = 100, Left = 20, Width = 100 };
            txtSubject = new TextBox { Top = 100, Left = 130, Width = 200 };

            var lblMarks = new Label { Text = "Marks", Top = 140, Left = 20, Width = 100 };
            txtMarks = new TextBox { Top = 140, Left = 130, Width = 200 };

            var lblGrade = new Label { Text = "Grade", Top = 180, Left = 20, Width = 100 };
            txtGrade = new TextBox { Top = 180, Left = 130, Width = 200 };

            var lblSGPA = new Label { Text = "GPA", Top = 220, Left = 20, Width = 100 };
            txtSGPA = new TextBox { Top = 220, Left = 130, Width = 200 };

            // DataGridView for displaying added subjects
            dgvExams = new DataGridView
            {
                Top = 280,
                Left = 20,
                Width = 420,
                Height = 200,
                Columns = {
            new DataGridViewTextBoxColumn { HeaderText = "Subject", Name = "Subject" },
            new DataGridViewTextBoxColumn { HeaderText = "Marks", Name = "Marks" },
            new DataGridViewTextBoxColumn { HeaderText = "Grade", Name = "Grade" },
            new DataGridViewTextBoxColumn { HeaderText = "GPA", Name = "SGPA" }
        },
                AllowUserToAddRows = false
            };

            // Center it now:
            dgvExams.Left = (tabExam.ClientSize.Width - dgvExams.Width) / 2;

            // Keep it centered on resize:
            tabExam.Resize += (s, e) =>
            {
                dgvExams.Left = (tabExam.ClientSize.Width - dgvExams.Width) / 2;
            };

            // Button to add a subject to the DataGridView
            btnAddSubject = new Button
            {
                Text = "Add Subject",
                Top = 220,
                Left = 350,
                Width = 70,
                Height = 30
            };
            btnAddSubject.Click += BtnAddSubject_Click;

            btnSaveExam = new Button
            {
                Text = "Save",
                Top = 510,
                Left = 130,
                Width = 70,
                Height = 30
            };
            btnSaveExam.Click += BtnSaveExam_Click;

            tabExam.Controls.AddRange(new Control[] {
        lblSem, cbSemester, lblSub, txtSubject, lblMarks, txtMarks, lblGrade, txtGrade, lblSGPA, txtSGPA,
        dgvExams, btnAddSubject, btnSaveExam
    });

            tabExam.ResumeLayout();
        }

        private void BtnAddSubject_Click(object sender, EventArgs e)
        {
            // Validate the current subject entry
            if (!ValidateSubjectEntry()) return;

            // Get the entered values from the TextBox controls
            string subject = txtSubject.Text.Trim();
            string marks = txtMarks.Text.Trim();
            string grade = txtGrade.Text.Trim();
            string sgpa = txtSGPA.Text.Trim();

            // Add the values to the DataGridView
            dgvExams.Rows.Add(subject, marks, grade, sgpa);

            // Clear the TextBoxes for the next input
            txtSubject.Clear();
            txtMarks.Clear();
            txtGrade.Clear();
            txtSGPA.Clear();
        }

        private void BuildPlacementTab()
        {
            tabPlacement.SuspendLayout();

            var lblPlacementHeading = new Label
            {
                Text = "Placement Cell Details",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 153, 51),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Tag = "SkipStyle"
            };

            tabPlacement.Controls.Add(lblPlacementHeading);
            lblPlacementHeading.Left = (tabPlacement.ClientSize.Width - lblPlacementHeading.PreferredWidth) / 2;
            lblPlacementHeading.Top = 10;

            tabPlacement.Resize += (s, e) =>
            {
                lblPlacementHeading.Left = (tabPlacement.ClientSize.Width - lblPlacementHeading.PreferredWidth) / 2;
            };

            var lblResume = new Label { Text = "Resume (PDF)", Top = 60, Left = 20, Width = 100 };
            btnBrowseResume = new Button { Text = "Browse...", Top = 60, Left = 130, Width = 70, Height = 30 };
            lblResumeName = new Label { Text = "No file selected", Top = 100, Left = 20, Width = 300 };
            btnBrowseResume.Click += (s, e) =>
            {
                var ofd = new OpenFileDialog { Filter = "PDF Files|*.pdf" };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    resumePath = ofd.FileName;
                    lblResumeName.Text = System.IO.Path.GetFileName(resumePath);
                }
            };

            var lblType = new Label { Text = "Placement Type", Top = 140, Left = 20, Width = 100 };
            cbType = new ComboBox { Top = 140, Left = 130, Width = 200 };
            cbType.Items.AddRange(new[] { "Internship", "Campus Placement" });

            var lblComp = new Label { Text = "Companies", Top = 180, Left = 20, Width = 100 };
            clbCompanies = new CheckedListBox { Top = 180, Left = 130, Width = 200, Height = 80 };
            clbCompanies.Items.AddRange(new[] { "Microsoft", "Google", "Amazon", "Facebook", "Other" });

            tabPlacement.Controls.AddRange(new Control[] { lblResume, btnBrowseResume, lblResumeName, lblType, cbType, lblComp, clbCompanies });
            tabPlacement.ResumeLayout();
        }
    }
}