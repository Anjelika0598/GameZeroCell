using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
	public partial class Form1 : Form
	{
		public static List<CellButton> cellButtons = new List<CellButton>();
		List<List<CellButton>> actions = new List<List<CellButton>>();

		Button buttonBack;
		Button buttonRestart;
		Button buttonNewGame;

		Button easyLvl;
		Button mediumLvl;
		Button hardLvl;

		Label score;
		string level;
		int unitedValue;
		int balanceValue;
		bool isGameScene = false;

		public Form1()
		{
			InitializeComponent();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			buttonBack = CreateButtonBack();
			buttonRestart = CreateButtonRestart();
			buttonNewGame = CreateButtonNewGame();
			easyLvl = CreateLvLButton(190, 190);
			easyLvl.Text = "easy";
			mediumLvl = CreateLvLButton(180, 250);
			mediumLvl.Text = "medium";
			hardLvl = CreateLvLButton(190, 310);
			hardLvl.Text = "hard";

			score = CreateLabel();
			CreateFieldCellButtons();
			HideField();
			LoadMenu(sender, e);
		}
		private void LoadMenu(object sender, EventArgs e)
		{
			ChangeScene(sender, e);
			actions.Clear();
		}
		private void InitializeValueForButton()
		{
			foreach (var button in cellButtons)
			{
				button.SetValue(RandomValue());
				button.InitializeColor();
			}
		}
		private void СalculateAllValuePoint()
		{
			foreach (var button in cellButtons) 
			{
				unitedValue += button.GetValue();
			}
		}
		private void CreateNewGame(object sender, EventArgs e)
		{
			level = ((Button)sender).Text;
			InitializeValueForButton();
			DownGradeBigNumbers();
			balanceValue = 0;
			unitedValue = 0;
			UpdateScore();
			СalculateAllValuePoint();
		}
		private void DownGradeBigNumbers()
		{
			foreach (var button in cellButtons)
			{
				if (IsTooBig(button))
					button.DownGrade();
			}
		}
		private bool IsTooBig(CellButton button)
		{
			var ambientbuttons = getActiveAmbientButtons(button);
			foreach (var ambientbutton in ambientbuttons)
			{
				if (ambientbutton.GetValue() >= button.GetValue())
					return false;
			}
			return true;
		}
		private void CreateFieldCellButtons()
		{
			int index = 0;
			while (index < 100)
			{
				CellButton cellButton = new CellButton();
				cellButton.Click += new EventHandler(ButtonClick);
				cellButtons.Add(cellButton);
				Controls.Add(cellButton);
				index++;
			}
		}
		private void ChangeScene(object sender, EventArgs e)
		{
			if (isGameScene == false)
			{
				buttonNewGame.Visible = false;
				buttonRestart.Visible = false;
				buttonBack.Visible = false;
				score.Visible = false;
				HideField();
				mediumLvl.Visible = true;
				hardLvl.Visible = true;
				easyLvl.Visible = true;
				isGameScene = true;
			}
			else 
			{
				buttonNewGame.Visible = true;
				buttonRestart.Visible = true;
				buttonBack.Visible = true;
				score.Visible = true;
				ShowField();
				mediumLvl.Visible = false;
				hardLvl.Visible = false;
				easyLvl.Visible = false;
				isGameScene = false;
			}
		}
		private Button CreateButtonNewGame()
		{
			var button = new Button();
			CreateMenuButton(button, 0, 5);
			button.Text = "New Game";
			button.Click += new EventHandler(LoadMenu);
			Controls.Add(button);
			return button;
		}
		private Button CreateButtonRestart()
		{
			var button = new Button();
			CreateMenuButton(button, 160, 5);
			button.Text = "Restart";
			button.Click += new EventHandler(Restart);
			Controls.Add(button);
			return button;
		}
		private Button CreateButtonBack()
		{
			var button = new Button();
			CreateMenuButton(button, 275, 5);
			button.Text = "back";
			button.AutoSize = true;
			button.Click += new EventHandler(Back);
			Controls.Add(button);
			return button;
		}
		private Label CreateLabel()
		{
			var label = new Label();
			label.Location = new Point(380, 5);
			label.AutoSize = true;
			label.Font = new Font("Arial", 20, FontStyle.Regular);
			label.ForeColor = Color.FromArgb(0, 183, 194);
			Controls.Add(label);
			return label;
		}
		private void CreateMenuButton(Button button, int PointX, int PointY)
		{
			button.Location = new Point(PointX, PointY);
			button.AutoSize = true;
			button.FlatAppearance.BorderSize = 0;
			button.FlatStyle = FlatStyle.Flat;
			button.Font = new Font("Arial", 20, FontStyle.Regular);
			button.ForeColor = Color.White;
			button.BackColor = Color.FromArgb(0, 183, 194);
		}
		private Button CreateLvLButton(int PointX, int PointY)
		{
			var button = new Button();
			CreateMenuButton(button, PointX, PointY);
			button.Size = new Size(150,30);
			button.FlatAppearance.BorderSize = 0;
			button.FlatStyle = FlatStyle.Flat;
			button.Font = new Font("Arial", 30, FontStyle.Regular);
			button.ForeColor = Color.White;
			button.BackColor = Color.FromArgb(0, 183, 194);
			button.Click += new EventHandler(CreateNewGame);
			button.Click += new EventHandler(ChangeScene);
			Controls.Add(button);
			return button;
		}
		private ComboBox CreateCombobox()
		{
			ComboBox comboBox = new ComboBox();
			comboBox.Items.Add("easy");
			comboBox.Items.Add("medium");
			comboBox.Items.Add("hard");
			comboBox.Location = new Point(200, 200);
			comboBox.Size = new Size(200, 40);
			comboBox.Font = new Font("Arial", 20, FontStyle.Regular);
			comboBox.Text = "Choose LvL";
			Controls.Add(comboBox);
			comboBox.SelectedIndexChanged += new EventHandler(CreateNewGame);
			comboBox.SelectedIndexChanged += new EventHandler(ChangeScene);
			return comboBox;
		}
		private int RandomValue()
		{
			Random rnd = new Random();
			int maxRandomNumber = 4;
			if (level == "easy")
			{
				maxRandomNumber = 4;
			}
			else if (level == "medium")
			{
				maxRandomNumber = 6;
			}
			else if (level == "hard")
			{
				maxRandomNumber = 8;
			}
			return (int)(rnd.NextDouble() * maxRandomNumber + 1);
		}
		public List<CellButton> getActiveAmbientButtons(CellButton button)
		{
			
			int x = button.coordX;
			int y = button.coordY;
			List<CellButton> ambientButtons = new List<CellButton>();
			if (button.GetValue() == 0)
				return ambientButtons;
			List<(int, int)> coordsList = new List<(int, int)>
			{
				(x - 1, y - 1),
				(x, y - 1),
				(x + 1, y - 1),
				(x - 1, y),
				(x + 1, y),
				(x - 1, y + 1),
				(x, y + 1),
				(x + 1, y + 1)
			};

			foreach (var coords in coordsList)
			{
				try
				{
					if (coords.Item1 >= 0 && coords.Item1 < 10 && coords.Item2 >= 0 && coords.Item2 < 10)
					{
						var but = cellButtons.Find(cell => cell.coordX == coords.Item1 && cell.coordY == coords.Item2);
						if (but != null && but.GetValue() > 0)
						{
							ambientButtons.Add(but);
						}
					}
				}
				catch (Exception e)
				{
					continue;
				}
			}
			return ambientButtons;
		}
		public static void HideField()
		{
			foreach (var cell in cellButtons)
			{
				cell.Visible = false;
			}
		}
		public static void ShowField()
		{
			foreach (var cell in cellButtons)
			{
				cell.Visible = true;
			}
		}
		public void ButtonClick(object sender, EventArgs e)
		{
			CellButton button = (CellButton)sender;
			var ambientButtons = getActiveAmbientButtons(button);
			if (ambientButtons.Count == 0 || button.GetValue() == 0)
				return;
			ambientButtons.Add(button);
			foreach (var item in ambientButtons)
			{
				item.DownGrade();
				++balanceValue;
			}
			UpdateScore();
			actions.Add(ambientButtons);
			CheckEnd();
		}
		private void CheckEnd()
		{
			if (!IsExistActiveCellButons())
			{
				score.ForeColor = Color.Red;
				score.Font = new Font("Arial", 30, FontStyle.Bold);
				if (!IsExistCellButtons())
				{
					score.Text = "WIN";
				}
				else
				{
					score.Text = "FALL";
				}
			}
		}
		private void UpdateScore()
		{
			if (unitedValue == 0)
				score.Text = "";
			else
				score.Text = ((balanceValue * 100) / unitedValue).ToString() + "%";
		}
		private bool IsExistActiveCellButons()
		{
			foreach (var cellButton in cellButtons)
			{
				if (getActiveAmbientButtons(cellButton).Count > 0)
					return true;
			}
			return false;
		}
		private bool IsExistCellButtons()
		{
			foreach (var cellbutton in cellButtons)
			{
				if (cellbutton.GetValue() > 0)
					return true;
			}
			return false;
		}
		private void Back(object sender, EventArgs e)
		{
			if (actions == null || actions.Count == 0)
				return;
			foreach (var button in actions.Last())
			{
				button.UpGrade();
				--balanceValue;
			}
			UpdateScore();
			actions.Remove(actions.Last());
			if (score.ForeColor == Color.Red)
			{
				score.Font = new Font("Arial", 20, FontStyle.Regular);
				score.ForeColor = Color.FromArgb(0, 183, 194);
			}

		}
		private void Restart(object sender, EventArgs e)
		{
			while (actions.Count != 0)
			{
				Back(sender, e);
			}
		}
	}

	public class CellButton : Button
	{
		public static int count = 0;
		public int coordX = count / 10;
		public int coordY = count % 10;
		public CellButton()
		{
			this.Location = new Point(coordX * 50, (coordY+ 1)  * 50);
			this.Size = new Size(50, 50);
			this.FlatAppearance.BorderSize = 0;
			this.FlatStyle = FlatStyle.Flat;
			this.Font = new Font("Arial", 30, FontStyle.Bold);
			this.ForeColor = Color.White;
			count++;
		}
		public int GetValue()
		{
			return this.Text == "" ? 0 : int.Parse(this.Text);
		}
		public void SetValue(int value)
		{
			if (value == 0)
				this.Text = "";
			else
				this.Text = value.ToString();
		}
		public void DownGrade()
		{
			if (this.GetValue() != 0)
			{
				this.SetValue(this.GetValue() - 1);
				InitializeColor();
			}
		}
		public void UpGrade()
		{
			this.SetValue(this.GetValue() + 1);
			InitializeColor();
		}
		public void InitializeColor()
		{
			int value = this.GetValue(); ;
			if (value < 3)
				this.BackColor = Color.FromArgb(253, 203, 158);
			else if (value < 5)
				this.BackColor = Color.FromArgb(0, 183, 194);
			else if (value < 7)
				this.BackColor = Color.FromArgb(15, 76, 117);
			else if (value < 9)
				this.BackColor = Color.FromArgb(27, 38, 44);
		}
	}
}
