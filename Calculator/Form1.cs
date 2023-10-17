using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public enum BtnClick
    {
        Number,
        Op,
        Equal,
        Dot,
        None
    }

    public enum OpMode
    {
        Plus,
        Minus,
        Multiply,
        Divide,
        None
    }

    public enum UseOp
    {
        Yes,
        No,
    }

    public partial class Form1 : Form
    {
        double max = double.MaxValue;
        double min = double.MinValue;

        decimal resultcalc = 0;
        decimal save1 = 0;
        decimal save2 = 0;

        string op = "";

        BtnClick endclick = BtnClick.Number;
        OpMode mode = OpMode.None;
        UseOp useop = UseOp.No;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnNumber_click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string txtnumber = btn.Text;
            string txt = txtResult.Text;
            string totaltxt;
            string conversiontxt;
            decimal totaldouble;



            if (txt == "0" || endclick == BtnClick.Equal || endclick == BtnClick.Op)
            {
                if (endclick == BtnClick.Equal)
                {
                    txtHistory.Text = "";
                }
                txtResult.Text = txtnumber.ToString();
            }
            else
            {
                if (double.TryParse(txt + txtnumber, out double s) == true)
                {
                    totaltxt = txt + txtnumber;
                    totaldouble = decimal.Parse(totaltxt);
                    conversiontxt = string.Format("{0}", totaldouble);
                    if (min < s && s < max)
                    {
                        if(conversiontxt.Length == 19 && (conversiontxt.Contains(".") || conversiontxt.Contains("-")))
                        {
                            endclick = BtnClick.Number;
                            return;
                        }
                        else if (conversiontxt.Length == 17 && (conversiontxt.Contains(".") == false || conversiontxt.Contains("-") == false))
                        {
                            endclick = BtnClick.Number;
                            return;
                        }
                        else
                        {
                            if (conversiontxt.Contains("0.0"))
                            {
                                txtResult.Text += txtnumber;
                            }
                            else
                            {
                                txtResult.Text = string.Format("{0:#,##0.################}", totaldouble);
                            }
                        }
                    }
                }
            }
            endclick = BtnClick.Number;
        }

        private void btnOp_click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if(decimal.TryParse(txtResult.Text, out resultcalc) == false)
            {
                return;
            }

            op = btn.Text;
            if (endclick != BtnClick.Op)
            {
                if (useop != UseOp.Yes)
                {
                    save1 = resultcalc;
                }
                else
                {
                    save2 = resultcalc;
                    resultcalc = Calc(save1, save2, mode);
                    save1 = resultcalc;
                }

                txtHistory.Text = string.Format("{0:#,##0.################}{1}", save1, op);
            }
            else
            {
                txtHistory.Text = string.Format("{0:#,##0.################}{1}", save1, op); ;
            }

            switch (op)
            {
                case "+":
                    mode = OpMode.Plus;
                    break;
                case "-":
                    mode = OpMode.Minus;
                    break;
                case "×":
                    mode = OpMode.Multiply;
                    break;
                case "÷":
                    mode = OpMode.Divide;
                    break;
                default:
                    mode = OpMode.None;
                    break;
            }

            endclick = BtnClick.Op;
            useop = UseOp.Yes;
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            resultcalc = decimal.Parse(txtResult.Text);
            if (endclick != BtnClick.Op)
            {
                if (useop == UseOp.No && op == "")
                {
                    save1 = resultcalc;
                    txtHistory.Text = string.Format("{0:#,##0.################}=", resultcalc);
                }
                else
                {
                    if (save2 == 0)
                    {
                        save2 = resultcalc;
                    }
                    resultcalc = Calc(save1, save2, mode);
                    txtHistory.Text = string.Format("{0:#,##0.################}{1}{2:#,##0.################}=", save1, op, save2);
                    save1 = resultcalc;
                }
            }
            else
            {
                save2 = resultcalc;
                resultcalc = Calc(save1, save2, mode);
                txtHistory.Text = string.Format("{0:#,##0.################}{1}{2:#,##0.################}=", save1, op, save2);
                save1 = resultcalc;
            }
            txtResult.Text = string.Format("{0:#,##0.################}", save1);
            //op = "";
            endclick = BtnClick.Equal;
            useop = UseOp.No;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtResult.Text = "0";
            txtHistory.Text = "";
            resultcalc = 0;
            save1 = 0;
            save2 = 0;
            op = "";
            endclick = BtnClick.None;
            mode = OpMode.None;
            useop = UseOp.No;
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            if(endclick == BtnClick.Op)
            {
                return;
            }
            txtResult.Text = txtResult.Text.Remove(txtResult.Text.Length - 1);
            string text = txtResult.Text;
            if (text.Length == 0 || 
                (text.Length == 1 && text.Contains("-")))
            {
                txtResult.Text = "0";
            }
        }

        private void btnbtnPnN_Click(object sender, EventArgs e)
        {
            decimal a = decimal.Parse(txtResult.Text);
            txtResult.Text = string.Format("{0:#,##0.################}", -1 * a);
        }

        public decimal Calc(decimal save1, decimal save2, OpMode mode)
        {
            decimal result;
            switch(mode)
            {
                case OpMode.Plus:
                    result = save1 + save2;
                    break;
                case OpMode.Minus:
                    result = save1 - save2;
                    break;
                case OpMode.Multiply:
                    result = save1 * save2;
                    break;
                case OpMode.Divide:
                    result = save1 / save2;
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;


        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            string txt = txtResult.Text;
            if (txt.Contains(".") && endclick == BtnClick.Equal)
            {
                txtResult.Text = "0.";
                endclick = BtnClick.Dot;
                return;
            }
            else if (txt.Contains(".")) 
            {
                endclick = BtnClick.Dot;
                return;
            }
            else
            {
                txtResult.Text += ".";
                endclick = BtnClick.Dot;
            }
        }
    }
}
