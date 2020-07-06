namespace t0001
{
	partial class MainWin
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.btnPress = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnPress
			// 
			this.btnPress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btnPress.Location = new System.Drawing.Point(13, 14);
			this.btnPress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnPress.Name = "btnPress";
			this.btnPress.Size = new System.Drawing.Size(358, 333);
			this.btnPress.TabIndex = 0;
			this.btnPress.Text = "おしてみなよ";
			this.btnPress.UseVisualStyleBackColor = true;
			this.btnPress.Click += new System.EventHandler(this.btnPress_Click);
			// 
			// MainWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 361);
			this.Controls.Add(this.btnPress);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "MainWin";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "t0001";
			this.Load += new System.EventHandler(this.MainWin_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnPress;
	}
}

