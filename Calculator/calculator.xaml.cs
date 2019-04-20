using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private float preValue = 0;
        private bool operationDetected = false;
        private bool newOperation = false;
        private string newNumber="";
        private string preOperation = "";
        private int DecimalPointCount = 0;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumericButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            if(b.Content.ToString() == "back")
            {
                string temp = "";
                for(int i = 0; i < DisplayData.Text.Length - 1; i++)
                {
                    temp += DisplayData.Text[i].ToString();
                }
                DisplayData.Text = temp;
                if (DisplayData.Text == "") { DisplayData.Text = "0"; }
                return;
            }

            if (b.Content.ToString() == "." && DecimalPointCount < 1)
            {
                DecimalPointCount++;
                DisplayData.Text += b.Content;
                newNumber = DisplayData.Text;
                operationDetected = false;
                newOperation = true;
            }
            else if(b.Content.ToString() != ".")
            {

                if (DisplayData.Text == "0" || operationDetected)
                {
                    DisplayData.Text = "";
                    DecimalPointCount = 0;
                    DisplayData.Text += b.Content;
                }
                else
                {
                    DisplayData.Text += b.Content;
                }
                newNumber = DisplayData.Text;
                operationDetected = false;
                newOperation = true;
            }
        }

        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            if (newOperation && b.Content.ToString() != "CE" && b.Content.ToString() != "C")
            {
                if (preOperation == "")
                {
                    if(DisplayPreData.Text == "") {
                        operations(DisplayData.Text,"+", b.Content.ToString());
                        operationDetected = true;
                    }
                    else {
                        operations(DisplayData.Text, b.Content.ToString(), b.Content.ToString());
                        operationDetected = true;
                    }
                }
                else
                {
                    operations(DisplayData.Text, preOperation, b.Content.ToString());
                    operationDetected = true;
                }
            }
           

            if (!newOperation)
            {
                if (b.Content.ToString() != "CE" && b.Content.ToString() != "C" && b.Content.ToString() != "=")
                {
                    DisplayPreData.Text = changeOperation(DisplayPreData.Text, b.Content.ToString());
                }else if(b.Content.ToString() == "CE")
                {
                    DisplayData.Text = "0";
                }
                else if(b.Content.ToString() == "C")
                {
                    DisplayData.Text = "0";
                    preValue = 0;
                    DisplayPreData.Text = "";
                    preOperation = "";
                    DecimalPointCount = 0;
                }
                else if (b.Content.ToString() == "=")
                {
                    DisplayPreData.Text += DisplayData.Text;
                }
            }

            newOperation = false;
            if (b.Content.ToString() != "CE" && b.Content.ToString() != "C" && b.Content.ToString() != "=")
            {
                preOperation = b.Content.ToString();
            }
        }


        private void operations(string value, string preoperation,string cOp)
        {
            float val = float.Parse(value);
            switch (preoperation)
            {
                case "+":
                    preValue += val;
                    DisplayData.Text = preValue.ToString();
                    break;
                case "-":
                    preValue -= val;
                    DisplayData.Text = preValue.ToString();
                    break;
                case "X":
                    preValue *= val;
                    DisplayData.Text = preValue.ToString();
                    break;
                case "/":
                    preValue /= val;
                    DisplayData.Text = preValue.ToString();                   
                    break;
                case "=":
                    string extractOp = DisplayPreData.Text[DisplayPreData.Text.Length - 1].ToString();
                    operations(newNumber, extractOp,extractOp);
                    DisplayData.Text = preValue.ToString();
                    DisplayPreData.Text += DisplayData.Text;
                    preValue = 0;
                    break;
                case "MOD":
                    float store = preValue % val;
                    preValue = store;
                    DisplayData.Text = preValue.ToString();
                    break;
            }
            if (preoperation != "=" && preoperation != "CE" && preoperation != "C")
                DisplayPreData.Text += " " + newNumber + " " + cOp;

            string v = DisplayData.Text;
            if (Modetext.Text=="Frac")
            {
                fractionMethod(v);
            }
        }

        private void fractionMethod(string v)
        {
            if (checkIfDecimal(v))
            {
                int c = countAfterDecimalPlace(v);
                double multiplier = Math.Pow(10, Convert.ToDouble(c));
                float multipliedVal = float.Parse(v) * (int)multiplier;
                float hcf = HCF(multipliedVal, (int)multiplier);
                //MessageBox.Show(multipliedVal.ToString()+ " " + hcf.ToString());

                if ((multiplier / hcf) != 1)
                {
                    DisplayData.Text = (multipliedVal / hcf).ToString() + "/" + (multiplier / hcf).ToString();
                }
            }
        }

        private bool checkIfDecimal(string value)
        {
            for(int i = 0; i < value.Length; i++)
            {
                if(value[i].ToString() == ".")
                {
                    return true;
                }
            }
            return false;
        }

        private int countAfterDecimalPlace(string value)
        {
            int count = 0;
            bool check = false; ;
            for (int i = 0; i < value.Length; i++)
            {              
                if (value[i].ToString() == "." || check)
                {
                    count++;
                    check = true;
                }
            }
          //  MessageBox.Show((count - 2).ToString());
            return count-1;
        }

        private float HCF(float a, float b)
        {
            float Num1 = a;
            float Num2 = b;
            float temp;
            while (Num2 != 0)
            {
                temp = Num2;
                Num2 = Num1 % Num2;
                Num1 = temp;
            }
            return Num1;
        }

        private string changeOperation(string val,string op)
        {
            string e = "";
            for(int i = 0; i < val.Length-1; i++)
            {
                e += val[i].ToString();
            }
            e += op;
            return e;
        }

        private void modeButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            if(b.Content.ToString() == "Deci/Frac")
            {
                if(Modetext.Text == "Deci") {
                    Modetext.Text = "Frac";
                    fractionMethod(DisplayData.Text);
                }
                else { Modetext.Text = "Deci";
                    DisplayData.Text = preValue.ToString();
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                string temp = "";
                for (int i = 0; i < DisplayData.Text.Length - 1; i++)
                {
                    temp += DisplayData.Text[i].ToString();
                }
                DisplayData.Text = temp;
                if (DisplayData.Text == "") { DisplayData.Text = "0"; }
                return;
            }

            if (e.Key == Key.Decimal && DecimalPointCount < 1)
            {
                DecimalPointCount++;
                DisplayData.Text += ".";
                newNumber = DisplayData.Text;
                operationDetected = false;
                newOperation = true;
            }
            else if (e.Key != Key.Decimal)
            {

                if (DisplayData.Text == "0" || operationDetected)
                {
                    DisplayData.Text = "";
                    DecimalPointCount = 0;
                    if ((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                    {
                        if (e.Key != Key.Enter)
                        {
                            string extract = e.Key.ToString();
                            DisplayData.Text += extract[extract.Length - 1].ToString();
                        }
                    }
                }
                else
                {
                    if ((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                    {
                        if (e.Key != Key.Enter)
                        {
                            string extract = e.Key.ToString();
                            DisplayData.Text += extract[extract.Length - 1].ToString();
                        }
                    }
                }
                newNumber = DisplayData.Text;
                operationDetected = false;
                newOperation = true;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            //operations 
            try
            {
                if (e.Key == Key.Add || e.Key == Key.OemPlus)
                {
                    keyOp(e, "+");
                }
                else if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
                {
                    keyOp(e, "-");
                }
                else if (e.Key == Key.Multiply)
                {
                    keyOp(e, "X");
                }
                else if (e.Key == Key.Divide)
                {
                    keyOp(e, "/");
                }
            }
            catch {  }
           
        }

        private void keyOp(KeyEventArgs e,string operation)
        {
            if (newOperation && e.Key.ToString() != "CE" && e.Key.ToString() != "C")
            {
                if (preOperation == "")
                {
                    if (DisplayPreData.Text == "")
                    {
                        operations(DisplayData.Text, "+", operation);
                        operationDetected = true;
                    }
                    else
                    {
                        operations(DisplayData.Text, operation, operation);
                        operationDetected = true;
                    }
                }
                else
                {
                    operations(DisplayData.Text, preOperation, operation);
                    operationDetected = true;
                }
            }


            if (!newOperation)
            {
                if (e.Key.ToString() != "CE" && e.Key.ToString() != "C" && e.Key.ToString() != "=")
                {
                    DisplayPreData.Text = changeOperation(DisplayPreData.Text, operation);
                }
                else if (e.Key.ToString() == "CE")
                {
                    DisplayData.Text = "0";
                }
                else if (e.Key.ToString() == "C")
                {
                    DisplayData.Text = "0";
                    preValue = 0;
                    DisplayPreData.Text = "";
                    preOperation = "";
                    DecimalPointCount = 0;
                }
                else if (e.Key == Key.Enter)
                {
                    DisplayPreData.Text += DisplayData.Text;
                }
            }

            newOperation = false;
            if (operation != "CE" && operation != "C" && operation != "=")
            {
                preOperation = operation;
            }
        }
    }
}
