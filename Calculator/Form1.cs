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
        None
    }

    public enum OpMode
    {
        Plus,
        Minus,
        None
    }

    public enum UseOp
    {
        Yes,
        No,
        None
    }

    public partial class Form1 : Form
    {
        int max = int.MaxValue;
        int min = int.MinValue;

        int resultcalc = 0;
        int save1 = 0;
        int save2 = 0;

        string op = "";

        BtnClick endclick = BtnClick.Number;
        OpMode mode = OpMode.None;
        UseOp useop = UseOp.None;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnNumber_click (object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string txtnumber = btn.Text;
            string txt = txtResult.Text;
            int s;


            if (txt == "0" || endclick == BtnClick.Equal || endclick == BtnClick.Op)
            {
                if(endclick == BtnClick.Equal)
                {
                    txtHistory.Text = "";
                }
                txtResult.Text = txtnumber;
            }
            else
            {
                if(int.TryParse(txt + txtnumber, out s) == true)
                {
                    if(min < s && s < max)
                    {
                        txt += txtnumber;
                    }
                    txtResult.Text = txt;
                }
            }
            endclick = BtnClick.Number;
        }

        private void btnOp_click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if(int.TryParse(txtResult.Text, out resultcalc) == false)
            {
                txtResult.Text = "int 범위를 벗어남";
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

                txtHistory.Text = save1 + op;
            }
            else
            {
                txtHistory.Text = save1 + op;
            }

            switch (op)
            {
                case "+":
                    mode = OpMode.Plus;
                    break;
                case "-":
                    mode = OpMode.Minus;
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
            resultcalc = int.Parse(txtResult.Text);
            if (endclick != BtnClick.Op)
            {
                if (useop != UseOp.Yes || endclick == BtnClick.Equal)
                {
                    save1 = resultcalc;
                    txtHistory.Text = "=" + resultcalc;
                }
                else
                {
                    save2 = resultcalc;
                    resultcalc = Calc(save1, save2, mode);
                    txtHistory.Text = save1 + op + save2 + "=" + resultcalc;
                    save1 = resultcalc;
                }

            }
            txtResult.Text = save1.ToString();
            op = "";
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
            useop = UseOp.None;
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
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
            int a = int.Parse(txtResult.Text);
            txtResult.Text = (-a).ToString();
        }

        public int Calc(int save1, int save2, OpMode mode)
        {
            int result;
            switch(mode)
            {
                case OpMode.Plus:
                    result = save1 + save2;
                    break;
                case OpMode.Minus:
                    result = save1 - save2;
                    break;
                default:
                    result = 0;
                    break;
            }

            return result;


        }
    }
}
