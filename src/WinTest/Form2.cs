using System;
using System.ComponentModel;
using System.Windows.Forms;
using StickyWindows;

namespace WinTest {
    /// <summary>
    /// Summary description for Form2.
    /// </summary>
    public class Form2 : Form {
        private StickyWindow stickyWindow;

        private Label    labelWindowType;
        private ComboBox comboWindowType;
        private Label    labelStickGravity;
        private TextBox  textStickGravity;
        private CheckBox checkStickToScreen;
        private CheckBox checkStickToOthers;
        private CheckBox checkStickOnResize;
        private CheckBox checkStickOnMove;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        public Form2() {
            InitializeComponent();
            stickyWindow = new StickyWindow(this);
            comboWindowType.SelectedIndex = (int) stickyWindow.WindowType;
            textStickGravity.Text = stickyWindow.StickGravity.ToString();
            checkStickOnMove.Checked = stickyWindow.StickOnMove;
            checkStickOnResize.Checked = stickyWindow.StickOnResize;
            checkStickToOthers.Checked = stickyWindow.StickToOther;
            checkStickToScreen.Checked = stickyWindow.StickToScreen;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.labelWindowType = new System.Windows.Forms.Label();
            this.comboWindowType = new System.Windows.Forms.ComboBox();
            this.labelStickGravity = new System.Windows.Forms.Label();
            this.textStickGravity = new System.Windows.Forms.TextBox();
            this.checkStickToScreen = new System.Windows.Forms.CheckBox();
            this.checkStickToOthers = new System.Windows.Forms.CheckBox();
            this.checkStickOnResize = new System.Windows.Forms.CheckBox();
            this.checkStickOnMove = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();

            // 
            // labelWindowType
            // 
            this.labelWindowType.Location = new System.Drawing.Point(26, 30);
            this.labelWindowType.Name = "labelWindowType";
            this.labelWindowType.Size = new System.Drawing.Size(85, 28);
            this.labelWindowType.Text = "Window Type";

            // 
            // comboWindowType
            // 
            this.comboWindowType.Location = new System.Drawing.Point(115, 30);
            this.comboWindowType.Items.AddRange(new object[] { "None", "Anchor", "Grabby", "Sticky", "Cohesive" });
            this.comboWindowType.Name = "comboWindowType";
            this.comboWindowType.Size = new System.Drawing.Size(75, 28);
            this.comboWindowType.TabIndex = 0;
            this.comboWindowType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboWindowType.SelectedIndexChanged += new System.EventHandler(this.comboWindowType_SelectionChanged);

            // 
            // labelStickGravity
            // 
            this.labelStickGravity.Location = new System.Drawing.Point(26, 65);
            this.labelStickGravity.Name = "labelStickGravity";
            this.labelStickGravity.Size = new System.Drawing.Size(85, 28);
            this.labelStickGravity.Text = "Stick Gravity";

            // 
            // textStickGravity
            // 
            this.textStickGravity.Location = new System.Drawing.Point(115, 65);
            this.textStickGravity.Name = "textStickGravity";
            this.textStickGravity.Size = new System.Drawing.Size(30, 28);
            this.textStickGravity.TabIndex = 1;
            this.textStickGravity.Text = "Stick Gravity";
            this.textStickGravity.TextChanged += new System.EventHandler(this.textStickGravity_TextChanged);

            // 
            // checkStickToScreen
            // 
            this.checkStickToScreen.Location = new System.Drawing.Point(26, 100);
            this.checkStickToScreen.Name = "checkStickToScreen";
            this.checkStickToScreen.Size = new System.Drawing.Size(125, 28);
            this.checkStickToScreen.TabIndex = 2;
            this.checkStickToScreen.Text = "Stick to Screen";
            this.checkStickToScreen.CheckedChanged += new System.EventHandler(this.checkStickToScreen_CheckedChanged);

            // 
            // checkStickToOthers
            // 
            this.checkStickToOthers.Location = new System.Drawing.Point(26, 135);
            this.checkStickToOthers.Name = "checkStickToOthers";
            this.checkStickToOthers.Size = new System.Drawing.Size(125, 28);
            this.checkStickToOthers.TabIndex = 3;
            this.checkStickToOthers.Text = "Stick to Others";
            this.checkStickToOthers.CheckedChanged += new System.EventHandler(this.checkStickToOthers_CheckedChanged);

            // 
            // checkStickOnResize
            // 
            this.checkStickOnResize.Location = new System.Drawing.Point(26, 170);
            this.checkStickOnResize.Name = "checkStickOnResize";
            this.checkStickOnResize.Size = new System.Drawing.Size(125, 27);
            this.checkStickOnResize.TabIndex = 4;
            this.checkStickOnResize.Text = "Stick on Resize";
            this.checkStickOnResize.CheckedChanged += new System.EventHandler(this.checkStickOnResize_CheckedChanged);

            // 
            // checkStickOnMove
            // 
            this.checkStickOnMove.Location = new System.Drawing.Point(26, 205);
            this.checkStickOnMove.Name = "checkStickOnMove";
            this.checkStickOnMove.Size = new System.Drawing.Size(125, 27);
            this.checkStickOnMove.TabIndex = 5;
            this.checkStickOnMove.Text = "Stick On Move";
            this.checkStickOnMove.CheckedChanged += new System.EventHandler(this.checkStickOnMove_CheckedChanged);

            // 
            // Form2
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.labelWindowType);
            this.Controls.Add(this.comboWindowType);
            this.Controls.Add(this.labelStickGravity);
            this.Controls.Add(this.textStickGravity);
            this.Controls.Add(this.checkStickOnMove);
            this.Controls.Add(this.checkStickOnResize);
            this.Controls.Add(this.checkStickToOthers);
            this.Controls.Add(this.checkStickToScreen);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private void Form2_Load(object sender, EventArgs e) {}

        private void comboWindowType_SelectionChanged(object sender, EventArgs e) {
            stickyWindow.WindowType = (StickyWindowType) comboWindowType.SelectedIndex;
        }

        private void textStickGravity_TextChanged(object sender, EventArgs e) {
            stickyWindow.StickGravity = Int32.TryParse(textStickGravity.Text, out int gravity) ? gravity : 0;
        }

        private void checkStickToScreen_CheckedChanged(object sender, EventArgs e) {
            stickyWindow.StickToScreen = checkStickToScreen.Checked;
        }

        private void checkStickToOthers_CheckedChanged(object sender, EventArgs e) {
            stickyWindow.StickToOther = checkStickToOthers.Checked;
        }

        private void checkStickOnResize_CheckedChanged(object sender, EventArgs e) {
            stickyWindow.StickOnResize = checkStickOnResize.Checked;
        }

        private void checkStickOnMove_CheckedChanged(object sender, EventArgs e) {
            stickyWindow.StickOnMove = checkStickOnMove.Checked;
        }
    }
}
