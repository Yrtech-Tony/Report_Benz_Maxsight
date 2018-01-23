namespace XHX.View
{
    partial class SingleShopReport
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grdShop = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cboProjects = new DevExpress.XtraEditors.ComboBoxEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.btnShopSubject = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnModule = new DevExpress.XtraEditors.ButtonEdit();
            this.btnShopCharter = new DevExpress.XtraEditors.SimpleButton();
            this.btnAreaCharter = new DevExpress.XtraEditors.SimpleButton();
            this.btnAllCharter = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdShop)).BeginInit();
            this.grdShop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboProjects.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnModule.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grdShop
            // 
            this.grdShop.Controls.Add(this.labelControl1);
            this.grdShop.Controls.Add(this.cboProjects);
            this.grdShop.Dock = System.Windows.Forms.DockStyle.Top;
            this.grdShop.Location = new System.Drawing.Point(5, 5);
            this.grdShop.Margin = new System.Windows.Forms.Padding(0);
            this.grdShop.Name = "grdShop";
            this.grdShop.Size = new System.Drawing.Size(982, 42);
            this.grdShop.TabIndex = 11;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl1.Location = new System.Drawing.Point(24, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 11;
            this.labelControl1.Text = "项目名";
            // 
            // cboProjects
            // 
            this.cboProjects.Location = new System.Drawing.Point(71, 12);
            this.cboProjects.Name = "cboProjects";
            this.cboProjects.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboProjects.Size = new System.Drawing.Size(100, 21);
            this.cboProjects.TabIndex = 12;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.btnShopSubject);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Controls.Add(this.btnModule);
            this.groupControl2.Controls.Add(this.btnShopCharter);
            this.groupControl2.Controls.Add(this.btnAreaCharter);
            this.groupControl2.Controls.Add(this.btnAllCharter);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(5, 47);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(982, 125);
            this.groupControl2.TabIndex = 92;
            // 
            // btnShopSubject
            // 
            this.btnShopSubject.Location = new System.Drawing.Point(733, 66);
            this.btnShopSubject.Name = "btnShopSubject";
            this.btnShopSubject.Size = new System.Drawing.Size(129, 44);
            this.btnShopSubject.TabIndex = 94;
            this.btnShopSubject.Text = "经销商体系得分";
            this.btnShopSubject.Click += new System.EventHandler(this.btnShopSubject_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Options.UseTextOptions = true;
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl2.Location = new System.Drawing.Point(12, 27);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 93;
            this.labelControl2.Text = "模板路径";
            // 
            // btnModule
            // 
            this.btnModule.EditValue = "";
            this.btnModule.Location = new System.Drawing.Point(71, 24);
            this.btnModule.Name = "btnModule";
            this.btnModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.btnModule.Size = new System.Drawing.Size(287, 21);
            this.btnModule.TabIndex = 92;
            this.btnModule.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnModule_ButtonClick);
            // 
            // btnShopCharter
            // 
            this.btnShopCharter.Location = new System.Drawing.Point(597, 66);
            this.btnShopCharter.Name = "btnShopCharter";
            this.btnShopCharter.Size = new System.Drawing.Size(129, 44);
            this.btnShopCharter.TabIndex = 91;
            this.btnShopCharter.Text = "经销商环节得分";
            this.btnShopCharter.Click += new System.EventHandler(this.btnShopCharter_Click);
            // 
            // btnAreaCharter
            // 
            this.btnAreaCharter.Location = new System.Drawing.Point(482, 66);
            this.btnAreaCharter.Name = "btnAreaCharter";
            this.btnAreaCharter.Size = new System.Drawing.Size(109, 44);
            this.btnAreaCharter.TabIndex = 90;
            this.btnAreaCharter.Text = "区域环节得分";
            this.btnAreaCharter.Click += new System.EventHandler(this.btnAreaCharter_Click);
            // 
            // btnAllCharter
            // 
            this.btnAllCharter.Location = new System.Drawing.Point(361, 66);
            this.btnAllCharter.Name = "btnAllCharter";
            this.btnAllCharter.Size = new System.Drawing.Size(115, 45);
            this.btnAllCharter.TabIndex = 88;
            this.btnAllCharter.Text = "全国环节得分";
            this.btnAllCharter.Click += new System.EventHandler(this.btnAllCharter_Click);
            // 
            // SingleShopReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.grdShop);
            this.Name = "SingleShopReport";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(992, 555);
            ((System.ComponentModel.ISupportInitialize)(this.grdShop)).EndInit();
            this.grdShop.ResumeLayout(false);
            this.grdShop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboProjects.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnModule.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl grdShop;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cboProjects;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton btnAllCharter;
        private DevExpress.XtraEditors.SimpleButton btnAreaCharter;
        private DevExpress.XtraEditors.SimpleButton btnShopCharter;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ButtonEdit btnModule;
        private DevExpress.XtraEditors.SimpleButton btnShopSubject;
    }
}
