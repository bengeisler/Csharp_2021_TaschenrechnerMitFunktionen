using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Taschenrechner
{
	public partial class Form1 : Form
	{
		private double LetztesErgebnis;
		private double Summe;

		public Form1()
		{
			InitializeComponent();
		}

		private void btnLöschen_Click(object sender, EventArgs e)
		{
			// Liste Verlauf löschen
			lstVerlauf.Items.Clear();

			// Globale Variablen zurücksetzen
			Summe = 0.0;
			LetztesErgebnis = 0.0;

			EingabefelderFürNeuenEintragVorbereiten();

			SummeAktualisieren();

			MittelwertAktualisieren();
		}

		private void btnAns_Click(object sender, EventArgs e)
		{
			txtZahl1.Text = LetztesErgebnis.ToString();

			// Bonus: Fokus in das zweite Eingabefeld setzen
			txtZahl2.Focus();
		}

		private void btnErgebnis_Click(object sender, EventArgs e)
		{
			try
			{
				// Beide Zahlen einlesen und in passenden Datentypen konvertieren
				var zahl1 = Convert.ToDouble(txtZahl1.Text);
				var zahl2 = Convert.ToDouble(txtZahl2.Text);

				LetztesErgebnis = ErgebnisBerechnen(zahl1, zahl2);

				var rechenweg = RechenwegAusgeben(zahl1, zahl2, LetztesErgebnis);
				lstVerlauf.Items.Add(rechenweg);

				SummeAktualisieren(LetztesErgebnis);

				MittelwertAktualisieren();

				EingabefelderFürNeuenEintragVorbereiten();

			}
			catch (FormatException)
			{
				lblFehler.Text = "Bitte geben Sie gültige Zahlen ein!";
			}
			catch (DivideByZeroException)
			{
				lblFehler.Text = "Division durch 0 ist unzulässig";
			}
			catch (Exception ex)
			{
				lblFehler.Text = "Fehler: " + ex.Message;
			}
		}

		private double ErgebnisBerechnen(double zahl1, double zahl2)
		{
			if (rBtnPlus.Checked) return zahl1 + zahl2;
			else if (rBtnMinus.Checked) return zahl1 - zahl2;
			else if (rBtnMal.Checked) return zahl1 * zahl2;
			else if (rBtnGeteilt.Checked)
			{
				if (zahl2 == 0.0) throw new DivideByZeroException();
				return zahl1 / zahl2;
			}
			else throw new Exception("Keine Rechenoperation ausgewählt!");
		}

		private string RechenwegAusgeben(double zahl1, double zahl2, double ergebnis)
		{
			string rechenoperation;

			if (rBtnPlus.Checked) rechenoperation = " + ";
			else if (rBtnMinus.Checked) rechenoperation = " - ";
			else if (rBtnMal.Checked) rechenoperation = " * ";
			else if (rBtnGeteilt.Checked) rechenoperation = " / ";
			else throw new Exception("Keine Rechenoperation ausgewählt!");

			return zahl1 + rechenoperation + zahl2 + " = " + ergebnis;
		}

		private void SummeAktualisieren(double letztesErgebnis = 0.0)
		{
			Summe += letztesErgebnis;
			lblSumme.Text = "Summe: " + Summe;
		}

		private void MittelwertAktualisieren()
		{
			if (lstVerlauf.Items.Count > 0) lblMittelwert.Text = "Mittelwert: " + (Summe / lstVerlauf.Items.Count);
			else lblMittelwert.Text = "Mittelwert: 0";
		}

		private void EingabefelderFürNeuenEintragVorbereiten()
		{
			// Eingabefelder leeren
			txtZahl1.Text = "";
			txtZahl2.Text = "";

			// Bonus: Fokus in erstes Feld setzen
			txtZahl1.Focus();
		}

		private void FehlermeldungRücksetzen(object sender, EventArgs e)
		{
			lblFehler.Text = "";
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			SummeAktualisieren();
			MittelwertAktualisieren();
		}

		private void txtZahl2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) btnErgebnis_Click(sender, e);
		}
	}
}
