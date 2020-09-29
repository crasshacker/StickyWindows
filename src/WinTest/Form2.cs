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
        private Label    labelClientMoveKey;
        private ComboBox comboClientMoveKey;
        private Label    labelStickGravity;
        private TextBox  textStickGravity;
        private CheckBox checkStickToScreen;
        private CheckBox checkStickToOthers;
        private CheckBox checkStickOnResize;
        private CheckBox checkStickOnMove;
        private CheckBox checkStickToInside;
        private CheckBox checkStickToOutside;
        private CheckBox checkStickyCorners;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        public Form2() {
            InitializeComponent();
            stickyWindow = new StickyWindow(this);
            comboWindowType.SelectedIndex = (int) stickyWindow.WindowType;
            comboClientMoveKey.SelectedItem = Enum.GetName(typeof(StickyWindow.ModifierKey),
                                                            stickyWindow.ClientAreaMoveKey);
            textStickGravity.Text = stickyWindow.StickGravity.ToString();
            checkStickOnMove.Checked = stickyWindow.StickOnMove;
            checkStickOnResize.Checked = stickyWindow.StickOnResize;
            checkStickToOthers.Checked = stickyWindow.StickToOther;
            checkStickToScreen.Checked = stickyWindow.StickToScreen;
            checkStickToInside.Checked = stickyWindow.StickToInside;
            checkStickToOutside.Checked = stickyWindow.StickToOutside;
            checkStickyCorners.Checked = stickyWindow.StickyCorners;
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
            this.labelClientMoveKey = new System.Windows.Forms.Label();
            this.comboClientMoveKey = new System.Windows.Forms.ComboBox();
            this.labelStickGravity = new System.Windows.Forms.Label();
            this.textStickGravity = new System.Windows.Forms.TextBox();
            this.checkStickToScreen = new System.Windows.Forms.CheckBox();
            this.checkStickToOthers = new System.Windows.Forms.CheckBox();
            this.checkStickOnResize = new System.Windows.Forms.CheckBox();
            this.checkStickOnMove = new System.Windows.Forms.CheckBox();
            this.checkStickToInside = new System.Windows.Forms.CheckBox();
            this.checkStickToOutside = new System.Windows.Forms.CheckBox();
            this.checkStickyCorners = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelWindowType
            // 
            this.labelWindowType.Location = new System.Drawing.Point(26, 32);
            this.labelWindowType.Name = "labelWindowType";
            this.labelWindowType.Size = new System.Drawing.Size(85, 30);
            this.labelWindowType.TabIndex = 0;
            this.labelWindowType.Text = "Window type";
            // 
            // comboWindowType
            // 
            this.comboWindowType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWindowType.Items.AddRange(new object[] {
            "None",
            "Anchor",
            "Grabby",
            "Sticky",
            "Cohesive"});
            this.comboWindowType.Location = new System.Drawing.Point(157, 32);
            this.comboWindowType.Name = "comboWindowType";
            this.comboWindowType.Size = new System.Drawing.Size(75, 23);
            this.comboWindowType.TabIndex = 0;
            this.comboWindowType.SelectedIndexChanged += new System.EventHandler(this.comboWindowType_SelectionChanged);
            // 
            // labelClientMoveKey
            // 
            this.labelClientMoveKey.Location = new System.Drawing.Point(26, 69);
            this.labelClientMoveKey.Name = "labelClientMoveKey";
            this.labelClientMoveKey.Size = new System.Drawing.Size(125, 30);
            this.labelClientMoveKey.TabIndex = 1;
            this.labelClientMoveKey.Text = "Client area move key";
            // 
            // comboClientMoveKey
            // 
            this.comboClientMoveKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboClientMoveKey.Items.AddRange(new object[] {
            "None",
            "Control",
            "Shift"});
            this.comboClientMoveKey.Location = new System.Drawing.Point(157, 69);
            this.comboClientMoveKey.Name = "comboClientMoveKey";
            this.comboClientMoveKey.Size = new System.Drawing.Size(75, 23);
            this.comboClientMoveKey.TabIndex = 0;
            this.comboClientMoveKey.SelectedIndexChanged += new System.EventHandler(this.comboClientMoveKey_SelectionChanged);
            // 
            // labelStickGravity
            // 
            this.labelStickGravity.Location = new System.Drawing.Point(26, 107);
            this.labelStickGravity.Name = "labelStickGravity";
            this.labelStickGravity.Size = new System.Drawing.Size(85, 30);
            this.labelStickGravity.TabIndex = 2;
            this.labelStickGravity.Text = "Stick gravity";
            // 
            // textStickGravity
            // 
            this.textStickGravity.Location = new System.Drawing.Point(157, 107);
            this.textStickGravity.Name = "textStickGravity";
            this.textStickGravity.Size = new System.Drawing.Size(75, 23);
            this.textStickGravity.TabIndex = 1;
            this.textStickGravity.TextChanged += new System.EventHandler(this.textStickGravity_TextChanged);
            // 
            // checkStickToScreen
            // 
            this.checkStickToScreen.Location = new System.Drawing.Point(26, 142);
            this.checkStickToScreen.Name = "checkStickToScreen";
            this.checkStickToScreen.Size = new System.Drawing.Size(125, 30);
            this.checkStickToScreen.TabIndex = 2;
            this.checkStickToScreen.Text = "Stick to screen";
            this.checkStickToScreen.CheckedChanged += new System.EventHandler(this.checkStickToScreen_CheckedChanged);
            // 
            // checkStickToOthers
            // 
            this.checkStickToOthers.Location = new System.Drawing.Point(26, 177);
            this.checkStickToOthers.Name = "checkStickToOthers";
            this.checkStickToOthers.Size = new System.Drawing.Size(125, 30);
            this.checkStickToOthers.TabIndex = 3;
            this.checkStickToOthers.Text = "Stick to others";
            this.checkStickToOthers.CheckedChanged += new System.EventHandler(this.checkStickToOthers_CheckedChanged);
            // 
            // checkStickOnResize
            // 
            this.checkStickOnResize.Location = new System.Drawing.Point(26, 212);
            this.checkStickOnResize.Name = "checkStickOnResize";
            this.checkStickOnResize.Size = new System.Drawing.Size(125, 28);
            this.checkStickOnResize.TabIndex = 4;
            this.checkStickOnResize.Text = "Stick on resize";
            this.checkStickOnResize.CheckedChanged += new System.EventHandler(this.checkStickOnResize_CheckedChanged);
            // 
            // checkStickOnMove
            // 
            this.checkStickOnMove.Location = new System.Drawing.Point(26, 247);
            this.checkStickOnMove.Name = "checkStickOnMove";
            this.checkStickOnMove.Size = new System.Drawing.Size(125, 29);
            this.checkStickOnMove.TabIndex = 5;
            this.checkStickOnMove.Text = "Stick on move";
            this.checkStickOnMove.CheckedChanged += new System.EventHandler(this.checkStickOnMove_CheckedChanged);
            // 
            // checkStickToInside
            // 
            this.checkStickToInside.Location = new System.Drawing.Point(26, 282);
            this.checkStickToInside.Name = "checkStickToInside";
            this.checkStickToInside.Size = new System.Drawing.Size(125, 24);
            this.checkStickToInside.TabIndex = 6;
            this.checkStickToInside.Text = "Stick to inside";
            this.checkStickToInside.CheckedChanged += new System.EventHandler(this.checkStickToInside_CheckedChanged);
            // 
            // checkStickToOutside
            // 
            this.checkStickToOutside.Location = new System.Drawing.Point(26, 317);
            this.checkStickToOutside.Name = "checkStickToOutside";
            this.checkStickToOutside.Size = new System.Drawing.Size(125, 24);
            this.checkStickToOutside.TabIndex = 7;
            this.checkStickToOutside.Text = "Stick to outside";
            this.checkStickToOutside.CheckedChanged += new System.EventHandler(this.checkStickToOutside_CheckedChanged);
            // 
            // checkStickyCorners
            // 
            this.checkStickyCorners.Location = new System.Drawing.Point(26, 352);
            this.checkStickyCorners.Name = "checkStickyCorners";
            this.checkStickyCorners.Size = new System.Drawing.Size(125, 24);
            this.checkStickyCorners.TabIndex = 8;
            this.checkStickyCorners.Text = "Sticky corners";
            this.checkStickyCorners.CheckedChanged += new System.EventHandler(this.checkStickyCorners_CheckedChanged);
            // 
            // Form2
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.ClientSize = new System.Drawing.Size(292, 400);
            this.Controls.Add(this.labelWindowType);
            this.Controls.Add(this.comboWindowType);
            this.Controls.Add(this.labelClientMoveKey);
            this.Controls.Add(this.comboClientMoveKey);
            this.Controls.Add(this.labelStickGravity);
            this.Controls.Add(this.textStickGravity);
            this.Controls.Add(this.checkStickOnMove);
            this.Controls.Add(this.checkStickOnResize);
            this.Controls.Add(this.checkStickToOthers);
            this.Controls.Add(this.checkStickToScreen);
            this.Controls.Add(this.checkStickToInside);
            this.Controls.Add(this.checkStickToOutside);
            this.Controls.Add(this.checkStickyCorners);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void Form2_Load(object sender, EventArgs e) {}

        private void comboWindowType_SelectionChanged(object sender, EventArgs e) {
            stickyWindow.WindowType = (StickyWindowType) comboWindowType.SelectedIndex;
        }

        private void comboClientMoveKey_SelectionChanged(object sender, EventArgs e) {
            stickyWindow.ClientAreaMoveKey = comboClientMoveKey.SelectedItem.ToString() switch
            {
                "None"    => StickyWindow.ModifierKey.None,
                "Control" => StickyWindow.ModifierKey.Control,
                "Shift"   => StickyWindow.ModifierKey.Shift,
                _         => StickyWindow.ModifierKey.None
            };
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

        private void checkStickToInside_CheckedChanged(object sender, EventArgs e) {
            stickyWindow.StickToInside = checkStickToInside.Checked;
        }

        private void checkStickToOutside_CheckedChanged(object sender, EventArgs e) {
            stickyWindow.StickToOutside = checkStickToOutside.Checked;
        }

        private void checkStickyCorners_CheckedChanged(object sender, EventArgs e) {
            stickyWindow.StickyCorners = checkStickyCorners.Checked;
        }
    }
}
